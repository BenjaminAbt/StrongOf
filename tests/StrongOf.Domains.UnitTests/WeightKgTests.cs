// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="WeightKg"/>.
/// </summary>
public class WeightKgTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        WeightKg weight = new WeightKg(80m);
        Assert.Equal(80m, weight.Value);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(80, true)]
    [InlineData(500, true)]
    [InlineData(-1, false)]
    [InlineData(501, false)]
    public void IsValidRange_ReturnsExpected(decimal value, bool expected)
    {
        WeightKg weight = new WeightKg(value);
        Assert.Equal(expected, weight.IsValidRange());
    }

    [Fact]
    public void ToGrams_ReturnsExpected()
    {
        WeightKg weight = new WeightKg(1.5m);
        Assert.Equal(1500m, weight.ToGrams());
    }

    [Fact]
    public void Clamp_ReturnsClampedValue()
    {
        WeightKg weight = new WeightKg(800m);
        WeightKg clamped = weight.Clamp();
        Assert.Equal(WeightKg.MaxValue, clamped.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongDecimalTypeConverter<WeightKg> converter = new StrongDecimalTypeConverter<WeightKg>();
        WeightKg? result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "80") as WeightKg;

        Assert.NotNull(result);
        Assert.Equal(80m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsInstance()
    {
        StrongDecimalTypeConverter<WeightKg> converter = new StrongDecimalTypeConverter<WeightKg>();
        WeightKg? result = converter.ConvertFrom(80m) as WeightKg;

        Assert.NotNull(result);
        Assert.Equal(80m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsInstance()
    {
        StrongDecimalTypeConverter<WeightKg> converter = new StrongDecimalTypeConverter<WeightKg>();
        WeightKg? result = converter.ConvertFrom(80.5) as WeightKg;

        Assert.NotNull(result);
        Assert.Equal(80.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsInstance()
    {
        StrongDecimalTypeConverter<WeightKg> converter = new StrongDecimalTypeConverter<WeightKg>();
        WeightKg? result = converter.ConvertFrom(80) as WeightKg;

        Assert.NotNull(result);
        Assert.Equal(80m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        StrongDecimalTypeConverter<WeightKg> converter = new StrongDecimalTypeConverter<WeightKg>();
        Assert.False(converter.CanConvertFrom(typeof(Guid)));
    }
}
