// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="PhoneNumber"/>.
/// </summary>
public class PhoneNumberTests
{
    [Fact]
    public void Constructor_WithValidPhoneNumber_SetsValue()
    {
        // Arrange
        const string phone = "+1-555-123-4567";

        // Act
        PhoneNumber phoneNumber = new PhoneNumber(phone);

        // Assert
        Assert.Equal(phone, phoneNumber.Value);
    }

    [Theory]
    [InlineData("+1-555-123-4567", true)]
    [InlineData("(555) 123-4567", true)]
    [InlineData("555.123.4567", true)]
    [InlineData("+49 123 456789", true)]
    [InlineData("1234567890", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("abc", false)]
    [InlineData("+++123", false)]
    public void IsValidFormat_ReturnsExpectedResult(string phone, bool expected)
    {
        // Arrange
        PhoneNumber phoneNumber = new PhoneNumber(phone);

        // Act
        bool result = phoneNumber.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("+1-555-123-4567", "+15551234567")]
    [InlineData("(555) 123-4567", "5551234567")]
    [InlineData("555.123.4567", "5551234567")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void GetNormalized_ReturnsExpectedResult(string phone, string expected)
    {
        // Arrange
        PhoneNumber phoneNumber = new PhoneNumber(phone);

        // Act
        string result = phoneNumber.GetNormalized();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        PhoneNumber phone1 = new PhoneNumber("+1-555-123-4567");
        PhoneNumber phone2 = new PhoneNumber("+1-555-123-4567");

        // Act & Assert
        Assert.Equal(phone1, phone2);
        Assert.True(phone1 == phone2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        PhoneNumber phone1 = new PhoneNumber("+1-555-123-4567");
        PhoneNumber phone2 = new PhoneNumber("+1-555-987-6543");

        // Act & Assert
        Assert.NotEqual(phone1, phone2);
        Assert.True(phone1 != phone2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string phone = "+1-555-123-4567";
        PhoneNumber phoneNumber = new PhoneNumber(phone);

        // Act
        string result = phoneNumber.ToString();

        // Assert
        Assert.Equal(phone, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        PhoneNumber phone1 = new PhoneNumber("+1-555-123-4567");
        PhoneNumber phone2 = new PhoneNumber("+1-555-123-4567");

        // Act & Assert
        Assert.Equal(phone1.GetHashCode(), phone2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string phone = "+1-555-123-4567";

        // Act
        PhoneNumber phoneNumber = PhoneNumber.From(phone);

        // Assert
        Assert.Equal(phone, phoneNumber.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        StrongStringTypeConverter<PhoneNumber> converter = new StrongStringTypeConverter<PhoneNumber>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPhoneNumber()
    {
        // Arrange
        StrongStringTypeConverter<PhoneNumber> converter = new StrongStringTypeConverter<PhoneNumber>();
        const string phone = "+1-555-123-4567";

        // Act
        PhoneNumber? result = converter.ConvertFrom(phone) as PhoneNumber;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phone, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        StrongStringTypeConverter<PhoneNumber> converter = new StrongStringTypeConverter<PhoneNumber>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
