// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public sealed class DomainContractTests
{
    [Fact]
    public void CaseInsensitiveDomainValuesHaveMatchingHashes()
    {
        CheckEquality(new CurrencyCode("USD"), new CurrencyCode("usd"));
        CheckEquality(new CountryCode("DE"), new CountryCode("de"));
        CheckEquality(new LanguageCode("DE"), new LanguageCode("de"));
        CheckEquality(new Locale("DE-DE"), new Locale("de-de"));
        CheckEquality(new FileExtension(".PNG"), new FileExtension(".png"));
        CheckEquality(new MimeType("IMAGE/PNG"), new MimeType("image/png"));
        CheckEquality(new HostName("EXAMPLE.COM"), new HostName("example.com"));
        CheckEquality(new MacAddress("AA:BB:CC:DD:EE:FF"), new MacAddress("aa:bb:cc:dd:ee:ff"));

        Assert.False(new CurrencyCode("USD").Equals(null));
        Assert.False(new CountryCode("DE").Equals(null));
        Assert.False(new LanguageCode("DE").Equals(null));
        Assert.False(new Locale("DE-DE").Equals(null));
        Assert.False(new FileExtension(".PNG").Equals(null));
        Assert.False(new MimeType("IMAGE/PNG").Equals(null));
        Assert.False(new HostName("EXAMPLE.COM").Equals(null));
        Assert.False(new MacAddress("AA:BB:CC:DD:EE:FF").Equals(null));
    }

    private static void CheckEquality(object first, object second)
    {
        Assert.True(first.Equals(second));
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.False(first.Equals(null));
    }

    [Fact]
    public void TryCreateRejectsNullForStringDomainTypes()
    {
        Assert.False(Sku.TryCreate(null, out _));
        Assert.False(CurrencyCode.TryCreate(null, out _));
        Assert.False(Iban.TryCreate(null, out _));
        Assert.False(Username.TryCreate(null, out _));
        Assert.False(LanguageCode.TryCreate(null, out _));
        Assert.False(Locale.TryCreate(null, out _));
        Assert.False(ColorHex.TryCreate(null, out _));
        Assert.False(FileExtension.TryCreate(null, out _));
        Assert.False(Isbn.TryCreate(null, out _));
        Assert.False(MimeType.TryCreate(null, out _));
        Assert.False(Slug.TryCreate(null, out _));
        Assert.False(EmailAddress.TryCreate(null, out _));
        Assert.False(HostName.TryCreate(null, out _));
        Assert.False(IpAddress.TryCreate(null, out _));
        Assert.False(MacAddress.TryCreate(null, out _));
        Assert.False(FirstName.TryCreate(null, out _));
        Assert.False(FullName.TryCreate(null, out _));
        Assert.False(LastName.TryCreate(null, out _));
        Assert.False(PhoneNumber.TryCreate(null, out _));
        Assert.False(City.TryCreate(null, out _));
        Assert.False(CountryCode.TryCreate(null, out _));
        Assert.False(CountryName.TryCreate(null, out _));
        Assert.False(HouseNumber.TryCreate(null, out _));
        Assert.False(Street.TryCreate(null, out _));
        Assert.False(ZipCode.TryCreate(null, out _));
        Assert.False(SemVer.TryCreate(null, out _));
    }

    [Fact]
    public void DomainEdgeCasesHandleEmptyAndMalformedInputs()
    {
        Assert.Equal(string.Empty, new Iban("D").GetCountryCode());
        Assert.False(new LanguageCode("").IsValidFormat());
        Assert.Equal("#FFF", new ColorHex("#fff").Normalize());
        Assert.Equal("#FFF", new ColorHex("fff").Normalize());
        Assert.False(new FileExtension("").IsValidFormat());
        Assert.Equal("png", new FileExtension(".png").WithoutDot());
        Assert.Equal("png", new FileExtension("png").WithoutDot());
        Assert.False(new Isbn("").IsIsbn13());
        Assert.False(new Isbn("").IsIsbn10());
        Assert.False(new MimeType("").IsValidFormat());
        Assert.Equal(string.Empty, new MimeType("noslash").GetTypePart());
        Assert.Equal(string.Empty, new MimeType("noslash").GetSubtypePart());
        Assert.False(new Port(-1).IsWellKnownPort());
        Assert.Equal(0, new HouseNumber("ABC").GetNumericPart());
        Assert.Equal(12, new HouseNumber("12").GetNumericPart());
        Assert.Equal(12, new HouseNumber("12A").GetNumericPart());
        Assert.False(new ZipCode("1234A-6789").IsValidUsFormat());
        Assert.False(new ZipCode("12345-678A").IsValidUsFormat());
        Assert.False(new SemVer("").IsValidFormat());
        Assert.False(new SemVer("...").TryGetMajor(out _));
    }

    [Fact]
    public void ValidationInterfaceUsesDomainRules()
    {
        IValidatable[] valid = [new Age(30), new MiddleName("Jane"), new Port(80),
            new Networking.HttpMethod("GET"), new FilePath("file.txt"), new TimeZoneId("UTC")];
        Assert.All(valid, value => Assert.True(value.IsValidFormat()));
        Assert.True(new Duration(TimeSpan.Zero).IsZero());
        Assert.True(new Duration(TimeSpan.FromSeconds(1)).IsPositive());
    }

    [Theory]
    [InlineData(typeof(Latitude), "-90", "90")]
    [InlineData(typeof(Longitude), "-180", "180")]
    [InlineData(typeof(Percentage), "0", "100")]
    [InlineData(typeof(HeightCm), "0", "300")]
    [InlineData(typeof(WeightKg), "0", "500")]
    [InlineData(typeof(TemperatureCelsius), "-273.15", "1000")]
    public void PublishedDecimalBounds(Type domain, string minimum, string maximum)
    {
        System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;
        Assert.Equal(decimal.Parse(minimum, culture), domain.GetField("MinValue")!.GetValue(null));
        Assert.Equal(decimal.Parse(maximum, culture), domain.GetField("MaxValue")!.GetValue(null));
    }
}
