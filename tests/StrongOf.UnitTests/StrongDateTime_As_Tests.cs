using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

public class StrongDateTime_As_Tests
{
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value) { }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        // Arrange
        DateTime value = DateTime.UtcNow;

        // Act
        TestDateTimeOf result = TestDateTimeOf.FromNullable(value);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        // Arrange
        DateTime? value = null;

        // Act
        TestDateTimeOf? result = TestDateTimeOf.FromNullable(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromNullable_WithNotNull_ReturnsCorrectValue()
    {
        // Arrange
        DateTime value = DateTime.UtcNow;

        // Act
        TestDateTimeOf result = TestDateTimeOf.FromNullable(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void AsDateTime_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOf strong = new(DateTime.UtcNow);

        // Assert
        Assert.Equal(strong.Value, strong.AsDateTime());
    }

    [Fact]
    public void AsDateTimeOffset_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOf strong = new(DateTime.UtcNow);

        // Assert
        Assert.Equal(new DateTimeOffset(strong.Value), strong.AsDateTimeOffset());
    }

    [Fact]
    public void AsDate_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOf strong = new(DateTime.UtcNow);

        // Assert
        Assert.Equal(DateOnly.FromDateTime(strong.Value), strong.AsDate());
    }

    [Fact]
    public void AsTime_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOf strong = new(DateTime.UtcNow);

        // Assert
        Assert.Equal(TimeOnly.FromDateTime(strong.Value), strong.AsTime());
    }

    [Fact]
    public void TryParseIso8601_WithValidInput_ReturnsTrueAndNonNull()
    {
        // Arrange
        ReadOnlySpan<char> content = "2022-01-02T00:00:00+00:00".AsSpan();
        TestDateTimeOf? expected = TestDateTimeOf.From(DateTime.Parse("2022-01-02T00:00:00+00:00", CultureInfo.InvariantCulture));

        // Act
        bool result = TestDateTimeOf.TryParseIso8601(content, out TestDateTimeOf? strong);

        // Assert
        Assert.True(result);
        Assert.NotNull(strong);
        Assert.Equal(expected, strong);
    }

    [Fact]
    public void TryParseIso8601_WithInvalidInput_ReturnsFalseAndNull()
    {
        // Arrange
        ReadOnlySpan<char> content = "invalid-date".AsSpan();

        // Act
        bool result = TestDateTimeOf.TryParseIso8601(content, out TestDateTimeOf? strong);

        // Assert
        Assert.False(result);
        Assert.Null(strong);
    }

    [Fact]
    public void TryParseExact_WithValidInput_ReturnsTrueAndNonNull()
    {
        // Arrange
        ReadOnlySpan<char> content = "2024-04-26T12:00:00".AsSpan();
        string format = "yyyy-MM-ddTHH:mm:ss";
        TestDateTimeOf? expected = TestDateTimeOf.From(DateTime.ParseExact("2024-04-26T12:00:00", format, CultureInfo.InvariantCulture));

        // Act
        bool result = TestDateTimeOf.TryParseExact(content, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out TestDateTimeOf? strong);

        // Assert
        Assert.True(result);
        Assert.NotNull(strong);
        Assert.Equal(expected, strong);
    }

    [Fact]
    public void TryParseExact_WithInvalidInput_ReturnsFalseAndNull()
    {
        // Arrange
        ReadOnlySpan<char> content = "invalid-date".AsSpan();
        string format = "yyyy-MM-ddTHH:mm:ss";

        // Act
        bool result = TestDateTimeOf.TryParseExact(content, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out TestDateTimeOf? strong);

        // Assert
        Assert.False(result);
        Assert.Null(strong);
    }
}
