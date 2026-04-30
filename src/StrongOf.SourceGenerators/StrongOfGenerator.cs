// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Generic;
using System.Collections.Immutable;
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

    private static void Emit(SourceProductionContext spc, TargetType target)
    {
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
          .Append('<').Append(target.TypeName).Append(">(value)")
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
        Location DiagnosticLocation);

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
    }
}
