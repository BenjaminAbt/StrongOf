// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StrongOf.SourceGeneration;
using Xunit;

namespace StrongOf.UnitTests.Generated;

public sealed class StrongOfSourceGeneratorDiagnosticsTests
{
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
        string generatorAssemblyPath = ResolveGeneratorAssemblyPath();
        Assembly generatorAssembly = Assembly.LoadFrom(generatorAssemblyPath);

        Type? generatorType = generatorAssembly.GetType("StrongOf.SourceGenerators.StrongOfGenerator", throwOnError: false, ignoreCase: false);
        Assert.NotNull(generatorType);

        object? instance = Activator.CreateInstance(generatorType!);
        Assert.NotNull(instance);

        Assert.IsAssignableFrom<IIncrementalGenerator>(instance);
        IIncrementalGenerator incrementalGenerator = (IIncrementalGenerator)instance;

        return incrementalGenerator.AsSourceGenerator();
    }

    private static string ResolveGeneratorAssemblyPath()
    {
        string repositoryRoot = ResolveRepositoryRoot();

        string debugPath = Path.Combine(
            repositoryRoot,
            "src",
            "StrongOf.SourceGenerators",
            "bin",
            "Debug",
            "netstandard2.0",
            "StrongOf.SourceGenerators.dll");

        if (File.Exists(debugPath))
        {
            return debugPath;
        }

        string releasePath = Path.Combine(
            repositoryRoot,
            "src",
            "StrongOf.SourceGenerators",
            "bin",
            "Release",
            "netstandard2.0",
            "StrongOf.SourceGenerators.dll");

        Assert.True(File.Exists(releasePath), "StrongOf.SourceGenerators.dll was not found in Debug or Release output.");
        return releasePath;
    }

    private static string ResolveRepositoryRoot()
    {
        DirectoryInfo? current = new(AppContext.BaseDirectory);

        while (current is not null)
        {
            string sourceGeneratorProjectDirectory = Path.Combine(current.FullName, "src", "StrongOf.SourceGenerators");
            if (Directory.Exists(sourceGeneratorProjectDirectory))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not resolve repository root from test base directory.");
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
}
