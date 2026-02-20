// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Localization.UnitTests;

public class LocaleTests
{
    [Theory]
    [InlineData("en-US", true)]
    [InlineData("de-DE", true)]
    [InlineData("zh-Hans-CN", true)]
    [InlineData("fr", true)]
    [InlineData("en", true)]
    [InlineData("invalid!", false)]
    [InlineData("", false)]
    [InlineData("123", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var locale = new Locale(value);
        Assert.Equal(expected, locale.IsValidFormat());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var locale = new Locale("en-US");
        Assert.Equal("en-US", locale.Value);
    }

    [Fact]
    public void ToCultureInfo_WithValidLocale_ReturnsCultureInfo()
    {
        var locale = new Locale("en-US");
        System.Globalization.CultureInfo? culture = locale.ToCultureInfo();
        Assert.NotNull(culture);
        Assert.Equal("en-US", culture.Name);
    }

    [Fact]
    public void ToCultureInfo_WithInvalidLocale_ReturnsNull()
    {
        var locale = new Locale("not-a-valid-culture-!!!!");
        System.Globalization.CultureInfo? culture = locale.ToCultureInfo();
        Assert.Null(culture);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var locale1 = new Locale("en-US");
        var locale2 = new Locale("en-US");
        Assert.Equal(locale1, locale2);
        Assert.True(locale1 == locale2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new StrongStringTypeConverter<Locale>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsLocale()
    {
        var converter = new StrongStringTypeConverter<Locale>();
        var result = converter.ConvertFrom("en-US") as Locale;
        Assert.NotNull(result);
        Assert.Equal("en-US", result.Value);
    }
}
