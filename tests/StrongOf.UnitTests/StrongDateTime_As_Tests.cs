using Xunit;

namespace StrongOf.Tests;

public class StrongDateTime_As_Tests
{
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value) { }

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
}
