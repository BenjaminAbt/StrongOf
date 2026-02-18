// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="LanguageCode"/>.
/// </summary>
public class LanguageCodeTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var code = new LanguageCode("en-US");
        Assert.Equal("en-US", code.Value);
    }

    [Theory]
    [InlineData("en", true)]
    [InlineData("de", true)]
    [InlineData("en-US", true)]
    [InlineData("e", false)]
    [InlineData("en-USA", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var code = new LanguageCode(value);
        Assert.Equal(expected, code.IsValidFormat());
    }

    [Fact]
    public void ToLowerCase_ReturnsExpected()
    {
        var code = new LanguageCode("EN-US");
        Assert.Equal("en-us", code.ToLowerCase());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new LanguageCodeTypeConverter();
        var result = converter.ConvertFrom("en") as LanguageCode;

        Assert.NotNull(result);
        Assert.Equal("en", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new LanguageCodeTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
