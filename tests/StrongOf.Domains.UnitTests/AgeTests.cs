// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Person.UnitTests;

/// <summary>
/// Tests for <see cref="Age"/>.
/// </summary>
public class AgeTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const int value = 25;

        // Act
        var age = new Age(value);

        // Assert
        Assert.Equal(value, age.Value);
    }

    [Fact]
    public void FromBirthDate_CalculatesCorrectAge()
    {
        // Arrange
        DateTime today = DateTime.Today;
        DateTime birthDate = today.AddYears(-25);

        // Act
        var age = Age.FromBirthDate(birthDate);

        // Assert
        Assert.Equal(25, age.Value);
    }

    [Fact]
    public void FromBirthDate_BeforeBirthdayThisYear_ReturnsCorrectAge()
    {
        // Arrange
        DateTime today = DateTime.Today;
        DateTime birthDate = today.AddYears(-25).AddDays(1); // Birthday is tomorrow

        // Act
        var age = Age.FromBirthDate(birthDate);

        // Assert
        Assert.Equal(24, age.Value);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(25, true)]
    [InlineData(150, true)]
    [InlineData(-1, false)]
    [InlineData(151, false)]
    public void IsValidRange_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var age = new Age(value);

        // Act
        bool result = age.IsValidRange();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(18, true)]
    [InlineData(21, true)]
    [InlineData(65, true)]
    [InlineData(17, false)]
    [InlineData(0, false)]
    public void IsAdult_DefaultThreshold_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var age = new Age(value);

        // Act
        bool result = age.IsAdult();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(21, 21, true)]
    [InlineData(20, 21, false)]
    [InlineData(16, 16, true)]
    [InlineData(15, 16, false)]
    public void IsAdult_CustomThreshold_ReturnsExpectedResult(int value, int threshold, bool expected)
    {
        // Arrange
        var age = new Age(value);

        // Act
        bool result = age.IsAdult(threshold);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(17, true)]
    [InlineData(0, true)]
    [InlineData(18, false)]
    [InlineData(21, false)]
    public void IsMinor_DefaultThreshold_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var age = new Age(value);

        // Act
        bool result = age.IsMinor();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(20, 21, true)]
    [InlineData(21, 21, false)]
    public void IsMinor_CustomThreshold_ReturnsExpectedResult(int value, int threshold, bool expected)
    {
        // Arrange
        var age = new Age(value);

        // Act
        bool result = age.IsMinor(threshold);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var age1 = new Age(25);
        var age2 = new Age(25);

        // Act & Assert
        Assert.Equal(age1, age2);
        Assert.True(age1 == age2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var age1 = new Age(25);
        var age2 = new Age(30);

        // Act & Assert
        Assert.NotEqual(age1, age2);
        Assert.True(age1 != age2);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var age1 = new Age(25);
        var age2 = new Age(25);

        // Act & Assert
        Assert.Equal(age1.GetHashCode(), age2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const int value = 25;

        // Act
        var age = Age.From(value);

        // Assert
        Assert.Equal(value, age.Value);
    }

    [Fact]
    public void MinValue_IsZero()
    {
        Assert.Equal(0, Age.MinValue);
    }

    [Fact]
    public void MaxValue_IsOneHundredFifty()
    {
        Assert.Equal(150, Age.MaxValue);
    }

    [Fact]
    public void AdultAge_IsEighteen()
    {
        Assert.Equal(18, Age.AdultAge);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        // Arrange
        var converter = new AgeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new AgeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsAge()
    {
        // Arrange
        var converter = new AgeTypeConverter();

        // Act
        var result = converter.ConvertFrom(25) as Age;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsAge()
    {
        // Arrange
        var converter = new AgeTypeConverter();

        // Act
        var result = converter.ConvertFrom("25") as Age;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(25, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        // Arrange
        var converter = new AgeTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(Guid));

        // Assert
        Assert.False(canConvert);
    }
}
