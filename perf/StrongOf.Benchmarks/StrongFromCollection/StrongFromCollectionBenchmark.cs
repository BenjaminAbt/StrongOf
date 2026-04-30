// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongFromCollection;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Benchmarks the bulk-conversion factory overloads (<c>From(IEnumerable)</c>,
/// <c>FromArray</c>, <c>FromSpan</c>) so the array/span fast paths added in the
/// runtime-performance overhaul can be tracked over time.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongFromCollectionBenchmark
{
    private const int Count = 1_000;

    private Guid[] _guidArray = null!;
    private List<Guid> _guidList = null!;

    [GlobalSetup]
    public void Setup()
    {
        _guidArray = new Guid[Count];
        for (int i = 0; i < Count; i++)
        {
            _guidArray[i] = Guid.NewGuid();
        }
        _guidList = new List<Guid>(_guidArray);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FromArray")]
    public int From_IEnumerable_Array()
        => TestStrongGuid.From((IEnumerable<Guid>)_guidArray)!.Count;

    [Benchmark]
    [BenchmarkCategory("FromArray")]
    public int FromArray_Direct()
        => TestStrongGuid.FromArray(_guidArray)!.Length;

    [Benchmark]
    [BenchmarkCategory("FromArray")]
    public int FromSpan_Direct()
        => TestStrongGuid.FromSpan(_guidArray).Length;

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("FromList")]
    public int From_IEnumerable_List()
        => TestStrongGuid.From((IEnumerable<Guid>)_guidList)!.Count;

    private sealed class TestStrongGuid(Guid value) : StrongGuid<TestStrongGuid>(value);
}
