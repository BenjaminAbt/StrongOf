// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="MiddleName"/>.
/// </summary>
public class MiddleNameTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        MiddleName name = new MiddleName("Ann");
        Assert.Equal("Ann", name.Value);
    }

    [Theory]
    [InlineData("Ann", true)]
    [InlineData("A", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    public void IsValidLength_ReturnsExpected(string value, bool expected)
    {
        MiddleName name = new MiddleName(value);
        Assert.Equal(expected, name.IsValidLength());
    }

    [Fact]
    public void Trimmed_ReturnsTrimmedValue()
    {
        MiddleName name = new MiddleName("  Ann  ");
        Assert.Equal("Ann", name.Trimmed());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<MiddleName> converter = new StrongStringTypeConverter<MiddleName>();
        MiddleName? result = converter.ConvertFrom("Ann") as MiddleName;

        Assert.NotNull(result);
        Assert.Equal("Ann", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<MiddleName> converter = new StrongStringTypeConverter<MiddleName>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
