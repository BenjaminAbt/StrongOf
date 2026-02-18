// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Identity.UnitTests;

public class TokenTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var token = new Token("my-secret-token");
        Assert.Equal("my-secret-token", token.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyToken_ReturnsTrue()
    {
        var token = new Token("abc123");
        Assert.True(token.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyToken_ReturnsFalse()
    {
        var token = Token.Empty();
        Assert.False(token.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var token1 = new Token("abc123");
        var token2 = new Token("abc123");
        Assert.Equal(token1, token2);
        Assert.True(token1 == token2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        var token1 = new Token("abc123");
        var token2 = new Token("xyz789");
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new TokenTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsToken()
    {
        var converter = new TokenTypeConverter();
        var result = converter.ConvertFrom("my-token") as Token;
        Assert.NotNull(result);
        Assert.Equal("my-token", result.Value);
    }
}
