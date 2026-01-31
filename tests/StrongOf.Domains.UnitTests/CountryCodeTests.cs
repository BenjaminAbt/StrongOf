// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="CountryCode"/>.
/// </summary>
public class CountryCodeTests
{
    [Fact]
    public void Constructor_WithValidCode_SetsValue()
    {
        // Arrange
        const string code = "US";

        // Act
        var countryCode = new CountryCode(code);

        // Assert
        Assert.Equal(code, countryCode.Value);
    }

    [Theory]
    [InlineData("US", true)]
    [InlineData("DE", true)]
    [InlineData("GB", true)]
    [InlineData("us", true)] // Lowercase letters are also valid
    [InlineData("USA", false)] // Wrong length
    [InlineData("U", false)] // Too short
    [InlineData("12", false)] // Digits not allowed
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string code, bool expected)
    {
        // Arrange
        var countryCode = new CountryCode(code);

        // Act
        bool result = countryCode.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("us", "US")]
    [InlineData("US", "US")]
    [InlineData("de", "DE")]
    public void ToUpperCase_ReturnsExpectedResult(string code, string expected)
    {
        // Arrange
        var countryCode = new CountryCode(code);

        // Act
        string result = countryCode.ToUpperCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var code1 = new CountryCode("US");
        var code2 = new CountryCode("US");

        // Act & Assert
        Assert.Equal(code1, code2);
        Assert.True(code1 == code2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var code1 = new CountryCode("US");
        var code2 = new CountryCode("DE");

        // Act & Assert
        Assert.NotEqual(code1, code2);
        Assert.True(code1 != code2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string code = "US";
        var countryCode = new CountryCode(code);

        // Act
        string result = countryCode.ToString();

        // Assert
        Assert.Equal(code, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var code1 = new CountryCode("US");
        var code2 = new CountryCode("US");

        // Act & Assert
        Assert.Equal(code1.GetHashCode(), code2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string code = "US";

        // Act
        var countryCode = CountryCode.From(code);

        // Assert
        Assert.Equal(code, countryCode.Value);
    }

    [Fact]
    public void RequiredLength_IsTwo()
    {
        Assert.Equal(2, CountryCode.RequiredLength);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new CountryCodeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsCountryCode()
    {
        // Arrange
        var converter = new CountryCodeTypeConverter();
        const string code = "US";

        // Act
        var result = converter.ConvertFrom(code) as CountryCode;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new CountryCodeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
