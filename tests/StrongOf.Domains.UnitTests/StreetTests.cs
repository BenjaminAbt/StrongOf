// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="Street"/>.
/// </summary>
public class StreetTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "123 Main Street";

        // Act
        var street = new Street(value);

        // Assert
        Assert.Equal(value, street.Value);
    }

    [Theory]
    [InlineData("123 Main Street", true)]
    [InlineData("456 Oak Avenue, Apt 7", true)]
    [InlineData("P.O. Box 123", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var street = new Street(value);

        // Act
        bool result = street.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("  123 Main Street  ", "123 Main Street")]
    [InlineData("456 Oak Avenue", "456 Oak Avenue")]
    public void GetNormalized_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        var street = new Street(value);

        // Act
        string result = street.GetNormalized();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var street1 = new Street("123 Main Street");
        var street2 = new Street("123 Main Street");

        // Act & Assert
        Assert.Equal(street1, street2);
        Assert.True(street1 == street2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var street1 = new Street("123 Main Street");
        var street2 = new Street("456 Oak Avenue");

        // Act & Assert
        Assert.NotEqual(street1, street2);
        Assert.True(street1 != street2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "123 Main Street";
        var street = new Street(value);

        // Act
        string result = street.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var street1 = new Street("123 Main Street");
        var street2 = new Street("123 Main Street");

        // Act & Assert
        Assert.Equal(street1.GetHashCode(), street2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "123 Main Street";

        // Act
        var street = Street.From(value);

        // Assert
        Assert.Equal(value, street.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new StreetTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsStreet()
    {
        // Arrange
        var converter = new StreetTypeConverter();
        const string value = "123 Main Street";

        // Act
        var result = converter.ConvertFrom(value) as Street;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new StreetTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
