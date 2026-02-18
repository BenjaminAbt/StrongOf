// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Geography.UnitTests;

/// <summary>
/// Tests for <see cref="Latitude"/>.
/// </summary>
public class LatitudeTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var lat = new Latitude(10.5m);
        Assert.Equal(10.5m, lat.Value);
    }

    [Theory]
    [InlineData(-90, true)]
    [InlineData(0, true)]
    [InlineData(90, true)]
    [InlineData(-91, false)]
    [InlineData(91, false)]
    public void IsValidRange_ReturnsExpected(decimal value, bool expected)
    {
        var lat = new Latitude(value);
        Assert.Equal(expected, lat.IsValidRange());
    }

    [Fact]
    public void Clamp_ReturnsClampedValue()
    {
        var lat = new Latitude(120m);
        var clamped = lat.Clamp();
        Assert.Equal(Latitude.MaxValue, clamped.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new StrongDecimalTypeConverter<Latitude>();
        var result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "10.5") as Latitude;

        Assert.NotNull(result);
        Assert.Equal(10.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsInstance()
    {
        var converter = new StrongDecimalTypeConverter<Latitude>();
        var result = converter.ConvertFrom(10.5m) as Latitude;

        Assert.NotNull(result);
        Assert.Equal(10.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsInstance()
    {
        var converter = new StrongDecimalTypeConverter<Latitude>();
        var result = converter.ConvertFrom(10.25) as Latitude;

        Assert.NotNull(result);
        Assert.Equal(10.25m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        var converter = new StrongDecimalTypeConverter<Latitude>();
        var result = converter.ConvertFrom(10) as Latitude;

        Assert.NotNull(result);
        Assert.Equal(10m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        var converter = new StrongDecimalTypeConverter<Latitude>();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
