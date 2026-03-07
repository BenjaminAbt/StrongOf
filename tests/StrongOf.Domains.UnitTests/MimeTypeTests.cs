// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="MimeType"/>.
/// </summary>
public class MimeTypeTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        MimeType mime = new MimeType("text/plain");
        Assert.Equal("text/plain", mime.Value);
    }

    [Theory]
    [InlineData("text/plain", true)]
    [InlineData("application/json", true)]
    [InlineData("invalid", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        MimeType mime = new MimeType(value);
        Assert.Equal(expected, mime.IsValidFormat());
    }

    [Fact]
    public void GetTypePart_ReturnsExpected()
    {
        MimeType mime = new MimeType("text/plain");
        Assert.Equal("text", mime.GetTypePart());
    }

    [Fact]
    public void GetSubtypePart_ReturnsExpected()
    {
        MimeType mime = new MimeType("text/plain");
        Assert.Equal("plain", mime.GetSubtypePart());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<MimeType> converter = new StrongStringTypeConverter<MimeType>();
        MimeType? result = converter.ConvertFrom("text/plain") as MimeType;

        Assert.NotNull(result);
        Assert.Equal("text/plain", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<MimeType> converter = new StrongStringTypeConverter<MimeType>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
