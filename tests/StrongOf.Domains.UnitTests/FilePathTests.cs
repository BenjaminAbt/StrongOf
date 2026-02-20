// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

/// <summary>
/// Tests for <see cref="FilePath"/>.
/// </summary>
public class FilePathTests
{
    private static readonly string _testPath = Path.Combine("temp", "file.txt");

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        FilePath path = new(_testPath);
        Assert.Equal(_testPath, path.Value);
    }

    [Fact]
    public void IsValidPath_WithValidPath_ReturnsTrue()
    {
        FilePath path = new(_testPath);
        Assert.True(path.IsValidPath());
    }

    [Fact]
    public void IsValidPath_WithEmpty_ReturnsFalse()
    {
        FilePath path = new(" ");
        Assert.False(path.IsValidPath());
    }

    [Fact]
    public void GetExtension_ReturnsExpected()
    {
        FilePath path = new(_testPath);
        Assert.Equal(".txt", path.GetExtension());
    }

    [Fact]
    public void GetFileName_ReturnsExpected()
    {
        FilePath path = new(_testPath);
        Assert.Equal("file.txt", path.GetFileName());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new StrongStringTypeConverter<FilePath>();
        var result = converter.ConvertFrom(_testPath) as FilePath;

        Assert.NotNull(result);
        Assert.Equal(_testPath, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new StrongStringTypeConverter<FilePath>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
