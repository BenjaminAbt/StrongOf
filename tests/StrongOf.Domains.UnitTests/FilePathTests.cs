// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

/// <summary>
/// Tests for <see cref="FilePath"/>.
/// </summary>
public class FilePathTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var path = new FilePath("C:\\temp\\file.txt");
        Assert.Equal("C:\\temp\\file.txt", path.Value);
    }

    [Fact]
    public void IsValidPath_WithValidPath_ReturnsTrue()
    {
        var path = new FilePath("C:\\temp\\file.txt");
        Assert.True(path.IsValidPath());
    }

    [Fact]
    public void IsValidPath_WithEmpty_ReturnsFalse()
    {
        var path = new FilePath(" ");
        Assert.False(path.IsValidPath());
    }

    [Fact]
    public void GetExtension_ReturnsExpected()
    {
        var path = new FilePath("C:\\temp\\file.txt");
        Assert.Equal(".txt", path.GetExtension());
    }

    [Fact]
    public void GetFileName_ReturnsExpected()
    {
        var path = new FilePath("C:\\temp\\file.txt");
        Assert.Equal("file.txt", path.GetFileName());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new FilePathTypeConverter();
        var result = converter.ConvertFrom("C:\\temp\\file.txt") as FilePath;

        Assert.NotNull(result);
        Assert.Equal("C:\\temp\\file.txt", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new FilePathTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
