// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

public class IbanTests
{
    [Theory]
    [InlineData("DE89370400440532013000", true)]
    [InlineData("GB29NWBK60161331926819", true)]
    [InlineData("DE89 3704 0044 0532 0130 00", true)]
    [InlineData("", false)]
    [InlineData("NOTANIBAN", false)]
    [InlineData("1234567890", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var iban = new Iban(value);
        Assert.Equal(expected, iban.IsValidFormat());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var iban = new Iban("DE89370400440532013000");
        Assert.Equal("DE89370400440532013000", iban.Value);
    }

    [Fact]
    public void GetCountryCode_ReturnsFirstTwoChars()
    {
        var iban = new Iban("DE89370400440532013000");
        Assert.Equal("DE", iban.GetCountryCode());
    }

    [Fact]
    public void GetCountryCode_GB_ReturnsGB()
    {
        var iban = new Iban("GB29NWBK60161331926819");
        Assert.Equal("GB", iban.GetCountryCode());
    }

    [Fact]
    public void ToFormattedString_WithoutSpaces_AddsSpacesEvery4Chars()
    {
        var iban = new Iban("DE89370400440532013000");
        string formatted = iban.ToFormattedString();
        Assert.Equal("DE89 3704 0044 0532 0130 00", formatted);
    }

    [Fact]
    public void ToFormattedString_AlreadyFormatted_NormalizesSpaces()
    {
        var iban = new Iban("DE89 3704 0044 0532 0130 00");
        string formatted = iban.ToFormattedString();
        Assert.Equal("DE89 3704 0044 0532 0130 00", formatted);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var iban1 = new Iban("DE89370400440532013000");
        var iban2 = new Iban("DE89370400440532013000");
        Assert.Equal(iban1, iban2);
        Assert.True(iban1 == iban2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new IbanTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsIban()
    {
        var converter = new IbanTypeConverter();
        var result = converter.ConvertFrom("DE89370400440532013000") as Iban;
        Assert.NotNull(result);
        Assert.Equal("DE89370400440532013000", result.Value);
    }
}
