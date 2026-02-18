// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Network.UnitTests;

/// <summary>
/// Tests for <see cref="HttpMethod"/>.
/// </summary>
public class HttpMethodTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var method = new HttpMethod("GET");
        Assert.Equal("GET", method.Value);
    }

    [Theory]
    [InlineData("GET", true)]
    [InlineData("post", true)]
    [InlineData("FOO", false)]
    [InlineData("", false)]
    public void IsStandard_ReturnsExpected(string value, bool expected)
    {
        var method = new HttpMethod(value);
        Assert.Equal(expected, method.IsStandard());
    }

    [Fact]
    public void ToUpperCase_ReturnsExpected()
    {
        var method = new HttpMethod("post");
        Assert.Equal("POST", method.ToUpperCase());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new HttpMethodTypeConverter();
        var result = converter.ConvertFrom("GET") as HttpMethod;

        Assert.NotNull(result);
        Assert.Equal("GET", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new HttpMethodTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
