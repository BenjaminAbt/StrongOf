// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="FilePath"/>.
/// </summary>
public class FilePathTests
{
    private static readonly string s_testPath = Path.Combine("temp", "file.txt");

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        FilePath path = new(s_testPath);
        Assert.Equal(s_testPath, path.Value);
    }

    [Fact]
    public void IsValidPath_WithValidPath_ReturnsTrue()
    {
        FilePath path = new(s_testPath);
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
        FilePath path = new(s_testPath);
        Assert.Equal(".txt", path.GetExtension());
    }

    [Fact]
    public void GetFileName_ReturnsExpected()
    {
        FilePath path = new(s_testPath);
        Assert.Equal("file.txt", path.GetFileName());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<FilePath> converter = new StrongStringTypeConverter<FilePath>();
        FilePath? result = converter.ConvertFrom(s_testPath) as FilePath;

        Assert.NotNull(result);
        Assert.Equal(s_testPath, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<FilePath> converter = new StrongStringTypeConverter<FilePath>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
