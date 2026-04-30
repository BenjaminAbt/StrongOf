// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongDictionary;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Measures Dictionary&lt;TStrong, T&gt; lookup hot paths. Validates that the typed
/// Equals(StrongOf) overload + EqualityComparer&lt;TTarget&gt;.Default devirtualisation
/// avoid boxing and unnecessary virtual calls.
/// </summary>
[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongDictionaryBenchmark
{
    private const int Count = 1_000;

    private readonly Dictionary<TestStrongGuid, int> _guidMap = new(Count);
    private readonly Dictionary<TestStrongInt32, int> _intMap = new(Count);
    private readonly Dictionary<TestStrongString, int> _strMap = new(Count);

    private readonly TestStrongGuid[] _guidKeys = new TestStrongGuid[Count];
    private readonly TestStrongInt32[] _intKeys = new TestStrongInt32[Count];
    private readonly TestStrongString[] _strKeys = new TestStrongString[Count];

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < Count; i++)
        {
            TestStrongGuid g = new(Guid.NewGuid());
            TestStrongInt32 n = new(i);
            TestStrongString s = new("k_" + i);

            _guidKeys[i] = g;
            _intKeys[i] = n;
            _strKeys[i] = s;

            _guidMap[g] = i;
            _intMap[n] = i;
            _strMap[s] = i;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public int Guid_Lookup()
    {
        int sum = 0;
        for (int i = 0; i < Count; i++)
        {
            sum += _guidMap[_guidKeys[i]];
        }
        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public int Int32_Lookup()
    {
        int sum = 0;
        for (int i = 0; i < Count; i++)
        {
            sum += _intMap[_intKeys[i]];
        }
        return sum;
    }

    [Benchmark]
    [BenchmarkCategory("String")]
    public int String_Lookup()
    {
        int sum = 0;
        for (int i = 0; i < Count; i++)
        {
            sum += _strMap[_strKeys[i]];
        }
        return sum;
    }

    private sealed class TestStrongGuid(Guid value) : StrongGuid<TestStrongGuid>(value);
    private sealed class TestStrongInt32(int value) : StrongInt32<TestStrongInt32>(value);
    private sealed class TestStrongString(string value) : StrongString<TestStrongString>(value);
}
