// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Media.UnitTests;

public class IsbnTests
{
    [Theory]
    [InlineData("9780306406157", true)]   // ISBN-13
    [InlineData("978-0-306-40615-7", true)]   // ISBN-13 with dashes
    [InlineData("0306406152", true)]       // ISBN-10
    [InlineData("0-306-40615-2", true)]    // ISBN-10 with dashes
    [InlineData("047096599X", true)]       // ISBN-10 with X check digit
    [InlineData("", false)]
    [InlineData("NOTANISBN", false)]
    [InlineData("12345", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var isbn = new Isbn(value);
        Assert.Equal(expected, isbn.IsValidFormat());
    }

    [Theory]
    [InlineData("9780306406157", true)]
    [InlineData("978-0-306-40615-7", true)]
    [InlineData("0306406152", false)]
    [InlineData("047096599X", false)]
    public void IsIsbn13_ReturnsExpected(string value, bool expected)
    {
        var isbn = new Isbn(value);
        Assert.Equal(expected, isbn.IsIsbn13());
    }

    [Theory]
    [InlineData("0306406152", true)]
    [InlineData("047096599X", true)]
    [InlineData("9780306406157", false)]
    [InlineData("978-0-306-40615-7", false)]
    public void IsIsbn10_ReturnsExpected(string value, bool expected)
    {
        var isbn = new Isbn(value);
        Assert.Equal(expected, isbn.IsIsbn10());
    }

    [Fact]
    public void ToNormalizedString_WithDashes_RemovesDashes()
    {
        var isbn = new Isbn("978-0-306-40615-7");
        Assert.Equal("9780306406157", isbn.ToNormalizedString());
    }

    [Fact]
    public void ToNormalizedString_WithSpaces_RemovesSpaces()
    {
        var isbn = new Isbn("978 0 306 40615 7");
        Assert.Equal("9780306406157", isbn.ToNormalizedString());
    }

    [Fact]
    public void ToNormalizedString_WithoutSeparators_ReturnsSameValue()
    {
        var isbn = new Isbn("9780306406157");
        Assert.Equal("9780306406157", isbn.ToNormalizedString());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var isbn = new Isbn("9780306406157");
        Assert.Equal("9780306406157", isbn.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var isbn1 = new Isbn("9780306406157");
        var isbn2 = new Isbn("9780306406157");
        Assert.Equal(isbn1, isbn2);
        Assert.True(isbn1 == isbn2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new IsbnTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsIsbn()
    {
        var converter = new IsbnTypeConverter();
        var result = converter.ConvertFrom("9780306406157") as Isbn;
        Assert.NotNull(result);
        Assert.Equal("9780306406157", result.Value);
    }
}
