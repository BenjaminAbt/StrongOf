using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt64_Operator_Tests
{
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value) { }
    private sealed class OtherTestInt64Of(long Value) : StrongInt64<OtherTestInt64Of>(Value) { }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestInt64Of strongInt = new(123);
        Assert.True(strongInt == 123);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestInt64Of strongInt = new(123);
        Assert.True(strongInt != 456);
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestInt64Of strongInt = new(123);
        Assert.True(strongInt != null);
        Assert.False(strongInt == null);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void OperatorLessThan_ReturnsCorrectResult(int value, int other)
    {
        // Arrange
        TestInt64Of strong = new(value);

        // Assert
        Assert.Equal((strong.Value < other), (strong < other));
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void OperatorGreaterThan_ReturnsCorrectResult(int value, int other)
    {
        // Arrange
        TestInt64Of strong = new(value);

        // Assert
        Assert.Equal((strong.Value > other), (strong > other));
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void OperatorLessThanOrEqual_ReturnsCorrectResult(int value, int other)
    {
        // Arrange
        TestInt64Of strong = new(value);

        // Assert
        Assert.Equal((strong.Value <= other), (strong <= other));
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void OperatorGreaterThanOrEqual_ReturnsCorrectResult(int value, int other)
    {
        // Arrange
        TestInt64Of strong = new(value);

        // Assert
        Assert.Equal((strong.Value >= other), (strong >= other));
    }
}
