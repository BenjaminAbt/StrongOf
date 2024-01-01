using Xunit;

namespace StrongOf.Tests;

public class StrongDateTimeOffset_As_Tests
{
    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value) { }

    [Fact]
    public void AsDateTime_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOffsetOf strong = new(DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(strong.Value.DateTime, strong.AsDateTime());
    }

    [Fact]
    public void AsDateTimeOffset_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOffsetOf strong = new(DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(strong.Value, strong.AsDateTimeOffset());
    }

    [Fact]
    public void AsDate_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOffsetOf strong = new(DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(DateOnly.FromDateTime(strong.Value.Date), strong.AsDate());
    }

    [Fact]
    public void AsTime_ReturnsCorrectResult()
    {
        // Arrange
        TestDateTimeOffsetOf strong = new(DateTimeOffset.UtcNow);

        // Assert
        Assert.Equal(TimeOnly.FromDateTime(strong.Value.Date), strong.AsTime());
    }
}
