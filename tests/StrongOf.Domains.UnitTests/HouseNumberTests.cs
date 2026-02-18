// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Address.UnitTests;

/// <summary>
/// Tests for <see cref="HouseNumber"/>.
/// </summary>
public class HouseNumberTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var number = new HouseNumber("12A");
        Assert.Equal("12A", number.Value);
    }

    [Theory]
    [InlineData("12", true)]
    [InlineData("12A", true)]
    [InlineData("12/3", true)]
    [InlineData("12-3", true)]
    [InlineData("A12", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var number = new HouseNumber(value);
        Assert.Equal(expected, number.IsValidFormat());
    }

    [Fact]
    public void GetNumericPart_ReturnsExpected()
    {
        var number = new HouseNumber("123B");
        Assert.Equal(123, number.GetNumericPart());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new HouseNumberTypeConverter();
        var result = converter.ConvertFrom("12") as HouseNumber;

        Assert.NotNull(result);
        Assert.Equal("12", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        var converter = new HouseNumberTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
