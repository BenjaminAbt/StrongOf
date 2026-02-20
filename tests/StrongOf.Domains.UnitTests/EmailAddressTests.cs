// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Networking.UnitTests;

/// <summary>
/// Tests for <see cref="EmailAddress"/>.
/// </summary>
public class EmailAddressTests
{
    [Fact]
    public void Constructor_WithValidEmail_SetsValue()
    {
        // Arrange
        const string email = "user@example.com";

        // Act
        var emailAddress = new EmailAddress(email);

        // Assert
        Assert.Equal(email, emailAddress.Value);
    }

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("user.name@example.com", true)]
    [InlineData("user+tag@example.com", true)]
    [InlineData("user@sub.example.com", true)]
    [InlineData("user123@example.co.uk", true)]
    [InlineData("invalid", false)]
    [InlineData("invalid@", false)]
    [InlineData("@example.com", false)]
    [InlineData("user@", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string email, bool expected)
    {
        // Arrange
        var emailAddress = new EmailAddress(email);

        // Act
        bool result = emailAddress.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("user@example.com", "example.com")]
    [InlineData("user@sub.example.com", "sub.example.com")]
    [InlineData("invalid", "")]
    public void GetDomain_ReturnsExpectedResult(string email, string expectedDomain)
    {
        // Arrange
        var emailAddress = new EmailAddress(email);

        // Act
        string domain = emailAddress.GetDomain();

        // Assert
        Assert.Equal(expectedDomain, domain);
    }

    [Theory]
    [InlineData("user@example.com", "user")]
    [InlineData("user.name@example.com", "user.name")]
    [InlineData("invalid", "invalid")]
    public void GetLocalPart_ReturnsExpectedResult(string email, string expectedLocalPart)
    {
        // Arrange
        var emailAddress = new EmailAddress(email);

        // Act
        string localPart = emailAddress.GetLocalPart();

        // Assert
        Assert.Equal(expectedLocalPart, localPart);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var email1 = new EmailAddress("user@example.com");
        var email2 = new EmailAddress("user@example.com");

        // Act & Assert
        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
        Assert.False(email1 != email2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var email1 = new EmailAddress("user1@example.com");
        var email2 = new EmailAddress("user2@example.com");

        // Act & Assert
        Assert.NotEqual(email1, email2);
        Assert.False(email1 == email2);
        Assert.True(email1 != email2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string email = "user@example.com";
        var emailAddress = new EmailAddress(email);

        // Act
        string result = emailAddress.ToString();

        // Assert
        Assert.Equal(email, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var email1 = new EmailAddress("user@example.com");
        var email2 = new EmailAddress("user@example.com");

        // Act & Assert
        Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string email = "user@example.com";

        // Act
        var emailAddress = EmailAddress.From(email);

        // Assert
        Assert.Equal(email, emailAddress.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<EmailAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsEmailAddress()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<EmailAddress>();
        const string email = "user@example.com";

        // Act
        var result = converter.ConvertFrom(email) as EmailAddress;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<EmailAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
