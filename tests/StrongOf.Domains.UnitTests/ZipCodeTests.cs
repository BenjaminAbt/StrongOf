// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="ZipCode"/>.
/// </summary>
public class ZipCodeTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "12345";

        // Act
        var zipCode = new ZipCode(value);

        // Assert
        Assert.Equal(value, zipCode.Value);
    }

    [Theory]
    [InlineData("12345", true)]
    [InlineData("12345-6789", true)]
    [InlineData("SW1A 1AA", true)]
    [InlineData("M5V 3L9", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("12345@", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var zipCode = new ZipCode(value);

        // Act
        bool result = zipCode.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("12345", true)]
    [InlineData("12345-6789", true)]
    [InlineData("1234", false)]
    [InlineData("123456", false)]
    [InlineData("12345-678", false)]
    [InlineData("", false)]
    public void IsValidUsFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var zipCode = new ZipCode(value);

        // Act
        bool result = zipCode.IsValidUsFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("12345", "12345")]
    [InlineData("  12345  ", "12345")]
    [InlineData("sw1a 1aa", "SW1A 1AA")]
    public void GetNormalized_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        var zipCode = new ZipCode(value);

        // Act
        string result = zipCode.GetNormalized();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var zip1 = new ZipCode("12345");
        var zip2 = new ZipCode("12345");

        // Act & Assert
        Assert.Equal(zip1, zip2);
        Assert.True(zip1 == zip2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var zip1 = new ZipCode("12345");
        var zip2 = new ZipCode("67890");

        // Act & Assert
        Assert.NotEqual(zip1, zip2);
        Assert.True(zip1 != zip2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "12345";
        var zipCode = new ZipCode(value);

        // Act
        string result = zipCode.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var zip1 = new ZipCode("12345");
        var zip2 = new ZipCode("12345");

        // Act & Assert
        Assert.Equal(zip1.GetHashCode(), zip2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "12345";

        // Act
        var zipCode = ZipCode.From(value);

        // Assert
        Assert.Equal(value, zipCode.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new ZipCodeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsZipCode()
    {
        // Arrange
        var converter = new ZipCodeTypeConverter();
        const string value = "12345";

        // Act
        var result = converter.ConvertFrom(value) as ZipCode;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new ZipCodeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
