// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace StrongOf.Benchmarks.StrongEquality;

#pragma warning disable CA1822 // Mark members as static

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0, baseline: true)]
[CategoriesColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class StrongEqualityBenchmark
{
    private static readonly TestStrongGuid s_guid1 = new(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    private static readonly TestStrongGuid s_guid2 = new(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    private static readonly TestStrongGuid s_guid3 = new(Guid.Parse("660e8400-e29b-41d4-a716-446655440000"));

    private static readonly TestStrongString s_str1 = new("Batman");
    private static readonly TestStrongString s_str2 = new("Batman");
    private static readonly TestStrongString s_str3 = new("Robin");

    private static readonly TestStrongInt32 s_int1 = new(42);
    private static readonly TestStrongInt32 s_int2 = new(42);

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public bool Guid_Equals_Typed()
        => s_guid1.Equals(s_guid2);

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public bool Guid_Equals_Object()
        => s_guid1.Equals((object)s_guid2);

    [Benchmark]
    [BenchmarkCategory("Guid")]
    public bool Guid_NotEqual()
        => s_guid1 != s_guid3;

    [Benchmark]
    [BenchmarkCategory("String")]
    public bool String_Equals_Typed()
        => s_str1.Equals(s_str2);

    [Benchmark]
    [BenchmarkCategory("String")]
    public bool String_NotEqual()
        => s_str1 != s_str3;

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public bool Int32_Equals_Typed()
        => s_int1.Equals(s_int2);

    [Benchmark]
    [BenchmarkCategory("Int32")]
    public int Int32_CompareTo()
        => s_int1.CompareTo(s_int2);
}

// Test Classes

public sealed class TestStrongGuid(Guid Value) : StrongGuid<TestStrongGuid>(Value);
public sealed class TestStrongString(string Value) : StrongString<TestStrongString>(Value);
public sealed class TestStrongInt32(int Value) : StrongInt32<TestStrongInt32>(Value);
