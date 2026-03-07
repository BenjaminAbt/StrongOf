// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="City"/>.
/// </summary>
public class CityTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "New York";

        // Act
        City city = new City(value);

        // Assert
        Assert.Equal(value, city.Value);
    }

    [Theory]
    [InlineData("New York", true)]
    [InlineData("Los Angeles", true)]
    [InlineData("O'Brien", true)]
    [InlineData("Saint-Denis", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("City123", false)]
    [InlineData("City@", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        City city = new City(value);

        // Act
        bool result = city.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        City city1 = new City("New York");
        City city2 = new City("New York");

        // Act & Assert
        Assert.Equal(city1, city2);
        Assert.True(city1 == city2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        City city1 = new City("New York");
        City city2 = new City("Los Angeles");

        // Act & Assert
        Assert.NotEqual(city1, city2);
        Assert.True(city1 != city2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "New York";
        City city = new City(value);

        // Act
        string result = city.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        City city1 = new City("New York");
        City city2 = new City("New York");

        // Act & Assert
        Assert.Equal(city1.GetHashCode(), city2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "New York";

        // Act
        City city = City.From(value);

        // Assert
        Assert.Equal(value, city.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        StrongStringTypeConverter<City> converter = new StrongStringTypeConverter<City>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsCity()
    {
        // Arrange
        StrongStringTypeConverter<City> converter = new StrongStringTypeConverter<City>();
        const string value = "New York";

        // Act
        City? result = converter.ConvertFrom(value) as City;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        StrongStringTypeConverter<City> converter = new StrongStringTypeConverter<City>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
