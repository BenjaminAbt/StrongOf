// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Person.UnitTests;

/// <summary>
/// Tests for <see cref="LastName"/>.
/// </summary>
public class LastNameTests
{
    [Fact]
    public void Constructor_WithValidName_SetsValue()
    {
        // Arrange
        const string name = "Smith";

        // Act
        var lastName = new LastName(name);

        // Assert
        Assert.Equal(name, lastName.Value);
    }

    [Theory]
    [InlineData("Smith", true)]
    [InlineData("O'Brien", true)]
    [InlineData("Van Der Berg", true)]
    [InlineData("Smith-Jones", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Smith123", false)]
    [InlineData("Smith@", false)]
    public void IsValidFormat_ReturnsExpectedResult(string name, bool expected)
    {
        // Arrange
        var lastName = new LastName(name);

        // Act
        bool result = lastName.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("smith", "Smith")]
    [InlineData("SMITH", "Smith")]
    [InlineData("sMITH", "Smith")]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void ToTitleCase_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var lastName = new LastName(name);

        // Act
        string result = lastName.ToTitleCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Smith", "SMITH")]
    [InlineData("smith", "SMITH")]
    [InlineData("SMITH", "SMITH")]
    public void ToUpperCase_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var lastName = new LastName(name);

        // Act
        string result = lastName.ToUpperCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var name1 = new LastName("Smith");
        var name2 = new LastName("Smith");

        // Act & Assert
        Assert.Equal(name1, name2);
        Assert.True(name1 == name2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var name1 = new LastName("Smith");
        var name2 = new LastName("Jones");

        // Act & Assert
        Assert.NotEqual(name1, name2);
        Assert.True(name1 != name2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string name = "Smith";
        var lastName = new LastName(name);

        // Act
        string result = lastName.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var name1 = new LastName("Smith");
        var name2 = new LastName("Smith");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string name = "Smith";

        // Act
        var lastName = LastName.From(name);

        // Assert
        Assert.Equal(name, lastName.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<LastName>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsLastName()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<LastName>();
        const string name = "Smith";

        // Act
        var result = converter.ConvertFrom(name) as LastName;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<LastName>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
