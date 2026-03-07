// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="MacAddress"/>.
/// </summary>
public class MacAddressTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "00:11:22:33:44:55";

        // Act
        MacAddress macAddress = new MacAddress(value);

        // Assert
        Assert.Equal(value, macAddress.Value);
    }

    [Theory]
    [InlineData("00:11:22:33:44:55", true)]
    [InlineData("00-11-22-33-44-55", true)]
    [InlineData("001122334455", true)]
    [InlineData("AA:BB:CC:DD:EE:FF", true)]
    [InlineData("aa:bb:cc:dd:ee:ff", true)]
    [InlineData("invalid", false)]
    [InlineData("00:11:22:33:44", false)]
    [InlineData("00:11:22:33:44:55:66", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        MacAddress macAddress = new MacAddress(value);

        // Act
        bool result = macAddress.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("00:11:22:33:44:55", "00:11:22:33:44:55")]
    [InlineData("00-11-22-33-44-55", "00:11:22:33:44:55")]
    [InlineData("001122334455", "00:11:22:33:44:55")]
    [InlineData("aa:bb:cc:dd:ee:ff", "AA:BB:CC:DD:EE:FF")]
    [InlineData("", "")]
    [InlineData("invalid", "invalid")] // Returns original if can't normalize
    public void GetNormalized_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        MacAddress macAddress = new MacAddress(value);

        // Act
        string result = macAddress.GetNormalized();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("00:11:22:33:44:55", "001122334455")]
    [InlineData("00-11-22-33-44-55", "001122334455")]
    [InlineData("001122334455", "001122334455")]
    [InlineData("aa:bb:cc:dd:ee:ff", "AABBCCDDEEFF")]
    public void GetWithoutSeparators_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        MacAddress macAddress = new MacAddress(value);

        // Act
        string result = macAddress.GetWithoutSeparators();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        MacAddress mac1 = new MacAddress("00:11:22:33:44:55");
        MacAddress mac2 = new MacAddress("00:11:22:33:44:55");

        // Act & Assert
        Assert.Equal(mac1, mac2);
        Assert.True(mac1 == mac2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        MacAddress mac1 = new MacAddress("00:11:22:33:44:55");
        MacAddress mac2 = new MacAddress("AA:BB:CC:DD:EE:FF");

        // Act & Assert
        Assert.NotEqual(mac1, mac2);
        Assert.True(mac1 != mac2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "00:11:22:33:44:55";
        MacAddress macAddress = new MacAddress(value);

        // Act
        string result = macAddress.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        MacAddress mac1 = new MacAddress("00:11:22:33:44:55");
        MacAddress mac2 = new MacAddress("00:11:22:33:44:55");

        // Act & Assert
        Assert.Equal(mac1.GetHashCode(), mac2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "00:11:22:33:44:55";

        // Act
        MacAddress macAddress = MacAddress.From(value);

        // Assert
        Assert.Equal(value, macAddress.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        StrongStringTypeConverter<MacAddress> converter = new StrongStringTypeConverter<MacAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsMacAddress()
    {
        // Arrange
        StrongStringTypeConverter<MacAddress> converter = new StrongStringTypeConverter<MacAddress>();
        const string value = "00:11:22:33:44:55";

        // Act
        MacAddress? result = converter.ConvertFrom(value) as MacAddress;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        StrongStringTypeConverter<MacAddress> converter = new StrongStringTypeConverter<MacAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
