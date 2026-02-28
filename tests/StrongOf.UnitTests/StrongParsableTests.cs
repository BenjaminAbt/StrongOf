// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

/// <summary>
/// Tests for IParsable, ISpanParsable, and IFormattable implementations across all strong types.
/// </summary>
public class StrongParsableTests
{
    // Test types for each strong type
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value);
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value);
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value);
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value);
    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value);
    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value);
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value);
    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value);

    // ==================== StrongString ====================

    [Fact]
    public void StrongString_Parse_WithValidString_ReturnsInstance()
    {
        TestStringOf result = TestStringOf.Parse("hello", null);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void StrongString_Parse_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TestStringOf.Parse(null!, null));
    }

    [Fact]
    public void StrongString_TryParse_WithValidString_ReturnsTrue()
    {
        bool success = TestStringOf.TryParse("hello", null, out TestStringOf? result);
        Assert.True(success);
        Assert.Equal("hello", result!.Value);
    }

    [Fact]
    public void StrongString_TryParse_WithNull_ReturnsFalse()
    {
        bool success = TestStringOf.TryParse(null, null, out TestStringOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongString_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestStringOf result = TestStringOf.Parse("hello".AsSpan(), null);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void StrongString_SpanTryParse_WithValidSpan_ReturnsTrue()
    {
        bool success = TestStringOf.TryParse("hello".AsSpan(), null, out TestStringOf? result);
        Assert.True(success);
        Assert.Equal("hello", result!.Value);
    }

    [Fact]
    public void StrongString_SpanTryParse_WithEmptySpan_ReturnsTrue()
    {
        bool success = TestStringOf.TryParse(ReadOnlySpan<char>.Empty, null, out TestStringOf? result);
        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal("", result!.Value);
    }

    [Fact]
    public void StrongString_AsSpan_ReturnsSpan()
    {
        TestStringOf strong = new("hello");
        ReadOnlySpan<char> span = strong.AsSpan();
        Assert.True(span.SequenceEqual("hello".AsSpan()));
    }

    // ==================== StrongGuid ====================

    [Fact]
    public void StrongGuid_Parse_WithValidGuid_ReturnsInstance()
    {
        Guid expected = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
        TestGuidOf result = TestGuidOf.Parse("550e8400-e29b-41d4-a716-446655440000", null);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void StrongGuid_Parse_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TestGuidOf.Parse(null!, null));
    }

    [Fact]
    public void StrongGuid_Parse_WithInvalid_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => TestGuidOf.Parse("not-a-guid", null));
    }

    [Fact]
    public void StrongGuid_TryParse_WithValidGuid_ReturnsTrue()
    {
        bool success = TestGuidOf.TryParse("550e8400-e29b-41d4-a716-446655440000", null, out TestGuidOf? result);
        Assert.True(success);
        Assert.Equal(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), result!.Value);
    }

    [Fact]
    public void StrongGuid_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestGuidOf.TryParse("not-a-guid", null, out TestGuidOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongGuid_SpanParse_WithValidSpan_ReturnsInstance()
    {
        Guid expected = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
        TestGuidOf result = TestGuidOf.Parse("550e8400-e29b-41d4-a716-446655440000".AsSpan(), null);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void StrongGuid_SpanTryParse_WithValidSpan_ReturnsTrue()
    {
        bool success = TestGuidOf.TryParse("550e8400-e29b-41d4-a716-446655440000".AsSpan(), null, out TestGuidOf? result);
        Assert.True(success);
        Assert.NotNull(result);
    }

    [Fact]
    public void StrongGuid_IFormattable_ToString_WithFormat()
    {
        Guid guid = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
        TestGuidOf strong = new(guid);
        string result = strong.ToString("N", null);
        Assert.Equal(guid.ToString("N", null), result);
    }

    // ==================== StrongInt32 ====================

    [Fact]
    public void StrongInt32_Parse_WithValidInt_ReturnsInstance()
    {
        TestInt32Of result = TestInt32Of.Parse("42", CultureInfo.InvariantCulture);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void StrongInt32_Parse_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TestInt32Of.Parse(null!, null));
    }

    [Fact]
    public void StrongInt32_TryParse_WithValidInt_ReturnsTrue()
    {
        bool success = TestInt32Of.TryParse("42", CultureInfo.InvariantCulture, out TestInt32Of? result);
        Assert.True(success);
        Assert.Equal(42, result!.Value);
    }

    [Fact]
    public void StrongInt32_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestInt32Of.TryParse("not-a-number", CultureInfo.InvariantCulture, out TestInt32Of? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongInt32_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestInt32Of result = TestInt32Of.Parse("42".AsSpan(), CultureInfo.InvariantCulture);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void StrongInt32_SpanTryParse_WithValidSpan_ReturnsTrue()
    {
        bool success = TestInt32Of.TryParse("42".AsSpan(), CultureInfo.InvariantCulture, out TestInt32Of? result);
        Assert.True(success);
        Assert.Equal(42, result!.Value);
    }

    [Fact]
    public void StrongInt32_IFormattable_ToString_WithFormat()
    {
        TestInt32Of strong = new(12345);
        string result = strong.ToString("N0", CultureInfo.InvariantCulture);
        Assert.Equal("12,345", result);
    }

    // ==================== StrongInt64 ====================

    [Fact]
    public void StrongInt64_Parse_WithValidLong_ReturnsInstance()
    {
        TestInt64Of result = TestInt64Of.Parse("9999999999", CultureInfo.InvariantCulture);
        Assert.Equal(9999999999L, result.Value);
    }

    [Fact]
    public void StrongInt64_TryParse_WithValidLong_ReturnsTrue()
    {
        bool success = TestInt64Of.TryParse("9999999999", CultureInfo.InvariantCulture, out TestInt64Of? result);
        Assert.True(success);
        Assert.Equal(9999999999L, result!.Value);
    }

    [Fact]
    public void StrongInt64_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestInt64Of.TryParse("not-a-number", CultureInfo.InvariantCulture, out TestInt64Of? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongInt64_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestInt64Of result = TestInt64Of.Parse("9999999999".AsSpan(), CultureInfo.InvariantCulture);
        Assert.Equal(9999999999L, result.Value);
    }

    [Fact]
    public void StrongInt64_IFormattable_ToString_WithFormat()
    {
        TestInt64Of strong = new(1234567890);
        string result = strong.ToString("N0", CultureInfo.InvariantCulture);
        Assert.Equal("1,234,567,890", result);
    }

    // ==================== StrongDecimal ====================

    [Fact]
    public void StrongDecimal_Parse_WithValidDecimal_ReturnsInstance()
    {
        TestDecimalOf result = TestDecimalOf.Parse("99.99", CultureInfo.InvariantCulture);
        Assert.Equal(99.99m, result.Value);
    }

    [Fact]
    public void StrongDecimal_TryParse_WithValidDecimal_ReturnsTrue()
    {
        bool success = TestDecimalOf.TryParse("99.99", CultureInfo.InvariantCulture, out TestDecimalOf? result);
        Assert.True(success);
        Assert.Equal(99.99m, result!.Value);
    }

    [Fact]
    public void StrongDecimal_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestDecimalOf.TryParse("not-a-number", CultureInfo.InvariantCulture, out TestDecimalOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongDecimal_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestDecimalOf result = TestDecimalOf.Parse("99.99".AsSpan(), CultureInfo.InvariantCulture);
        Assert.Equal(99.99m, result.Value);
    }

    [Fact]
    public void StrongDecimal_IFormattable_ToString_WithFormat()
    {
        TestDecimalOf strong = new(1234.5m);
        string result = strong.ToString("C2", CultureInfo.GetCultureInfo("en-US"));
        Assert.Contains("1,234.50", result, StringComparison.Ordinal);
    }

    // ==================== StrongChar ====================

    [Fact]
    public void StrongChar_Parse_WithValidChar_ReturnsInstance()
    {
        TestCharOf result = TestCharOf.Parse("A", null);
        Assert.Equal('A', result.Value);
    }

    [Fact]
    public void StrongChar_Parse_WithMultipleChars_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => TestCharOf.Parse("AB", null));
    }

    [Fact]
    public void StrongChar_TryParse_WithValidChar_ReturnsTrue()
    {
        bool success = TestCharOf.TryParse("A", null, out TestCharOf? result);
        Assert.True(success);
        Assert.Equal('A', result!.Value);
    }

    [Fact]
    public void StrongChar_TryParse_WithMultipleChars_ReturnsFalse()
    {
        bool success = TestCharOf.TryParse("AB", null, out TestCharOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongChar_SpanParse_WithSingleChar_ReturnsInstance()
    {
        TestCharOf result = TestCharOf.Parse("A".AsSpan(), null);
        Assert.Equal('A', result.Value);
    }

    [Fact]
    public void StrongChar_SpanParse_WithMultipleChars_ThrowsFormatException()
    {
        Assert.Throws<FormatException>(() => TestCharOf.Parse("AB".AsSpan(), null));
    }

    [Fact]
    public void StrongChar_SpanTryParse_WithSingleChar_ReturnsTrue()
    {
        bool success = TestCharOf.TryParse("A".AsSpan(), null, out TestCharOf? result);
        Assert.True(success);
        Assert.Equal('A', result!.Value);
    }

    [Fact]
    public void StrongChar_SpanTryParse_WithMultipleChars_ReturnsFalse()
    {
        bool success = TestCharOf.TryParse("AB".AsSpan(), null, out TestCharOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    // ==================== StrongDateTime ====================

    [Fact]
    public void StrongDateTime_Parse_WithValidDate_ReturnsInstance()
    {
        TestDateTimeOf result = TestDateTimeOf.Parse("2024-01-15", CultureInfo.InvariantCulture);
        Assert.Equal(new DateTime(2024, 1, 15), result.Value);
    }

    [Fact]
    public void StrongDateTime_TryParse_WithValidDate_ReturnsTrue()
    {
        bool success = TestDateTimeOf.TryParse("2024-01-15", CultureInfo.InvariantCulture, out TestDateTimeOf? result);
        Assert.True(success);
        Assert.Equal(new DateTime(2024, 1, 15), result!.Value);
    }

    [Fact]
    public void StrongDateTime_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestDateTimeOf.TryParse("not-a-date", CultureInfo.InvariantCulture, out TestDateTimeOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongDateTime_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestDateTimeOf result = TestDateTimeOf.Parse("2024-01-15".AsSpan(), CultureInfo.InvariantCulture);
        Assert.Equal(new DateTime(2024, 1, 15), result.Value);
    }

    [Fact]
    public void StrongDateTime_IFormattable_ToString_WithFormat()
    {
        TestDateTimeOf strong = new(new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc));
        string result = strong.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        Assert.Equal("2024-01-15", result);
    }

    // ==================== StrongDateTimeOffset ====================

    [Fact]
    public void StrongDateTimeOffset_Parse_WithValidDate_ReturnsInstance()
    {
        TestDateTimeOffsetOf result = TestDateTimeOffsetOf.Parse("2024-01-15T10:30:00+00:00", CultureInfo.InvariantCulture);
        Assert.Equal(new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero), result.Value);
    }

    [Fact]
    public void StrongDateTimeOffset_TryParse_WithValidDate_ReturnsTrue()
    {
        bool success = TestDateTimeOffsetOf.TryParse("2024-01-15T10:30:00+00:00", CultureInfo.InvariantCulture, out TestDateTimeOffsetOf? result);
        Assert.True(success);
        Assert.NotNull(result);
    }

    [Fact]
    public void StrongDateTimeOffset_TryParse_WithInvalid_ReturnsFalse()
    {
        bool success = TestDateTimeOffsetOf.TryParse("not-a-date", CultureInfo.InvariantCulture, out TestDateTimeOffsetOf? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void StrongDateTimeOffset_SpanParse_WithValidSpan_ReturnsInstance()
    {
        TestDateTimeOffsetOf result = TestDateTimeOffsetOf.Parse("2024-01-15T10:30:00+00:00".AsSpan(), CultureInfo.InvariantCulture);
        Assert.Equal(new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero), result.Value);
    }

    [Fact]
    public void StrongDateTimeOffset_IFormattable_ToString_WithFormat()
    {
        TestDateTimeOffsetOf strong = new(new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero));
        string result = strong.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        Assert.Equal("2024-01-15", result);
    }

    // ==================== IParsable constraint verification ====================

    [Fact]
    public void IParsable_CanBeUsedAsGenericConstraint()
    {
        TestGuidOf result = ParseHelper<TestGuidOf>("550e8400-e29b-41d4-a716-446655440000");
        Assert.Equal(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), result.Value);
    }

    [Fact]
    public void ISpanParsable_CanBeUsedAsGenericConstraint()
    {
        TestInt32Of result = SpanParseHelper<TestInt32Of>("42");
        Assert.Equal(42, result.Value);
    }

    private static T ParseHelper<T>(string value) where T : IParsable<T>
        => T.Parse(value, null);

    private static T SpanParseHelper<T>(string value) where T : ISpanParsable<T>
        => T.Parse(value.AsSpan(), null);
}
