// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StrongOf.SourceGeneration;
using Xunit;

namespace StrongOf.UnitTests.Generated;

public sealed class StrongOfSourceGeneratorDiagnosticsTests
{
    [Theory]
    [InlineData("StrongGuid")]
    [InlineData("Strong<System.Guid>")]
    [InlineData("Strong(typeof(System.Guid))")]
    public void GenericClass_ReportsStrong005(string marker)
    {
        string source = $$"""
                          using StrongOf.SourceGeneration;
                          namespace Demo;
                          [{{marker}}]
                          public partial class UserId<T>;
                          """;

        ImmutableArray<Diagnostic> diagnostics = RunGenerator(source);

        Diagnostic diagnostic = Assert.Single(diagnostics.Where(static d => string.Equals(d.Id, "STRONG005", StringComparison.Ordinal)));
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void MultiplePrimitiveMarkers_ReportStrong004ExactlyOnce()
    {
        const string source = """
                              using StrongOf.SourceGeneration;

                              namespace Demo;

                              [StrongGuid]
                              [StrongString]
                              public partial class UserId;
                              """;

        ImmutableArray<Diagnostic> diagnostics = RunGenerator(source);

        Diagnostic diagnostic = Assert.Single(diagnostics.Where(static d => string.Equals(d.Id, "STRONG004", StringComparison.Ordinal)));
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void GenericAndTypeofMarkers_ReportStrong004ExactlyOnce()
    {
        const string source = """
                              using System;
                              using StrongOf.SourceGeneration;

                              namespace Demo;

                              [Strong<Guid>]
                              [Strong(typeof(Guid))]
                              public partial class UserId;
                              """;

        ImmutableArray<Diagnostic> diagnostics = RunGenerator(source);

        Diagnostic diagnostic = Assert.Single(diagnostics.Where(static d => string.Equals(d.Id, "STRONG004", StringComparison.Ordinal)));
        Assert.Equal(DiagnosticSeverity.Error, diagnostic.Severity);
    }

    [Fact]
    public void SingleMarker_DoesNotReportStrong004()
    {
        const string source = """
                              using StrongOf.SourceGeneration;

                              namespace Demo;

                              [StrongGuid]
                              public partial class UserId;
                              """;

        ImmutableArray<Diagnostic> diagnostics = RunGenerator(source);

        Assert.DoesNotContain(diagnostics, static d => string.Equals(d.Id, "STRONG004", StringComparison.Ordinal));
        Assert.DoesNotContain(diagnostics, static d => d.Severity == DiagnosticSeverity.Error);
    }

    [Theory]
    [InlineData("bool")]
    [InlineData("char")]
    [InlineData("decimal")]
    [InlineData("double")]
    [InlineData("int")]
    [InlineData("long")]
    [InlineData("string")]
    [InlineData("System.Guid")]
    [InlineData("System.DateTime")]
    [InlineData("System.DateTimeOffset")]
    [InlineData("System.TimeSpan")]
    public void BothMarkerFormsGenerateConstructibleTypes(string primitive)
    {
        string source = $$"""
            using StrongOf.SourceGeneration;
            [Strong<{{primitive}}>]
            public partial class GenericValue;
            [Strong(typeof({{primitive}}))]
            public partial class TypeofValue;
            public static class Consumer
            {
                public static (GenericValue, TypeofValue) Wrap({{primitive}} value) =>
                    (new GenericValue(value), new TypeofValue(value));
            }
            """;
        Assert.DoesNotContain(RunGenerator(source), static d => d.Severity == DiagnosticSeverity.Error);
    }

    private static ImmutableArray<Diagnostic> RunGenerator(string source)
    {
        CSharpCompilation compilation = CreateCompilation(source);
        ISourceGenerator sourceGenerator = CreateSourceGenerator();
        CSharpParseOptions parseOptions = CreateParseOptions();
        CSharpGeneratorDriver generatorDriver = CSharpGeneratorDriver.Create(
            generators: [sourceGenerator],
            parseOptions: parseOptions);

        GeneratorDriver _ = generatorDriver.RunGeneratorsAndUpdateCompilation(
            compilation,
            out Compilation outputCompilation,
            out ImmutableArray<Diagnostic> generatorDiagnostics,
            cancellationToken: TestContext.Current.CancellationToken);

        return generatorDiagnostics.AddRange(outputCompilation.GetDiagnostics(TestContext.Current.CancellationToken));
    }

    private static ISourceGenerator CreateSourceGenerator()
    {
        return new global::StrongOf.SourceGenerators.StrongOfGenerator().AsSourceGenerator();
    }

    private static CSharpCompilation CreateCompilation(string source)
    {
        CSharpParseOptions parseOptions = CreateParseOptions();
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
            source,
            parseOptions,
            cancellationToken: TestContext.Current.CancellationToken);
        CSharpCompilationOptions compilationOptions = new(OutputKind.DynamicallyLinkedLibrary);

        return CSharpCompilation.Create(
            assemblyName: "StrongOf.SourceGenerators.DiagnosticTests",
            syntaxTrees: [syntaxTree],
            references: CreateMetadataReferences(),
            options: compilationOptions);
    }

    private static CSharpParseOptions CreateParseOptions()
        => CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);

