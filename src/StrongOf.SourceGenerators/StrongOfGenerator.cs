// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StrongOf.SourceGenerators;

/// <summary>
/// Incremental source generator that turns
/// <code>
/// [StrongGuid] public partial class UserId;
/// </code>
/// into a fully-implemented strong type without any Expression-based fallback,
/// making the resulting type fully Native AOT and trim safe.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class StrongOfGenerator : IIncrementalGenerator
{
    private const string AttributeNamespace = "StrongOf.SourceGeneration";
    private const string GenericStrongAttributeName = "StrongAttribute`1";
    private const string TypeofStrongAttributeName = "StrongAttribute";
    private const string GeneratedFileSuffix = ".g.cs";

    /// <summary>
    /// Mapping table: marker attribute -> (base class name, fully-qualified primitive type).
    /// </summary>
    private static readonly (string AttributeName, string BaseTypeName, string PrimitiveType)[] s_kinds =
    [
        ("StrongBooleanAttribute",        "StrongBoolean",        "global::System.Boolean"),
        ("StrongCharAttribute",           "StrongChar",           "global::System.Char"),
        ("StrongDateTimeAttribute",       "StrongDateTime",       "global::System.DateTime"),
        ("StrongDateTimeOffsetAttribute", "StrongDateTimeOffset", "global::System.DateTimeOffset"),
        ("StrongDecimalAttribute",        "StrongDecimal",        "global::System.Decimal"),
        ("StrongDoubleAttribute",         "StrongDouble",         "global::System.Double"),
        ("StrongGuidAttribute",           "StrongGuid",           "global::System.Guid"),
        ("StrongInt32Attribute",          "StrongInt32",          "global::System.Int32"),
        ("StrongInt64Attribute",          "StrongInt64",          "global::System.Int64"),
        ("StrongStringAttribute",         "StrongString",         "global::System.String"),
        ("StrongTimeSpanAttribute",       "StrongTimeSpan",       "global::System.TimeSpan"),
    ];

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Build one dedicated incremental pipeline per marker attribute. We copy tuple values
        // into locals to avoid closure-capture pitfalls across loop iterations.
        foreach ((string attributeName, string baseTypeName, string primitiveType) in s_kinds)
        {
            string fullyQualifiedAttribute = AttributeNamespace + "." + attributeName;
            string baseType = baseTypeName;
            string primitive = primitiveType;

            IncrementalValuesProvider<TargetType?> targets = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    fullyQualifiedAttribute,
                    static (node, _) => node is ClassDeclarationSyntax,
                    (ctx, ct) => ToTarget(ctx, baseType, primitive, ct));

            IncrementalValuesProvider<TargetType> validTargets = targets
                .Where(static t => t is not null)
                .Select(static (t, _) => t!.Value);

            context.RegisterSourceOutput(validTargets, static (spc, target) => Emit(spc, target));
        }

        // Generic syntax: [Strong<TTarget>]
        IncrementalValuesProvider<TargetType?> genericTargets = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeNamespace + "." + GenericStrongAttributeName,
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, ct) => ToGenericTarget(ctx, ct));

        IncrementalValuesProvider<TargetType> validGenericTargets = genericTargets
            .Where(static t => t is not null)
            .Select(static (t, _) => t!.Value);

        context.RegisterSourceOutput(validGenericTargets, static (spc, target) => Emit(spc, target));

        // Runtime-type syntax: [Strong(typeof(TTarget))]
        IncrementalValuesProvider<TargetType?> typeofTargets = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeNamespace + "." + TypeofStrongAttributeName,
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, ct) => ToTypeofTarget(ctx, ct));

        IncrementalValuesProvider<TargetType> validTypeofTargets = typeofTargets
            .Where(static t => t is not null)
            .Select(static (t, _) => t!.Value);

        context.RegisterSourceOutput(validTypeofTargets, static (spc, target) => Emit(spc, target));
    }

    /// <summary>
    /// Creates a generation target for primitive-specific marker attributes such as
    /// <c>[StrongGuid]</c> or <c>[StrongString]</c>.
    /// </summary>
    /// <param name="context">Current generator attribute context.</param>
    /// <param name="baseTypeName">StrongOf base type to inherit from.</param>
    /// <param name="primitiveType">Fully-qualified primitive CLR type name.</param>
    /// <param name="cancellationToken">Cancellation token for cooperative cancellation.</param>
    /// <returns>A populated <see cref="TargetType"/> when the declaration is valid; otherwise <see langword="null"/>.</returns>
    private static TargetType? ToTarget(
        GeneratorAttributeSyntaxContext context,
        string baseTypeName,
        string primitiveType,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        if (context.TargetNode is not ClassDeclarationSyntax classDecl)
        {
            return null;
        }

        if (TryResolveMultipleMarkerDiagnosticTarget(context, typeSymbol, classDecl, out TargetType? multipleMarkerTarget))
        {
            return multipleMarkerTarget;
        }

        // Manual token scan keeps this hot path allocation free.
        // We intentionally avoid LINQ here because generators run frequently while typing.
        bool isPartial = false;
        foreach (SyntaxToken modifier in classDecl.Modifiers)
        {
            if (modifier.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword))
            {
                isPartial = true;
                break;
            }
        }

        // Top-level type only (no nested types) - keeps generated file naming sane.
        bool isNested = typeSymbol.ContainingType is not null;

        string? namespaceName = typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } ns
            ? ns.ToDisplayString()
            : null;

        return new TargetType(
            TypeName: typeSymbol.Name,
            Namespace: namespaceName,
            BaseTypeName: baseTypeName,
            PrimitiveType: primitiveType,
            IsPartial: isPartial,
            IsNested: isNested,
            DiagnosticLocation: classDecl.Identifier.GetLocation());
    }

    /// <summary>
    /// Creates a generation target for the generic marker syntax <c>[Strong&lt;TTarget&gt;]</c>.
    /// </summary>
    /// <param name="context">Current generator attribute context.</param>
    /// <param name="cancellationToken">Cancellation token for cooperative cancellation.</param>
    /// <returns>
    /// A populated <see cref="TargetType"/>, a diagnostic target for unsupported targets,
    /// or <see langword="null"/> when the declaration cannot be processed by this pipeline.
    /// </returns>
    private static TargetType? ToGenericTarget(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        if (context.TargetNode is not ClassDeclarationSyntax classDecl)
        {
            return null;
        }

        if (TryResolveMultipleMarkerDiagnosticTarget(context, typeSymbol, classDecl, out TargetType? multipleMarkerTarget))
        {
            return multipleMarkerTarget;
        }

        if (context.Attributes.Length == 0)
        {
            return null;
        }

        AttributeData attribute = context.Attributes[0];
        if (attribute.AttributeClass?.TypeArguments.Length != 1)
        {
            return null;
        }

        ITypeSymbol targetType = attribute.AttributeClass.TypeArguments[0];
        if (!TryMapTargetType(targetType, out string baseTypeName, out string primitiveType))
        {
            // Returning a diagnostic target keeps reporting centralized in Emit(), so every
            // pipeline follows the same diagnostic ordering and formatting behavior.
            return new TargetType(
                TypeName: typeSymbol.Name,
                Namespace: typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } ns ? ns.ToDisplayString() : null,
                BaseTypeName: string.Empty,
                PrimitiveType: string.Empty,
                IsPartial: false,
                IsNested: false,
                DiagnosticLocation: classDecl.Identifier.GetLocation(),
                UnsupportedTypeName: targetType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
        }

        bool isPartial = false;
        foreach (SyntaxToken modifier in classDecl.Modifiers)
        {
            if (modifier.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword))
            {
                isPartial = true;
                break;
            }
        }

        bool isNested = typeSymbol.ContainingType is not null;
        string? namespaceName = typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } targetNs
            ? targetNs.ToDisplayString()
            : null;

        return new TargetType(
            TypeName: typeSymbol.Name,
            Namespace: namespaceName,
            BaseTypeName: baseTypeName,
            PrimitiveType: primitiveType,
            IsPartial: isPartial,
            IsNested: isNested,
            DiagnosticLocation: classDecl.Identifier.GetLocation());
    }

    /// <summary>
    /// Creates a generation target for runtime-type marker syntax <c>[Strong(typeof(TTarget))]</c>.
    /// </summary>
    /// <param name="context">Current generator attribute context.</param>
    /// <param name="cancellationToken">Cancellation token for cooperative cancellation.</param>
    /// <returns>
    /// A populated <see cref="TargetType"/>, a diagnostic target for unsupported targets,
    /// or <see langword="null"/> when the declaration cannot be processed.
    /// </returns>
    private static TargetType? ToTypeofTarget(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not INamedTypeSymbol typeSymbol)
        {
            return null;
        }

        if (context.TargetNode is not ClassDeclarationSyntax classDecl)
        {
            return null;
        }

        if (TryResolveMultipleMarkerDiagnosticTarget(context, typeSymbol, classDecl, out TargetType? multipleMarkerTarget))
        {
            return multipleMarkerTarget;
        }

        if (context.Attributes.Length == 0)
        {
            return null;
        }

        AttributeData attribute = context.Attributes[0];
        if (attribute.ConstructorArguments.Length != 1)
        {
            return null;
        }

        TypedConstant argument = attribute.ConstructorArguments[0];
        if (argument.Kind != TypedConstantKind.Type || argument.Value is not ITypeSymbol targetType)
        {
            return null;
        }

        if (!TryMapTargetType(targetType, out string baseTypeName, out string primitiveType))
        {
            // Keep unsupported-type handling deferred to Emit() for deterministic diagnostics.
            return new TargetType(
                TypeName: typeSymbol.Name,
                Namespace: typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } ns ? ns.ToDisplayString() : null,
                BaseTypeName: string.Empty,
                PrimitiveType: string.Empty,
                IsPartial: false,
                IsNested: false,
                DiagnosticLocation: classDecl.Identifier.GetLocation(),
                UnsupportedTypeName: targetType.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat));
        }

        bool isPartial = false;
        foreach (SyntaxToken modifier in classDecl.Modifiers)
        {
            if (modifier.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword))
            {
                isPartial = true;
                break;
            }
        }

        bool isNested = typeSymbol.ContainingType is not null;
        string? namespaceName = typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } targetNs
            ? targetNs.ToDisplayString()
            : null;

        return new TargetType(
            TypeName: typeSymbol.Name,
            Namespace: namespaceName,
            BaseTypeName: baseTypeName,
            PrimitiveType: primitiveType,
            IsPartial: isPartial,
            IsNested: isNested,
            DiagnosticLocation: classDecl.Identifier.GetLocation());
    }

    /// <summary>
    /// Maps a target CLR type symbol to the corresponding StrongOf base type metadata.
    /// </summary>
    /// <param name="type">Type symbol declared by the marker attribute.</param>
    /// <param name="baseTypeName">Resolved StrongOf base type name.</param>
    /// <param name="primitiveType">Fully-qualified primitive CLR type name.</param>
    /// <returns><see langword="true"/> when mapping is supported; otherwise <see langword="false"/>.</returns>
    private static bool TryMapTargetType(ITypeSymbol type, out string baseTypeName, out string primitiveType)
    {
        // SpecialType checks are the cheapest branch for CLR primitives.
        if (type.SpecialType == SpecialType.System_Boolean)
        {
            baseTypeName = "StrongBoolean";
            primitiveType = "global::System.Boolean";
            return true;
        }

        if (type.SpecialType == SpecialType.System_Char)
        {
            baseTypeName = "StrongChar";
            primitiveType = "global::System.Char";
            return true;
        }

        if (type.SpecialType == SpecialType.System_Decimal)
        {
            baseTypeName = "StrongDecimal";
            primitiveType = "global::System.Decimal";
            return true;
        }

        if (type.SpecialType == SpecialType.System_Double)
        {
            baseTypeName = "StrongDouble";
            primitiveType = "global::System.Double";
            return true;
        }

        if (type.SpecialType == SpecialType.System_Int32)
        {
            baseTypeName = "StrongInt32";
            primitiveType = "global::System.Int32";
            return true;
        }

        if (type.SpecialType == SpecialType.System_Int64)
        {
            baseTypeName = "StrongInt64";
            primitiveType = "global::System.Int64";
            return true;
        }

        if (type.SpecialType == SpecialType.System_String)
        {
            baseTypeName = "StrongString";
            primitiveType = "global::System.String";
            return true;
        }

        if (type is INamedTypeSymbol named)
        {
            // Guid/DateTime/DateTimeOffset/TimeSpan are not represented by SpecialType,
            // so we match the fully-qualified metadata name here.
            string metadataName = named.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (metadataName == "global::System.Guid")
            {
                baseTypeName = "StrongGuid";
                primitiveType = "global::System.Guid";
                return true;
            }

            if (metadataName == "global::System.DateTime")
            {
                baseTypeName = "StrongDateTime";
                primitiveType = "global::System.DateTime";
                return true;
            }

            if (metadataName == "global::System.DateTimeOffset")
            {
                baseTypeName = "StrongDateTimeOffset";
                primitiveType = "global::System.DateTimeOffset";
                return true;
            }

            if (metadataName == "global::System.TimeSpan")
            {
                baseTypeName = "StrongTimeSpan";
                primitiveType = "global::System.TimeSpan";
                return true;
            }
        }

        baseTypeName = string.Empty;
        primitiveType = string.Empty;
        return false;
    }

    /// <summary>
    /// Resolves a synthetic target that reports <c>STRONG004</c> when a class has multiple Strong markers.
    /// </summary>
    /// <param name="context">Current generator attribute context.</param>
    /// <param name="typeSymbol">Target class symbol.</param>
    /// <param name="classDecl">Target class declaration syntax.</param>
    /// <param name="diagnosticTarget">Resulting synthetic diagnostic target.</param>
    /// <returns><see langword="true"/> when processing should stop for the current pipeline.</returns>
    private static bool TryResolveMultipleMarkerDiagnosticTarget(
        GeneratorAttributeSyntaxContext context,
        INamedTypeSymbol typeSymbol,
        ClassDeclarationSyntax classDecl,
        out TargetType? diagnosticTarget)
    {
        diagnosticTarget = null;

        if (!HasMultipleStrongMarkers(typeSymbol, out string canonicalMarkerName))
        {
            return false;
        }

        string? currentMarkerName = context.Attributes.Length > 0
            ? context.Attributes[0].AttributeClass?.MetadataName
            : null;

        if (currentMarkerName is null || string.CompareOrdinal(currentMarkerName, canonicalMarkerName) != 0)
        {
            // Another marker pipeline reports STRONG004 for this type.
            return true;
        }

        string? namespaceName = typeSymbol.ContainingNamespace is { IsGlobalNamespace: false } ns
            ? ns.ToDisplayString()
            : null;

        diagnosticTarget = new TargetType(
            TypeName: typeSymbol.Name,
            Namespace: namespaceName,
            BaseTypeName: string.Empty,
            PrimitiveType: string.Empty,
            IsPartial: false,
            IsNested: false,
            DiagnosticLocation: classDecl.Identifier.GetLocation(),
            HasMultipleMarkers: true);

        return true;
    }

    /// <summary>
    /// Checks whether a class declaration uses more than one Strong marker attribute.
    /// </summary>
    /// <param name="typeSymbol">Class symbol to inspect.</param>
    /// <param name="canonicalMarkerName">
    /// Lexicographically smallest marker name used as deterministic diagnostic owner.
    /// </param>
    /// <returns><see langword="true"/> when multiple Strong markers were found.</returns>
    private static bool HasMultipleStrongMarkers(INamedTypeSymbol typeSymbol, out string canonicalMarkerName)
    {
        canonicalMarkerName = string.Empty;
        int markerCount = 0;

        foreach (AttributeData attribute in typeSymbol.GetAttributes())
        {
            if (!TryGetStrongMarkerMetadataName(attribute.AttributeClass, out string markerName))
            {
                continue;
            }

            markerCount++;
            // Pick one canonical reporter to avoid duplicate STRONG004 diagnostics from
            // concurrently running pipelines.
            if (canonicalMarkerName.Length == 0 || string.CompareOrdinal(markerName, canonicalMarkerName) < 0)
            {
                canonicalMarkerName = markerName;
            }
        }

        return markerCount > 1;
    }

    /// <summary>
    /// Tries to normalize any Strong marker attribute to its metadata name.
    /// </summary>
    /// <param name="attributeClass">Attribute symbol to inspect.</param>
    /// <param name="markerName">Resolved metadata name when successful.</param>
    /// <returns><see langword="true"/> when the attribute belongs to StrongOf markers.</returns>
    private static bool TryGetStrongMarkerMetadataName(INamedTypeSymbol? attributeClass, out string markerName)
    {
        markerName = string.Empty;

        if (attributeClass?.ContainingNamespace.ToDisplayString() != AttributeNamespace)
        {
            return false;
        }

        string metadataName = attributeClass.MetadataName;
        if (metadataName == GenericStrongAttributeName || metadataName == TypeofStrongAttributeName)
        {
            markerName = metadataName;
            return true;
        }

        foreach ((string attributeName, _, _) in s_kinds)
        {
            if (metadataName == attributeName)
            {
                markerName = metadataName;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Emits generated source for a validated target or reports diagnostics for invalid declarations.
    /// </summary>
    /// <param name="spc">Source production context used for diagnostics and output.</param>
    /// <param name="target">Resolved generation target.</param>
    private static void Emit(SourceProductionContext spc, TargetType target)
    {
        // Diagnostics are emitted before source generation so invalid declarations never
        // produce partial output that could hide the real user-facing error.
        if (target.HasMultipleMarkers)
        {
            spc.ReportDiagnostic(Diagnostic.Create(Diagnostics.MultipleStrongMarkersNotAllowed, target.DiagnosticLocation, target.TypeName));
            return;
        }

        if (target.UnsupportedTypeName is not null)
        {
            spc.ReportDiagnostic(Diagnostic.Create(Diagnostics.UnsupportedStrongTargetType, target.DiagnosticLocation, target.UnsupportedTypeName));
            return;
        }

        if (target.IsNested)
        {
            spc.ReportDiagnostic(Diagnostic.Create(Diagnostics.NestedNotSupported, target.DiagnosticLocation, target.TypeName));
            return;
        }

        if (!target.IsPartial)
        {
            spc.ReportDiagnostic(Diagnostic.Create(Diagnostics.MustBePartial, target.DiagnosticLocation, target.TypeName));
            return;
        }

        // The generated shape is very small and fixed; pre-sizing avoids intermediate
        // reallocations while still keeping the code simple.
        StringBuilder sb = new(capacity: 512);

        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine("// Generated by StrongOf.SourceGenerators - do not edit by hand.");
        sb.AppendLine("#nullable enable");
        sb.AppendLine("#pragma warning disable CS1591");
        sb.AppendLine();

        bool hasNamespace = !string.IsNullOrEmpty(target.Namespace);
        if (hasNamespace)
        {
            sb.Append("namespace ").Append(target.Namespace).AppendLine(";");
            sb.AppendLine();
        }

        sb.Append("partial class ").Append(target.TypeName)
          .Append('(').Append(target.PrimitiveType).Append(" value)").AppendLine()
          .Append("    : global::StrongOf.").Append(target.BaseTypeName)
                    .Append('<').Append(target.TypeName).Append(">(value), global::StrongOf.IStrongOf<")
                    .Append(target.PrimitiveType).Append(", ").Append(target.TypeName).Append(">")
          .AppendLine();

        sb.AppendLine("{");
        sb.Append("    /// <summary>Creates a new <see cref=\"").Append(target.TypeName).AppendLine("\" /> instance. Generated by StrongOf.SourceGenerators - AOT and trim safe.</summary>");
        sb.Append("    public static ").Append(target.TypeName)
          .Append(" Create(").Append(target.PrimitiveType).Append(" value) => new(value);")
          .AppendLine();
        sb.AppendLine("}");

        string hint = (hasNamespace ? target.Namespace + "." : string.Empty) + target.TypeName + GeneratedFileSuffix;
        spc.AddSource(hint, SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    /// <summary>
    /// Immutable intermediate representation of a class candidate discovered by the syntax provider.
    /// </summary>
    /// <remarks>
    /// This record carries both generation metadata and deferred diagnostic metadata so all
    /// source-output pipelines can share the same <see cref="Emit"/> implementation.
    /// </remarks>
    private readonly record struct TargetType(
        string TypeName,
        string? Namespace,
        string BaseTypeName,
        string PrimitiveType,
        bool IsPartial,
        bool IsNested,
        Location DiagnosticLocation,
        string? UnsupportedTypeName = null,
        bool HasMultipleMarkers = false);

    /// <summary>
    /// Centralized diagnostic descriptors emitted by the StrongOf source generator.
    /// </summary>
    private static class Diagnostics
    {
        public static readonly DiagnosticDescriptor MustBePartial = new(
            id: "STRONG001",
            title: "StrongOf marker requires partial class",
            messageFormat: "'{0}' must be declared 'partial' for the StrongOf source generator to extend it",
            category: "StrongOf",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor NestedNotSupported = new(
            id: "STRONG002",
            title: "StrongOf marker not supported on nested types",
            messageFormat: "'{0}' is a nested type; the StrongOf source generator only supports top-level (non-nested) classes",
            category: "StrongOf",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor UnsupportedStrongTargetType = new(
            id: "STRONG003",
            title: "Unsupported Strong target type",
            messageFormat: "Type '{0}' is not supported by [Strong<TTarget>] or [Strong(typeof(TTarget))]. Use one of: bool, char, DateTime, DateTimeOffset, decimal, double, Guid, int, long, string, TimeSpan.",
            category: "StrongOf",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor MultipleStrongMarkersNotAllowed = new(
            id: "STRONG004",
            title: "Multiple Strong markers are not allowed",
            messageFormat: "'{0}' has multiple Strong markers. Apply exactly one marker attribute per class declaration.",
            category: "StrongOf",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
