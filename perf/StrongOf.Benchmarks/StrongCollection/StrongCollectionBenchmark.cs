// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongCollection;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongCollectionBenchmark
{
    private readonly Guid[] _guids = Enumerable.Range(0, 100).Select(_ => Guid.NewGuid()).ToArray();
    private readonly int[] _ints = Enumerable.Range(0, 100).ToArray();
    private readonly string[] _strings = Enumerable.Range(0, 100).Select(i => $"value_{i}").ToArray();

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public List<TestStrongGuid>? Guid_From_Array()
        => TestStrongGuid.From(_guids);

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public List<TestStrongInt32>? Int32_From_Array()
        => TestStrongInt32.From(_ints);

    [Benchmark]
    [BenchmarkCategory("String")]
    public List<TestStrongString>? String_From_Array()
        => TestStrongString.From(_strings);
}

// Test Classes

public sealed class TestStrongGuid(Guid Value) : StrongGuid<TestStrongGuid>(Value);
public sealed class TestStrongInt32(int Value) : StrongInt32<TestStrongInt32>(Value);
public sealed class TestStrongString(string Value) : StrongString<TestStrongString>(Value);