    private static ImmutableArray<MetadataReference> CreateMetadataReferences()
    {
        object? trustedPlatformAssemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
        Assert.NotNull(trustedPlatformAssemblies);

        string platformAssemblyPaths = (string)trustedPlatformAssemblies;
        string[] splitPaths = platformAssemblyPaths.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

        HashSet<string> referencePaths = new(StringComparer.OrdinalIgnoreCase);
        foreach (string path in splitPaths)
        {
            referencePaths.Add(path);
        }

        referencePaths.Add(typeof(IStrongOf).Assembly.Location);
        referencePaths.Add(typeof(StrongAttribute<>).Assembly.Location);

        ImmutableArray<MetadataReference>.Builder references = ImmutableArray.CreateBuilder<MetadataReference>(referencePaths.Count);
        foreach (string referencePath in referencePaths)
        {
            references.Add(MetadataReference.CreateFromFile(referencePath));
        }

        return references.ToImmutable();
    }

    [Theory]
    [InlineData("[StrongGuid] public class Id;", "STRONG001")]
    [InlineData("public class Parent { [StrongGuid] public partial class Id; }", "STRONG002")]
    [InlineData("[Strong<short>] public partial class Id;", "STRONG003")]
    [InlineData("[Strong<int[]>] public partial class Id;", "STRONG003")]
    [InlineData("[Strong(typeof(short))] public partial class Id;", "STRONG003")]
    public void InvalidDeclarationReportsSpecificDiagnostic(string declaration, string expectedId)
    {
        ImmutableArray<Diagnostic> diagnostics = RunGenerator("using StrongOf.SourceGeneration; " + declaration);
        Assert.Contains(diagnostics, d => string.Equals(d.Id, expectedId, StringComparison.Ordinal));
    }

    [Fact]
    public void UnrelatedAttributesDoNotCountAsStrongMarkers()
    {
        const string source = """
            using StrongOf.SourceGeneration;
            namespace StrongOf.SourceGeneration { public class ExtraAttribute : System.Attribute; }
            [System.Serializable, Extra, StrongGuid] public partial class Id;
            """;
        Assert.DoesNotContain(RunGenerator(source), static d => d.Severity == DiagnosticSeverity.Error);
    }

    [Theory]
    [InlineData("[Strong] public partial class Id;")]
    [InlineData("[Strong(null)] public partial class Id;")]
    [InlineData("[Strong(\"bad\")] public partial class Id;")]
    public void IncompleteTypeofAttributeDoesNotCrashGenerator(string declaration)
    {
        ImmutableArray<Diagnostic> diagnostics = RunGenerator("using StrongOf.SourceGeneration; " + declaration);
        Assert.DoesNotContain(diagnostics, static d => string.Equals(d.Id, "CS8785", StringComparison.Ordinal));
        Assert.DoesNotContain(diagnostics, static d => d.Id.StartsWith("STRONG", StringComparison.Ordinal));
    }

    [Fact]
    public void RuntimeMarkerExposesPrimitiveType()
    {
        Assert.Equal(typeof(int), new StrongAttribute(typeof(int)).TargetType);
    }
}
