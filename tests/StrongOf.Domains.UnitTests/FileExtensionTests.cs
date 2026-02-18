// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

/// <summary>
/// Tests for <see cref="FileExtension"/>.
/// </summary>
public class FileExtensionTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var ext = new FileExtension(".txt");
        Assert.Equal(".txt", ext.Value);
    }

    [Theory]
    [InlineData(".txt", true)]
    [InlineData(".jpeg", true)]
    [InlineData("txt", false)]
    [InlineData(".", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var ext = new FileExtension(value);
        Assert.Equal(expected, ext.IsValidFormat());
    }

    [Fact]
    public void WithoutDot_ReturnsExpected()
    {
        var ext = new FileExtension(".txt");
        Assert.Equal("txt", ext.WithoutDot());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new FileExtensionTypeConverter();
        var result = converter.ConvertFrom(".txt") as FileExtension;

        Assert.NotNull(result);
        Assert.Equal(".txt", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new FileExtensionTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
