// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Finance.UnitTests;

/// <summary>
/// Tests for <see cref="CurrencyCode"/>.
/// </summary>
public class CurrencyCodeTests
{
    [Fact]
    public void Constructor_WithValidCode_SetsValue()
    {
        // Arrange
        const string code = "USD";

        // Act
        var currencyCode = new CurrencyCode(code);

        // Assert
        Assert.Equal(code, currencyCode.Value);
    }

    [Theory]
    [InlineData("USD", true)]
    [InlineData("EUR", true)]
    [InlineData("GBP", true)]
    [InlineData("usd", true)] // Lowercase letters are also valid
    [InlineData("US", false)] // Wrong length
    [InlineData("USDD", false)] // Too long
    [InlineData("123", false)] // Digits not allowed
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string code, bool expected)
    {
        // Arrange
        var currencyCode = new CurrencyCode(code);

        // Act
        bool result = currencyCode.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("usd", "USD")]
    [InlineData("USD", "USD")]
    [InlineData("eur", "EUR")]
    public void ToUpperCase_ReturnsExpectedResult(string code, string expected)
    {
        // Arrange
        var currencyCode = new CurrencyCode(code);

        // Act
        string result = currencyCode.ToUpperCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var code1 = new CurrencyCode("USD");
        var code2 = new CurrencyCode("USD");

        // Act & Assert
        Assert.Equal(code1, code2);
        Assert.True(code1 == code2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var code1 = new CurrencyCode("USD");
        var code2 = new CurrencyCode("EUR");

        // Act & Assert
        Assert.NotEqual(code1, code2);
        Assert.True(code1 != code2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string code = "USD";
        var currencyCode = new CurrencyCode(code);

        // Act
        string result = currencyCode.ToString();

        // Assert
        Assert.Equal(code, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var code1 = new CurrencyCode("USD");
        var code2 = new CurrencyCode("USD");

        // Act & Assert
        Assert.Equal(code1.GetHashCode(), code2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string code = "USD";

        // Act
        var currencyCode = CurrencyCode.From(code);

        // Assert
        Assert.Equal(code, currencyCode.Value);
    }

    [Fact]
    public void RequiredLength_IsThree()
    {
        Assert.Equal(3, CurrencyCode.RequiredLength);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<CurrencyCode>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsCurrencyCode()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<CurrencyCode>();
        const string code = "USD";

        // Act
        var result = converter.ConvertFrom(code) as CurrencyCode;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<CurrencyCode>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
