// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="FullName"/>.
/// </summary>
public class FullNameTests
{
    [Fact]
    public void Constructor_WithValidName_SetsValue()
    {
        // Arrange
        const string name = "John Smith";

        // Act
        var fullName = new FullName(name);

        // Assert
        Assert.Equal(name, fullName.Value);
    }

    [Fact]
    public void FromNames_CreatesFullName()
    {
        // Arrange
        var firstName = new FirstName("John");
        var lastName = new LastName("Smith");

        // Act
        var fullName = FullName.FromNames(firstName, lastName);

        // Assert
        Assert.Equal("John Smith", fullName.Value);
    }

    [Theory]
    [InlineData("John Smith", true)]
    [InlineData("John Michael Smith", true)]
    [InlineData("John", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string name, bool expected)
    {
        // Arrange
        var fullName = new FullName(name);

        // Act
        bool result = fullName.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("John Smith", "John")]
    [InlineData("John Michael Smith", "John")]
    [InlineData("John", "John")]
    public void GetFirstPart_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var fullName = new FullName(name);

        // Act
        string result = fullName.GetFirstPart();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("John Smith", "Smith")]
    [InlineData("John Michael Smith", "Smith")]
    [InlineData("John", "John")]
    public void GetLastPart_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var fullName = new FullName(name);

        // Act
        string result = fullName.GetLastPart();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("John Smith", "JS")]
    [InlineData("John Michael Smith", "JMS")]
    [InlineData("John", "J")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    public void GetInitials_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var fullName = new FullName(name);

        // Act
        string result = fullName.GetInitials();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var name1 = new FullName("John Smith");
        var name2 = new FullName("John Smith");

        // Act & Assert
        Assert.Equal(name1, name2);
        Assert.True(name1 == name2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var name1 = new FullName("John Smith");
        var name2 = new FullName("Jane Doe");

        // Act & Assert
        Assert.NotEqual(name1, name2);
        Assert.True(name1 != name2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string name = "John Smith";
        var fullName = new FullName(name);

        // Act
        string result = fullName.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var name1 = new FullName("John Smith");
        var name2 = new FullName("John Smith");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string name = "John Smith";

        // Act
        var fullName = FullName.From(name);

        // Assert
        Assert.Equal(name, fullName.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new FullNameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsFullName()
    {
        // Arrange
        var converter = new FullNameTypeConverter();
        const string name = "John Smith";

        // Act
        var result = converter.ConvertFrom(name) as FullName;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new FullNameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
