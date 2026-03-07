// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="FirstName"/>.
/// </summary>
public class FirstNameTests
{
    [Fact]
    public void Constructor_WithValidName_SetsValue()
    {
        // Arrange
        const string name = "John";

        // Act
        FirstName firstName = new FirstName(name);

        // Assert
        Assert.Equal(name, firstName.Value);
    }

    [Theory]
    [InlineData("John", true)]
    [InlineData("Mary-Anne", true)]
    [InlineData("O'Brien", true)]
    [InlineData("Jean Pierre", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("John123", false)]
    [InlineData("John@", false)]
    public void IsValidFormat_ReturnsExpectedResult(string name, bool expected)
    {
        // Arrange
        FirstName firstName = new FirstName(name);

        // Act
        bool result = firstName.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("john", "John")]
    [InlineData("JOHN", "John")]
    [InlineData("jOHN", "John")]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void ToTitleCase_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        FirstName firstName = new FirstName(name);

        // Act
        string result = firstName.ToTitleCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        FirstName name1 = new FirstName("John");
        FirstName name2 = new FirstName("John");

        // Act & Assert
        Assert.Equal(name1, name2);
        Assert.True(name1 == name2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        FirstName name1 = new FirstName("John");
        FirstName name2 = new FirstName("Jane");

        // Act & Assert
        Assert.NotEqual(name1, name2);
        Assert.True(name1 != name2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string name = "John";
        FirstName firstName = new FirstName(name);

        // Act
        string result = firstName.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        FirstName name1 = new FirstName("John");
        FirstName name2 = new FirstName("John");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string name = "John";

        // Act
        FirstName firstName = FirstName.From(name);

        // Assert
        Assert.Equal(name, firstName.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        StrongStringTypeConverter<FirstName> converter = new StrongStringTypeConverter<FirstName>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsFirstName()
    {
        // Arrange
        StrongStringTypeConverter<FirstName> converter = new StrongStringTypeConverter<FirstName>();
        const string name = "John";

        // Act
        FirstName? result = converter.ConvertFrom(name) as FirstName;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        StrongStringTypeConverter<FirstName> converter = new StrongStringTypeConverter<FirstName>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
