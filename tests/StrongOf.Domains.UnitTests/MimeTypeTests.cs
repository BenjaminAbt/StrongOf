// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

/// <summary>
/// Tests for <see cref="MimeType"/>.
/// </summary>
public class MimeTypeTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var mime = new MimeType("text/plain");
        Assert.Equal("text/plain", mime.Value);
    }

    [Theory]
    [InlineData("text/plain", true)]
    [InlineData("application/json", true)]
    [InlineData("invalid", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var mime = new MimeType(value);
        Assert.Equal(expected, mime.IsValidFormat());
    }

    [Fact]
    public void GetTypePart_ReturnsExpected()
    {
        var mime = new MimeType("text/plain");
        Assert.Equal("text", mime.GetTypePart());
    }

    [Fact]
    public void GetSubtypePart_ReturnsExpected()
    {
        var mime = new MimeType("text/plain");
        Assert.Equal("plain", mime.GetSubtypePart());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new MimeTypeTypeConverter();
        var result = converter.ConvertFrom("text/plain") as MimeType;

        Assert.NotNull(result);
        Assert.Equal("text/plain", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new MimeTypeTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
