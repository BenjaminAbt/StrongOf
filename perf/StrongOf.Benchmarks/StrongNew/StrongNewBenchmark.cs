// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongNew;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongNewBenchmark
{
    private static readonly Guid s_guid = Guid.NewGuid();

    [Benchmark]
    [BenchmarkCategory("StrongInt32")]
    public TestStrongInt32 Int32_New()
        => new(31);

    [Benchmark]
    [BenchmarkCategory("StrongInt32")]
    public TestStrongInt32 Int32_From()
        => TestStrongInt32.From(31);

    [Benchmark]
    [BenchmarkCategory("StrongInt64")]
    public TestStrongInt64 Int64_New()
        => new(31);

    [Benchmark]
    [BenchmarkCategory("StrongInt64")]
    public TestStrongInt64 Int64_From()
        => TestStrongInt64.From(31);

    [Benchmark]
    [BenchmarkCategory("StrongString")]
    public TestStrongString String_New()
        => new("Batman");

    [Benchmark]
    [BenchmarkCategory("StrongString")]
    public TestStrongString String_From()
        => TestStrongString.From("Batman");

    [Benchmark]
    [BenchmarkCategory("StrongGuid")]
    public TestStrongGuid Guid_New()
        => new(s_guid);

    [Benchmark]
    [BenchmarkCategory("StrongGuid")]
    public TestStrongGuid Guid_From()
        => TestStrongGuid.From(s_guid);
}

// Test Classes

[StrongInt32]
public sealed partial class TestStrongInt32;

[StrongInt64]
public sealed partial class TestStrongInt64;

[StrongString]
public sealed partial class TestStrongString;

[StrongGuid]
public sealed partial class TestStrongGuid;
