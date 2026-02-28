// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using StrongOf.EntityFrameworkCore;
using Xunit;

namespace StrongOf.EntityFrameworkCore.UnitTests;

/// <summary>
/// Tests for <see cref="StrongOfValueConverter{TStrong,TTarget}"/>
/// verifying round-trip conversion between strong types and their underlying primitives.
/// </summary>
public class StrongOfValueConverterTests
{
    // Test types
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value);
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value);
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value);
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value);
    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value);
    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value);
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value);
    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value);

    // ==================== Guid ====================

    [Fact]
    public void GuidConverter_ConvertToProvider_ReturnsGuid()
    {
        StrongOfValueConverter<TestGuidOf, Guid> converter = new();
        Guid guid = Guid.NewGuid();
        TestGuidOf strong = new(guid);
        object? result = converter.ConvertToProvider(strong);
        Assert.Equal(guid, result);
    }

    [Fact]
    public void GuidConverter_ConvertFromProvider_ReturnsStrongType()
    {
        StrongOfValueConverter<TestGuidOf, Guid> converter = new();
        Guid guid = Guid.NewGuid();
        object? result = converter.ConvertFromProvider(guid);
        Assert.NotNull(result);
        Assert.IsType<TestGuidOf>(result);
        Assert.Equal(guid, ((TestGuidOf)result!).Value);
    }

    [Fact]
    public void GuidConverter_RoundTrip()
    {
        StrongOfValueConverter<TestGuidOf, Guid> converter = new();
        Guid guid = Guid.NewGuid();
        TestGuidOf original = new(guid);
        object? providerValue = converter.ConvertToProvider(original);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.Equal(original, roundTripped);
    }

    // ==================== String ====================

    [Fact]
    public void StringConverter_ConvertToProvider_ReturnsString()
    {
        StrongOfValueConverter<TestStringOf, string> converter = new();
        TestStringOf strong = new("hello");
        object? result = converter.ConvertToProvider(strong);
        Assert.Equal("hello", result);
    }

    [Fact]
    public void StringConverter_ConvertFromProvider_ReturnsStrongType()
    {
        StrongOfValueConverter<TestStringOf, string> converter = new();
        object? result = converter.ConvertFromProvider("hello");
        Assert.NotNull(result);
        Assert.IsType<TestStringOf>(result);
        Assert.Equal("hello", ((TestStringOf)result!).Value);
    }

    // ==================== Int32 ====================

    [Fact]
    public void Int32Converter_RoundTrip()
    {
        StrongOfValueConverter<TestInt32Of, int> converter = new();
        TestInt32Of original = new(42);
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal(42, providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestInt32Of>(roundTripped);
        Assert.Equal(42, ((TestInt32Of)roundTripped!).Value);
    }

    // ==================== Int64 ====================

    [Fact]
    public void Int64Converter_RoundTrip()
    {
        StrongOfValueConverter<TestInt64Of, long> converter = new();
        TestInt64Of original = new(9999999999L);
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal(9999999999L, providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestInt64Of>(roundTripped);
        Assert.Equal(9999999999L, ((TestInt64Of)roundTripped!).Value);
    }

    // ==================== Decimal ====================

    [Fact]
    public void DecimalConverter_RoundTrip()
    {
        StrongOfValueConverter<TestDecimalOf, decimal> converter = new();
        TestDecimalOf original = new(99.99m);
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal(99.99m, providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestDecimalOf>(roundTripped);
        Assert.Equal(99.99m, ((TestDecimalOf)roundTripped!).Value);
    }

    // ==================== Char ====================

    [Fact]
    public void CharConverter_RoundTrip()
    {
        StrongOfValueConverter<TestCharOf, char> converter = new();
        TestCharOf original = new('A');
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal('A', providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestCharOf>(roundTripped);
        Assert.Equal('A', ((TestCharOf)roundTripped!).Value);
    }

    // ==================== DateTime ====================

    [Fact]
    public void DateTimeConverter_RoundTrip()
    {
        StrongOfValueConverter<TestDateTimeOf, DateTime> converter = new();
        DateTime dt = new(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        TestDateTimeOf original = new(dt);
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal(dt, providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestDateTimeOf>(roundTripped);
        Assert.Equal(dt, ((TestDateTimeOf)roundTripped!).Value);
    }

    // ==================== DateTimeOffset ====================

    [Fact]
    public void DateTimeOffsetConverter_RoundTrip()
    {
        StrongOfValueConverter<TestDateTimeOffsetOf, DateTimeOffset> converter = new();
        DateTimeOffset dto = new(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);
        TestDateTimeOffsetOf original = new(dto);
        object? providerValue = converter.ConvertToProvider(original);
        Assert.Equal(dto, providerValue);
        object? roundTripped = converter.ConvertFromProvider(providerValue!);
        Assert.IsType<TestDateTimeOffsetOf>(roundTripped);
        Assert.Equal(dto, ((TestDateTimeOffsetOf)roundTripped!).Value);
    }
}
