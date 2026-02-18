// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Network.UnitTests;

/// <summary>
/// Tests for <see cref="HostName"/>.
/// </summary>
public class HostNameTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "www.example.com";

        // Act
        var hostName = new HostName(value);

        // Assert
        Assert.Equal(value, hostName.Value);
    }

    [Theory]
    [InlineData("www.example.com", true)]
    [InlineData("example.com", true)]
    [InlineData("sub.example.com", true)]
    [InlineData("localhost", true)]
    [InlineData("my-host", true)]
    [InlineData("host123", true)]
    [InlineData("-invalid", false)] // Starts with hyphen
    [InlineData("invalid-", false)] // Ends with hyphen
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var hostName = new HostName(value);

        // Act
        bool result = hostName.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void IsValidFormat_TooLong_ReturnsFalse()
    {
        // Arrange
        string value = new('a', 254); // 254 chars, max is 253
        var hostName = new HostName(value);

        // Act
        bool result = hostName.IsValidFormat();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("WWW.EXAMPLE.COM", "www.example.com")]
    [InlineData("www.example.com", "www.example.com")]
    [InlineData("Example.Com", "example.com")]
    public void ToLowerCase_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        var hostName = new HostName(value);

        // Act
        string result = hostName.ToLowerCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("www.example.com", "com")]
    [InlineData("www.example.co.uk", "uk")]
    [InlineData("localhost", "")]
    public void GetTopLevelDomain_ReturnsExpectedResult(string value, string expected)
    {
        // Arrange
        var hostName = new HostName(value);

        // Act
        string result = hostName.GetTopLevelDomain();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var host1 = new HostName("www.example.com");
        var host2 = new HostName("www.example.com");

        // Act & Assert
        Assert.Equal(host1, host2);
        Assert.True(host1 == host2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var host1 = new HostName("www.example.com");
        var host2 = new HostName("www.other.com");

        // Act & Assert
        Assert.NotEqual(host1, host2);
        Assert.True(host1 != host2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "www.example.com";
        var hostName = new HostName(value);

        // Act
        string result = hostName.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var host1 = new HostName("www.example.com");
        var host2 = new HostName("www.example.com");

        // Act & Assert
        Assert.Equal(host1.GetHashCode(), host2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "www.example.com";

        // Act
        var hostName = HostName.From(value);

        // Assert
        Assert.Equal(value, hostName.Value);
    }

    [Fact]
    public void MaxLength_Is253()
    {
        Assert.Equal(253, HostName.MaxLength);
    }

    [Fact]
    public void MaxLabelLength_Is63()
    {
        Assert.Equal(63, HostName.MaxLabelLength);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new HostNameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsHostName()
    {
        // Arrange
        var converter = new HostNameTypeConverter();
        const string value = "www.example.com";

        // Act
        var result = converter.ConvertFrom(value) as HostName;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new HostNameTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
