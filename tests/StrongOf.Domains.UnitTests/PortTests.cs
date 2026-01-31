// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="Port"/>.
/// </summary>
public class PortTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        const int value = 443;

        // Act
        var port = new Port(value);

        // Assert
        Assert.Equal(value, port.Value);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(80, true)]
    [InlineData(443, true)]
    [InlineData(65535, true)]
    [InlineData(-1, false)]
    [InlineData(65536, false)]
    public void IsValidRange_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var port = new Port(value);

        // Act
        bool result = port.IsValidRange();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(80, true)]
    [InlineData(443, true)]
    [InlineData(1023, true)]
    [InlineData(1024, false)]
    [InlineData(8080, false)]
    public void IsWellKnownPort_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var port = new Port(value);

        // Act
        bool result = port.IsWellKnownPort();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1024, true)]
    [InlineData(8080, true)]
    [InlineData(49151, true)]
    [InlineData(1023, false)]
    [InlineData(49152, false)]
    public void IsRegisteredPort_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var port = new Port(value);

        // Act
        bool result = port.IsRegisteredPort();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(49152, true)]
    [InlineData(50000, true)]
    [InlineData(65535, true)]
    [InlineData(49151, false)]
    [InlineData(1024, false)]
    public void IsDynamicPort_ReturnsExpectedResult(int value, bool expected)
    {
        // Arrange
        var port = new Port(value);

        // Act
        bool result = port.IsDynamicPort();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Http_ReturnsPort80()
    {
        // Act
        var port = Port.Http;

        // Assert
        Assert.Equal(80, port.Value);
    }

    [Fact]
    public void Https_ReturnsPort443()
    {
        // Act
        var port = Port.Https;

        // Assert
        Assert.Equal(443, port.Value);
    }

    [Fact]
    public void Ssh_ReturnsPort22()
    {
        // Act
        var port = Port.Ssh;

        // Assert
        Assert.Equal(22, port.Value);
    }

    [Fact]
    public void Ftp_ReturnsPort21()
    {
        // Act
        var port = Port.Ftp;

        // Assert
        Assert.Equal(21, port.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var port1 = new Port(443);
        var port2 = new Port(443);

        // Act & Assert
        Assert.Equal(port1, port2);
        Assert.True(port1 == port2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var port1 = new Port(80);
        var port2 = new Port(443);

        // Act & Assert
        Assert.NotEqual(port1, port2);
        Assert.True(port1 != port2);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var port1 = new Port(443);
        var port2 = new Port(443);

        // Act & Assert
        Assert.Equal(port1.GetHashCode(), port2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const int value = 443;

        // Act
        var port = Port.From(value);

        // Assert
        Assert.Equal(value, port.Value);
    }

    [Fact]
    public void MinValue_IsZero()
    {
        Assert.Equal(0, Port.MinValue);
    }

    [Fact]
    public void MaxValue_Is65535()
    {
        Assert.Equal(65535, Port.MaxValue);
    }

    [Fact]
    public void WellKnownPortMax_Is1023()
    {
        Assert.Equal(1023, Port.WellKnownPortMax);
    }

    [Fact]
    public void RegisteredPortMax_Is49151()
    {
        Assert.Equal(49151, Port.RegisteredPortMax);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        // Arrange
        var converter = new PortTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new PortTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsPort()
    {
        // Arrange
        var converter = new PortTypeConverter();

        // Act
        var result = converter.ConvertFrom(443) as Port;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(443, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPort()
    {
        // Arrange
        var converter = new PortTypeConverter();

        // Act
        var result = converter.ConvertFrom("443") as Port;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(443, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid_ReturnsFalse()
    {
        // Arrange
        var converter = new PortTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(Guid));

        // Assert
        Assert.False(canConvert);
    }
}
