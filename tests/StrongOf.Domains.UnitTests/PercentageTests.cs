// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="Percentage"/>.
/// </summary>
public class PercentageTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const decimal value = 75.5m;

        // Act
        var percentage = new Percentage(value);

        // Assert
        Assert.Equal(value, percentage.Value);
    }

    [Fact]
    public void FromFraction_ConvertsToPercentage()
    {
        // Arrange
        const decimal fraction = 0.755m;

        // Act
        var percentage = Percentage.FromFraction(fraction);

        // Assert
        Assert.Equal(75.5m, percentage.Value);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(50, true)]
    [InlineData(100, true)]
    [InlineData(-1, false)]
    [InlineData(101, false)]
    [InlineData(150, false)]
    public void IsValidRange_ReturnsExpectedResult(decimal value, bool expected)
    {
        // Arrange
        var percentage = new Percentage(value);

        // Act
        bool result = percentage.IsValidRange();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(50, 0.5)]
    [InlineData(100, 1)]
    [InlineData(75.5, 0.755)]
    public void ToFraction_ReturnsExpectedResult(decimal value, decimal expected)
    {
        // Arrange
        var percentage = new Percentage(value);

        // Act
        decimal result = percentage.ToFraction();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-10, 0)]
    [InlineData(0, 0)]
    [InlineData(50, 50)]
    [InlineData(100, 100)]
    [InlineData(150, 100)]
    public void Clamp_ReturnsClampedValue(decimal value, decimal expected)
    {
        // Arrange
        var percentage = new Percentage(value);

        // Act
        var result = percentage.Clamp();

        // Assert
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var p1 = new Percentage(75.5m);
        var p2 = new Percentage(75.5m);

        // Act & Assert
        Assert.Equal(p1, p2);
        Assert.True(p1 == p2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var p1 = new Percentage(75.5m);
        var p2 = new Percentage(50m);

        // Act & Assert
        Assert.NotEqual(p1, p2);
        Assert.True(p1 != p2);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var p1 = new Percentage(75.5m);
        var p2 = new Percentage(75.5m);

        // Act & Assert
        Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const decimal value = 75.5m;

        // Act
        var percentage = Percentage.From(value);

        // Assert
        Assert.Equal(value, percentage.Value);
    }

    [Fact]
    public void MinValue_IsZero()
    {
        Assert.Equal(0m, Percentage.MinValue);
    }

    [Fact]
    public void MaxValue_IsOneHundred()
    {
        Assert.Equal(100m, Percentage.MaxValue);
    }

    [Fact]
    public void TypeConverter_CanConvertFromDecimal()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(decimal));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromDouble()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(double));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromDecimal_ReturnsPercentage()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        var result = converter.ConvertFrom(75.5m) as Percentage;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(75.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromDouble_ReturnsPercentage()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        var result = converter.ConvertFrom(75.5) as Percentage;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(75.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsPercentage()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        var result = converter.ConvertFrom(75) as Percentage;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(75m, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPercentage()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        var result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "75.5") as Percentage;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(75.5m, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        // Arrange
        var converter = new PercentageTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(Guid));

        // Assert
        Assert.False(canConvert);
    }
}
