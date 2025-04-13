using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongNew;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70)] // PGO enabled by default
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
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

public sealed class TestStrongInt32(int Value)
    : StrongInt32<TestStrongInt32>(Value);

public sealed class TestStrongInt64(long Value)
    : StrongInt64<TestStrongInt64>(Value);

public sealed class TestStrongString(string Value)
    : StrongString<TestStrongString>(Value);

public sealed class TestStrongGuid(Guid Value)
    : StrongGuid<TestStrongGuid>(Value);
