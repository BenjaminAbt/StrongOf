// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Commerce.UnitTests;

public class SkuTests
{
    [Theory]
    [InlineData("ABC123", true)]
    [InlineData("sku-001", true)]
    [InlineData("my_sku", true)]
    [InlineData("X", true)]
    [InlineData("", false)]
    [InlineData("has space", false)]
    [InlineData("has@special", false)]
    public void IsValidFormat_ReturnsExpected(string value, bool expected)
    {
        var sku = new Sku(value);
        Assert.Equal(expected, sku.IsValidFormat());
    }

    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var sku = new Sku("PROD-001");
        Assert.Equal("PROD-001", sku.Value);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var sku1 = new Sku("PROD-001");
        var sku2 = new Sku("PROD-001");
        Assert.Equal(sku1, sku2);
        Assert.True(sku1 == sku2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        var sku1 = new Sku("PROD-001");
        var sku2 = new Sku("PROD-002");
        Assert.NotEqual(sku1, sku2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new SkuTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsSku()
    {
        var converter = new SkuTypeConverter();
        var result = converter.ConvertFrom("PROD-001") as Sku;
        Assert.NotNull(result);
        Assert.Equal("PROD-001", result.Value);
    }
}
