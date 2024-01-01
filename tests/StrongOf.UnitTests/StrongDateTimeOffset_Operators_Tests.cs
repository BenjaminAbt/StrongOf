using Xunit;

namespace StrongOf.Tests;

public class StrongDateTimeOffset_Operators_Tests
{
    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value) { }
    private sealed class OtherTestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<OtherTestDateTimeOffsetOf>(Value) { }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset == new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset != new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset != null);
        Assert.False(strongDateTimeOffset == null);
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-02T00:00:00+00:00")]
    [InlineData("2022-01-02T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    public void OperatorLessThan_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOffsetOf testStrongDateTimeOffset = new(DateTimeOffset.Parse(value));
        DateTimeOffset otherDateTimeOffset = DateTimeOffset.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTimeOffset.Value < otherDateTimeOffset), (testStrongDateTimeOffset < otherDateTimeOffset));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-02T00:00:00+00:00")]
    [InlineData("2022-01-02T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    public void OperatorGreaterThan_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOffsetOf testStrongDateTimeOffset = new(DateTimeOffset.Parse(value));
        DateTimeOffset otherDateTimeOffset = DateTimeOffset.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTimeOffset.Value > otherDateTimeOffset), (testStrongDateTimeOffset > otherDateTimeOffset));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-02T00:00:00+00:00")]
    [InlineData("2022-01-02T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    public void OperatorLessThanOrEqual_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOffsetOf testStrongDateTimeOffset = new(DateTimeOffset.Parse(value));
        DateTimeOffset otherDateTimeOffset = DateTimeOffset.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTimeOffset.Value <= otherDateTimeOffset), (testStrongDateTimeOffset <= otherDateTimeOffset));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-02T00:00:00+00:00")]
    [InlineData("2022-01-02T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    [InlineData("2022-01-01T00:00:00+00:00", "2022-01-01T00:00:00+00:00")]
    public void OperatorGreaterThanOrEqual_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOffsetOf testStrongDateTimeOffset = new(DateTimeOffset.Parse(value));
        DateTimeOffset otherDateTimeOffset = DateTimeOffset.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTimeOffset.Value >= otherDateTimeOffset), (testStrongDateTimeOffset >= otherDateTimeOffset));
    }
}
