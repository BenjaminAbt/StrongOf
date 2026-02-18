// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="HeightCm"/>.
/// </summary>
public class HeightCmTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var height = new HeightCm(180m);
        Assert.Equal(180m, height.Value);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(180, true)]
    [InlineData(300, true)]
    [InlineData(-1, false)]
    [InlineData(301, false)]
    public void IsValidRange_ReturnsExpected(decimal value, bool expected)
    {
        var height = new HeightCm(value);
        Assert.Equal(expected, height.IsValidRange());
    }

    [Fact]
    public void ToMeters_ReturnsExpected()
    {
        var height = new HeightCm(180m);
        Assert.Equal(1.8m, height.ToMeters());
    }

    [Fact]
    public void Clamp_ReturnsClampedValue()
    {
        var height = new HeightCm(400m);
        var clamped = height.Clamp();
        Assert.Equal(HeightCm.MaxValue, clamped.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new HeightCmTypeConverter();
        var result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "180") as HeightCm;

        Assert.NotNull(result);
        Assert.Equal(180m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsInstance()
    {
        var converter = new HeightCmTypeConverter();
        var result = converter.ConvertFrom(180m) as HeightCm;

        Assert.NotNull(result);
        Assert.Equal(180m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsInstance()
    {
        var converter = new HeightCmTypeConverter();
        var result = converter.ConvertFrom(180.5) as HeightCm;

        Assert.NotNull(result);
        Assert.Equal(180.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        var converter = new HeightCmTypeConverter();
        var result = converter.ConvertFrom(180) as HeightCm;

        Assert.NotNull(result);
        Assert.Equal(180m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        var converter = new HeightCmTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
