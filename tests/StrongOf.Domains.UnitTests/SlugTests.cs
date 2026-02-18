// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

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
        var slug = new Slug(value);
        Assert.Equal(expected, slug.IsValidFormat());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var slug = new Slug("my-page");
        Assert.Equal("my-page", slug.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var slug1 = new Slug("my-page");
        var slug2 = new Slug("my-page");
        Assert.Equal(slug1, slug2);
        Assert.True(slug1 == slug2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        var slug1 = new Slug("my-page");
        var slug2 = new Slug("other-page");
        Assert.NotEqual(slug1, slug2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new SlugTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsSlug()
    {
        var converter = new SlugTypeConverter();
        var result = converter.ConvertFrom("my-page") as Slug;
        Assert.NotNull(result);
        Assert.Equal("my-page", result.Value);
    }
}
