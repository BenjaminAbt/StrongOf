// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="Url"/>.
/// </summary>
public class UrlTests
{
    [Fact]
    public void Constructor_WithValidUrl_SetsValue()
    {
        // Arrange
        const string urlValue = "https://example.com";

        // Act
        var url = new Url(urlValue);

        // Assert
        Assert.Equal(urlValue, url.Value);
    }

    [Theory]
    [InlineData("https://example.com", true)]
    [InlineData("http://example.com", true)]
    [InlineData("https://example.com/path", true)]
    [InlineData("https://example.com/path?query=value", true)]
    [InlineData("ftp://example.com", false)]
    [InlineData("not-a-url", false)]
    [InlineData("", false)]
    public void IsValidFormat_ReturnsExpectedResult(string urlValue, bool expected)
    {
        // Arrange
        var url = new Url(urlValue);

        // Act
        bool result = url.IsValidFormat();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("https://example.com", true)]
    [InlineData("http://example.com", true)]
    [InlineData("ftp://example.com", true)]
    [InlineData("file:///path/to/file", true)]
    [InlineData("not-a-url", false)]
    public void IsAbsoluteUri_ReturnsExpectedResult(string urlValue, bool expected)
    {
        // Arrange
        var url = new Url(urlValue);

        // Act
        bool result = url.IsAbsoluteUri();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("https://example.com", "example.com")]
    [InlineData("https://sub.example.com/path", "sub.example.com")]
    [InlineData("not-a-url", "")]
    public void GetHost_ReturnsExpectedResult(string urlValue, string expectedHost)
    {
        // Arrange
        var url = new Url(urlValue);

        // Act
        string host = url.GetHost();

        // Assert
        Assert.Equal(expectedHost, host);
    }

    [Theory]
    [InlineData("https://example.com", "https")]
    [InlineData("http://example.com", "http")]
    [InlineData("ftp://example.com", "ftp")]
    [InlineData("not-a-url", "")]
    public void GetScheme_ReturnsExpectedResult(string urlValue, string expectedScheme)
    {
        // Arrange
        var url = new Url(urlValue);

        // Act
        string scheme = url.GetScheme();

        // Assert
        Assert.Equal(expectedScheme, scheme);
    }

    [Theory]
    [InlineData("https://example.com/api/users", "/api/users")]
    [InlineData("https://example.com", "/")]
    [InlineData("not-a-url", "")]
    public void GetPath_ReturnsExpectedResult(string urlValue, string expectedPath)
    {
        // Arrange
        var url = new Url(urlValue);

        // Act
        string path = url.GetPath();

        // Assert
        Assert.Equal(expectedPath, path);
    }

    [Fact]
    public void ToUri_ValidUrl_ReturnsUri()
    {
        // Arrange
        var url = new Url("https://example.com");

        // Act
        Uri? uri = url.ToUri();

        // Assert
        Assert.NotNull(uri);
        Assert.Equal("https://example.com/", uri.ToString());
    }

    [Fact]
    public void ToUri_InvalidUrl_ReturnsNull()
    {
        // Arrange
        var url = new Url("not-a-url");

        // Act
        Uri? uri = url.ToUri();

        // Assert
        Assert.Null(uri);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        var url1 = new Url("https://example.com");
        var url2 = new Url("https://example.com");

        // Act & Assert
        Assert.Equal(url1, url2);
        Assert.True(url1 == url2);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        const string urlValue = "https://example.com";
        var url = new Url(urlValue);

        // Act
        string result = url.ToString();

        // Assert
        Assert.Equal(urlValue, result);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        var url1 = new Url("https://example.com");
        var url2 = new Url("https://example.com");

        // Act & Assert
        Assert.Equal(url1.GetHashCode(), url2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        const string urlValue = "https://example.com";

        // Act
        var url = Url.From(urlValue);

        // Assert
        Assert.Equal(urlValue, url.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new UrlTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsUrl()
    {
        // Arrange
        var converter = new UrlTypeConverter();
        const string urlValue = "https://example.com";

        // Act
        var result = converter.ConvertFrom(urlValue) as Url;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(urlValue, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new UrlTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
