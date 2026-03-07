// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="ColorHex"/>.
/// </summary>
public class ColorHexTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        ColorHex color = new ColorHex("#ff00aa");
        Assert.Equal("#ff00aa", color.Value);
    }

    [Theory]
    [InlineData("#FFFFFF", true)]
    [InlineData("FFFFFF", true)]
    [InlineData("#FFFFFFFF", true)]
    [InlineData("#FFF", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        ColorHex color = new ColorHex(value);
        Assert.Equal(expected, color.IsValidFormat());
    }

    [Fact]
    public void Normalize_ReturnsUppercaseWithHash()
    {
        ColorHex color = new ColorHex("ff00aa");
        Assert.Equal("#FF00AA", color.Normalize());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<ColorHex> converter = new StrongStringTypeConverter<ColorHex>();
        ColorHex? result = converter.ConvertFrom("#FFFFFF") as ColorHex;

        Assert.NotNull(result);
        Assert.Equal("#FFFFFF", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<ColorHex> converter = new StrongStringTypeConverter<ColorHex>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
