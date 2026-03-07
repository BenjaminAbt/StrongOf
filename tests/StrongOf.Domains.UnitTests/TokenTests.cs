// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public class TokenTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        Token token = new Token("my-secret-token");
        Assert.Equal("my-secret-token", token.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyToken_ReturnsTrue()
    {
        Token token = new Token("abc123");
        Assert.True(token.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyToken_ReturnsFalse()
    {
        Token token = Token.Empty();
        Assert.False(token.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Token token1 = new Token("abc123");
        Token token2 = new Token("abc123");
        Assert.Equal(token1, token2);
        Assert.True(token1 == token2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        Token token1 = new Token("abc123");
        Token token2 = new Token("xyz789");
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        StrongStringTypeConverter<Token> converter = new StrongStringTypeConverter<Token>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsToken()
    {
        StrongStringTypeConverter<Token> converter = new StrongStringTypeConverter<Token>();
        Token? result = converter.ConvertFrom("my-token") as Token;
        Assert.NotNull(result);
        Assert.Equal("my-token", result.Value);
    }
}
