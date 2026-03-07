// Copyright © Benjamin Abt 2025. All rights reserved.

using DomainHttpMethod = StrongOf.Domains.Networking.HttpMethod;

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="global::StrongOf.Domains.Networking.HttpMethod"/>.
/// </summary>
public class HttpMethodTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        DomainHttpMethod method = new DomainHttpMethod("GET");
        Assert.Equal("GET", method.Value);
    }

    [Theory]
    [InlineData("GET", true)]
    [InlineData("post", true)]
    [InlineData("FOO", false)]
    [InlineData("", false)]
    public void IsStandard_ReturnsExpected(string value, bool expected)
    {
        DomainHttpMethod method = new DomainHttpMethod(value);
        Assert.Equal(expected, method.IsStandard());
    }

    [Fact]
    public void ToUpperCase_ReturnsExpected()
    {
        DomainHttpMethod method = new DomainHttpMethod("post");
        Assert.Equal("POST", method.ToUpperCase());
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<DomainHttpMethod> converter = new StrongStringTypeConverter<DomainHttpMethod>();
        DomainHttpMethod? result = converter.ConvertFrom("GET") as DomainHttpMethod;

        Assert.NotNull(result);
        Assert.Equal("GET", result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<DomainHttpMethod> converter = new StrongStringTypeConverter<DomainHttpMethod>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
