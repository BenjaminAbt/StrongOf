// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public class SlugTests
{
    [Theory]
    [InlineData("my-slug", true)]
    [InlineData("hello-world", true)]
    [InlineData("abc", true)]
    [InlineData("abc123", true)]
    [InlineData("123", true)]
    [InlineData("UPPERCASE", false)]
    [InlineData("has space", false)]
    [InlineData("has_underscore", false)]
    [InlineData("-leading-dash", false)]
    [InlineData("trailing-dash-", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        Slug slug = new Slug(value);
        Assert.Equal(expected, slug.IsValidFormat());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        Slug slug = new Slug("my-page");
        Assert.Equal("my-page", slug.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Slug slug1 = new Slug("my-page");
        Slug slug2 = new Slug("my-page");
        Assert.Equal(slug1, slug2);
        Assert.True(slug1 == slug2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        Slug slug1 = new Slug("my-page");
        Slug slug2 = new Slug("other-page");
        Assert.NotEqual(slug1, slug2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        StrongStringTypeConverter<Slug> converter = new StrongStringTypeConverter<Slug>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsSlug()
    {
        StrongStringTypeConverter<Slug> converter = new StrongStringTypeConverter<Slug>();
        Slug? result = converter.ConvertFrom("my-page") as Slug;
        Assert.NotNull(result);
        Assert.Equal("my-page", result.Value);
    }
}
