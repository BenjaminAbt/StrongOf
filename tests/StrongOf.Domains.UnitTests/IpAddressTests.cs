// Copyright © Benjamin Abt 2025. All rights reserved.

using System.Net;

namespace StrongOf.Domains.Networking.UnitTests;

/// <summary>
/// Tests for <see cref="IpAddress"/>.
/// </summary>
public class IpAddressTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const string value = "192.168.1.1";

        // Act
        var ipAddress = new IpAddress(value);

        // Assert
        Assert.Equal(value, ipAddress.Value);
    }

    [Theory]
    [InlineData("192.168.1.1", true)]
    [InlineData("10.0.0.1", true)]
    [InlineData("255.255.255.255", true)]
    [InlineData("::1", true)]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true)]
    [InlineData("invalid", false)]
    [InlineData("256.256.256.256", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var ipAddress = new IpAddress(value);

        // Act
        bool result = ipAddress.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("192.168.1.1", true)]
    [InlineData("10.0.0.1", true)]
    [InlineData("::1", false)]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", false)]
    [InlineData("invalid", false)]
    public void IsIPv4_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var ipAddress = new IpAddress(value);

        // Act
        bool result = ipAddress.IsIPv4();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("::1", true)]
    [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334", true)]
    [InlineData("192.168.1.1", false)]
    [InlineData("10.0.0.1", false)]
    [InlineData("invalid", false)]
    public void IsIPv6_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var ipAddress = new IpAddress(value);

        // Act
        bool result = ipAddress.IsIPv6();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("127.0.0.1", true)]
    [InlineData("::1", true)]
    [InlineData("192.168.1.1", false)]
    [InlineData("10.0.0.1", false)]
    [InlineData("invalid", false)]
    public void IsLoopback_ReturnsExpectedResult(string value, bool expected)
    {
        // Arrange
        var ipAddress = new IpAddress(value);

        // Act
        bool result = ipAddress.IsLoopback();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToIPAddress_ValidIp_ReturnsIPAddress()
    {
        // Arrange
        var ipAddress = new IpAddress("192.168.1.1");

        // Act
        IPAddress? result = ipAddress.ToIPAddress();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(IPAddress.Parse("192.168.1.1"), result);
    }

    [Fact]
    public void ToIPAddress_InvalidIp_ReturnsNull()
    {
        // Arrange
        var ipAddress = new IpAddress("invalid");

        // Act
        IPAddress? result = ipAddress.ToIPAddress();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var ip1 = new IpAddress("192.168.1.1");
        var ip2 = new IpAddress("192.168.1.1");

        // Act & Assert
        Assert.Equal(ip1, ip2);
        Assert.True(ip1 == ip2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var ip1 = new IpAddress("192.168.1.1");
        var ip2 = new IpAddress("10.0.0.1");

        // Act & Assert
        Assert.NotEqual(ip1, ip2);
        Assert.True(ip1 != ip2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string value = "192.168.1.1";
        var ipAddress = new IpAddress(value);

        // Act
        string result = ipAddress.ToString();

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var ip1 = new IpAddress("192.168.1.1");
        var ip2 = new IpAddress("192.168.1.1");

        // Act & Assert
        Assert.Equal(ip1.GetHashCode(), ip2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string value = "192.168.1.1";

        // Act
        var ipAddress = IpAddress.From(value);

        // Assert
        Assert.Equal(value, ipAddress.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<IpAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsIpAddress()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<IpAddress>();
        const string value = "192.168.1.1";

        // Act
        var result = converter.ConvertFrom(value) as IpAddress;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new StrongStringTypeConverter<IpAddress>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
