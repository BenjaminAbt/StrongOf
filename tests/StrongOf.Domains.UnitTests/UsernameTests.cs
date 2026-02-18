// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Identity.UnitTests;

/// <summary>
/// Tests for <see cref="Username"/>.
/// </summary>
public class UsernameTests
{
    [Fact]
    public void Constructor_WithValidUsername_SetsValue()
    {
        // Arrange
        const string name = "john_doe123";

        // Act
        var username = new Username(name);

        // Assert
        Assert.Equal(name, username.Value);
    }

    [Theory]
    [InlineData("john", true)]
    [InlineData("john_doe", true)]
    [InlineData("john-doe", true)]
    [InlineData("john123", true)]
    [InlineData("John_Doe_123", true)]
    [InlineData("ab", false)] // Too short
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("john@doe", false)] // Invalid character
    [InlineData("john.doe", false)] // Invalid character
    public void IsValidFormat_ReturnsExpectedResult(string name, bool expected)
    {
        // Arrange
        var username = new Username(name);

        // Act
        bool result = username.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValidFormat_TooLong_ReturnsFalse()
    {
        // Arrange
        string name = new('a', 65); // 65 chars, max is 64
        var username = new Username(name);

        // Act
        bool result = username.IsValidFormat();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidFormat_ExactMaxLength_ReturnsTrue()
    {
        // Arrange
        string name = new('a', 64); // Exactly 64 chars
        var username = new Username(name);

        // Act
        bool result = username.IsValidFormat();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("John_Doe", "john_doe")]
    [InlineData("JOHN_DOE", "john_doe")]
    [InlineData("john_doe", "john_doe")]
    public void ToLowerCase_ReturnsExpectedResult(string name, string expected)
    {
        // Arrange
        var username = new Username(name);

        // Act
        string result = username.ToLowerCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var name1 = new Username("john_doe");
        var name2 = new Username("john_doe");

        // Act & Assert
        Assert.Equal(name1, name2);
        Assert.True(name1 == name2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var name1 = new Username("john_doe");
        var name2 = new Username("jane_doe");

        // Act & Assert
        Assert.NotEqual(name1, name2);
        Assert.True(name1 != name2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string name = "john_doe";
        var username = new Username(name);

        // Act
        string result = username.ToString();

        // Assert
        Assert.Equal(name, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var name1 = new Username("john_doe");
        var name2 = new Username("john_doe");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string name = "john_doe";

        // Act
        var username = Username.From(name);

        // Assert
        Assert.Equal(name, username.Value);
    }

    [Fact]
    public void MinLength_IsThree()
    {
        Assert.Equal(3, Username.MinLength);
    }

    [Fact]
    public void MaxLength_IsSixtyFour()
    {
        Assert.Equal(64, Username.MaxLength);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new UsernameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsUsername()
    {
        // Arrange
        var converter = new UsernameTypeConverter();
        const string name = "john_doe";

        // Act
        var result = converter.ConvertFrom(name) as Username;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new UsernameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
