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

        IncrementalValuesProvider<TargetType?> genericTargets = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeNamespace + ".StrongAttribute`1",
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, ct) => ToGenericTarget(ctx, ct));

        IncrementalValuesProvider<TargetType> validGenericTargets = genericTargets
            .Where(static t => t is not null)
            .Select(static (t, _) => t!.Value);

        context.RegisterSourceOutput(validGenericTargets, static (spc, target) => Emit(spc, target));

        IncrementalValuesProvider<TargetType?> typeofTargets = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeNamespace + ".StrongAttribute",
                static (node, _) => node is ClassDeclarationSyntax,
                static (ctx, ct) => ToTypeofTarget(ctx, ct));

        IncrementalValuesProvider<TargetType> validTypeofTargets = typeofTargets
            .Where(static t => t is not null)
            .Select(static (t, _) => t!.Value);

        context.RegisterSourceOutput(validTypeofTargets, static (spc, target) => Emit(spc, target));
    }

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

        // The user must declare the type as partial.
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

    private static bool TryMapTargetType(ITypeSymbol type, out string baseTypeName, out string primitiveType)
    {
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

    private static void Emit(SourceProductionContext spc, TargetType target)
    {
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

    private readonly record struct TargetType(
        string TypeName,
        string? Namespace,
        string BaseTypeName,
        string PrimitiveType,
        bool IsPartial,
        bool IsNested,
        Location DiagnosticLocation,
        string? UnsupportedTypeName = null);

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
    }
}
