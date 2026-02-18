// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Software.UnitTests;

/// <summary>
/// Tests for <see cref="SemVer"/>.
/// </summary>
public class SemVerTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var version = new SemVer("1.2.3");
        Assert.Equal("1.2.3", version.Value);
    }

    [Theory]
    [InlineData("1.0.0", true)]
    [InlineData("1.2.3-alpha", true)]
    [InlineData("1.2.3+build", true)]
    [InlineData("01.2.3", false)]
    [InlineData("1.2", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var version = new SemVer(value);
        Assert.Equal(expected, version.IsValidFormat());
    }

    [Fact]
    public void TryGetMajor_ReturnsExpected()
    {
        var version = new SemVer("2.1.0");
        Assert.True(version.TryGetMajor(out int major));
        Assert.Equal(2, major);
    }

    [Fact]
    public void TryGetMajor_WithInvalid_ReturnsFalse()
    {
        var version = new SemVer(string.Empty);
        Assert.False(version.TryGetMajor(out int _));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new SemVerTypeConverter();
        var result = converter.ConvertFrom("1.2.3") as SemVer;

        Assert.NotNull(result);
        Assert.Equal("1.2.3", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new SemVerTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
