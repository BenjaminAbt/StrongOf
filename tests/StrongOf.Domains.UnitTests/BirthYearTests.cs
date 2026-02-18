// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Person.UnitTests;

/// <summary>
/// Tests for <see cref="BirthYear"/>.
/// </summary>
public class BirthYearTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        var year = new BirthYear(1990);
        Assert.Equal(1990, year.Value);
    }

    [Theory]
    [InlineData(1900, true)]
    [InlineData(2000, true)]
    [InlineData(2100, true)]
    [InlineData(1899, false)]
    [InlineData(2101, false)]
    public void IsValidRange_ReturnsExpected(int value, bool expected)
    {
        var year = new BirthYear(value);
        Assert.Equal(expected, year.IsValidRange());
    }

    [Fact]
    public void IsLeapYear_ReturnsExpected()
    {
        var leap = new BirthYear(2000);
        var nonLeap = new BirthYear(2001);

        Assert.True(leap.IsLeapYear());
        Assert.False(nonLeap.IsLeapYear());
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        var converter = new BirthYearTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(int)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new BirthYearTypeConverter();
        var result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "1990") as BirthYear;

        Assert.NotNull(result);
        Assert.Equal(1990, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        var converter = new BirthYearTypeConverter();
        var result = converter.ConvertFrom(1990) as BirthYear;

        Assert.NotNull(result);
        Assert.Equal(1990, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        var converter = new BirthYearTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
