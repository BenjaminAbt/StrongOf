// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongParse;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongParseBenchmark
{
    private const string GuidString = "550e8400-e29b-41d4-a716-446655440000";
    private const string IntString = "42";
    private const string DecimalString = "99.99";
    private const string DateString = "2024-01-15T10:30:00Z";

    // IParsable<T>.Parse

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public TestStrongGuid Guid_Parse()
        => TestStrongGuid.Parse(GuidString, null);

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public TestStrongInt32 Int32_Parse()
        => TestStrongInt32.Parse(IntString, CultureInfo.InvariantCulture);

    [Benchmark]
    [BenchmarkCategory("Decimal")]
    public TestStrongDecimal Decimal_Parse()
        => TestStrongDecimal.Parse(DecimalString, CultureInfo.InvariantCulture);

    [Benchmark]
    [BenchmarkCategory("DateTime")]
    public TestStrongDateTime DateTime_Parse()
        => TestStrongDateTime.Parse(DateString, CultureInfo.InvariantCulture);

    // IParsable<T>.TryParse

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public bool Guid_TryParse()
        => TestStrongGuid.TryParse(GuidString, null, out _);

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public bool Int32_TryParse()
        => TestStrongInt32.TryParse(IntString, CultureInfo.InvariantCulture, out _);

    [Benchmark]
    [BenchmarkCategory("Decimal")]
    public bool Decimal_TryParse()
        => TestStrongDecimal.TryParse(DecimalString, CultureInfo.InvariantCulture, out _);

    // ISpanParsable<T>.Parse

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public TestStrongGuid Guid_Parse_Span()
        => TestStrongGuid.Parse(GuidString.AsSpan(), null);

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public TestStrongInt32 Int32_Parse_Span()
        => TestStrongInt32.Parse(IntString.AsSpan(), CultureInfo.InvariantCulture);
}

// Test Classes

public sealed class TestStrongGuid(Guid Value) : StrongGuid<TestStrongGuid>(Value);
public sealed class TestStrongInt32(int Value) : StrongInt32<TestStrongInt32>(Value);
public sealed class TestStrongDecimal(decimal Value) : StrongDecimal<TestStrongDecimal>(Value);
public sealed class TestStrongDateTime(DateTime Value) : StrongDateTime<TestStrongDateTime>(Value);
