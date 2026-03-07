// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="FileExtension"/>.
/// </summary>
public class FileExtensionTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        FileExtension ext = new FileExtension(".txt");
        Assert.Equal(".txt", ext.Value);
    }

    [Theory]
    [InlineData(".txt", true)]
    [InlineData(".jpeg", true)]
    [InlineData("txt", false)]
    [InlineData(".", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        FileExtension ext = new FileExtension(value);
        Assert.Equal(expected, ext.IsValidFormat());
    }

    [Fact]
    public void WithoutDot_ReturnsExpected()
    {
        FileExtension ext = new FileExtension(".txt");
        Assert.Equal("txt", ext.WithoutDot());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<FileExtension> converter = new StrongStringTypeConverter<FileExtension>();
        FileExtension? result = converter.ConvertFrom(".txt") as FileExtension;

        Assert.NotNull(result);
        Assert.Equal(".txt", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<FileExtension> converter = new StrongStringTypeConverter<FileExtension>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
