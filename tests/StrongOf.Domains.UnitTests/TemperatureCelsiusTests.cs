// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="TemperatureCelsius"/>.
/// </summary>
public class TemperatureCelsiusTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var temp = new TemperatureCelsius(20m);
        Assert.Equal(20m, temp.Value);
    }

    [Theory]
    [InlineData(-273.15, true)]
    [InlineData(0, true)]
    [InlineData(1000, true)]
    [InlineData(-274, false)]
    public void IsValidRange_ReturnsExpected(decimal value, bool expected)
    {
        var temp = new TemperatureCelsius(value);
        Assert.Equal(expected, temp.IsValidRange());
    }

    [Fact]
    public void ToFahrenheit_ReturnsExpected()
    {
        var temp = new TemperatureCelsius(0m);
        Assert.Equal(32m, temp.ToFahrenheit());
    }

    [Fact]
    public void ToKelvin_ReturnsExpected()
    {
        var temp = new TemperatureCelsius(0m);
        Assert.Equal(273.15m, temp.ToKelvin());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new TemperatureCelsiusTypeConverter();
        var result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "20") as TemperatureCelsius;

        Assert.NotNull(result);
        Assert.Equal(20m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsInstance()
    {
        var converter = new TemperatureCelsiusTypeConverter();
        var result = converter.ConvertFrom(20m) as TemperatureCelsius;

        Assert.NotNull(result);
        Assert.Equal(20m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsInstance()
    {
        var converter = new TemperatureCelsiusTypeConverter();
        var result = converter.ConvertFrom(20.5) as TemperatureCelsius;

        Assert.NotNull(result);
        Assert.Equal(20.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        var converter = new TemperatureCelsiusTypeConverter();
        var result = converter.ConvertFrom(20) as TemperatureCelsius;

        Assert.NotNull(result);
        Assert.Equal(20m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        var converter = new TemperatureCelsiusTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
