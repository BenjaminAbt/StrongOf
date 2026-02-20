// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Postal.UnitTests;

/// <summary>
/// Tests for <see cref="CountryName"/>.
/// </summary>
public class CountryNameTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var name = new CountryName("Germany");
        Assert.Equal("Germany", name.Value);
    }

    [Theory]
    [InlineData("Germany", true)]
    [InlineData("United States", true)]
    [InlineData("C", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var name = new CountryName(value);
        Assert.Equal(expected, name.IsValidFormat());
    }

    [Fact]
    public void ToUpperCase_ReturnsExpected()
    {
        var name = new CountryName("Germany");
        Assert.Equal("GERMANY", name.ToUpperCase());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new StrongStringTypeConverter<CountryName>();
        var result = converter.ConvertFrom("Germany") as CountryName;

        Assert.NotNull(result);
        Assert.Equal("Germany", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new StrongStringTypeConverter<CountryName>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
