// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="Longitude"/>.
/// </summary>
public class LongitudeTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        Longitude lon = new Longitude(20.5m);
        Assert.Equal(20.5m, lon.Value);
    }

    [Theory]
    [InlineData(-180, true)]
    [InlineData(0, true)]
    [InlineData(180, true)]
    [InlineData(-181, false)]
    [InlineData(181, false)]
    public void IsValidRange_ReturnsExpected(decimal value, bool expected)
    {
        Longitude lon = new Longitude(value);
        Assert.Equal(expected, lon.IsValidRange());
    }

    [Fact]
    public void Clamp_ReturnsClampedValue()
    {
        Longitude lon = new Longitude(200m);
        Longitude clamped = lon.Clamp();
        Assert.Equal(Longitude.MaxValue, clamped.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongDecimalTypeConverter<Longitude> converter = new StrongDecimalTypeConverter<Longitude>();
        Longitude? result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "20.5") as Longitude;

        Assert.NotNull(result);
        Assert.Equal(20.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsInstance()
    {
        StrongDecimalTypeConverter<Longitude> converter = new StrongDecimalTypeConverter<Longitude>();
        Longitude? result = converter.ConvertFrom(20.5m) as Longitude;

        Assert.NotNull(result);
        Assert.Equal(20.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsInstance()
    {
        StrongDecimalTypeConverter<Longitude> converter = new StrongDecimalTypeConverter<Longitude>();
        Longitude? result = converter.ConvertFrom(20.25) as Longitude;

        Assert.NotNull(result);
        Assert.Equal(20.25m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        StrongDecimalTypeConverter<Longitude> converter = new StrongDecimalTypeConverter<Longitude>();
        Longitude? result = converter.ConvertFrom(20) as Longitude;

        Assert.NotNull(result);
        Assert.Equal(20m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        StrongDecimalTypeConverter<Longitude> converter = new StrongDecimalTypeConverter<Longitude>();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
