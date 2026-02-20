// Copyright � Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt32_Operator_Tests
{
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value) { }
    private sealed class OtherTestInt32Of(int Value) : StrongInt32<OtherTestInt32Of>(Value) { }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestInt32Of strongInt = new(123);
        Assert.True(strongInt == 123);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestInt32Of strongInt = new(123);
        Assert.True(strongInt != 456);
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestInt32Of strongInt = new(123);
        Assert.NotNull(strongInt);
        Assert.NotNull(strongInt);
    }

    [Theory]
    [InlineData(5, 10)]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void OperatorLessThan_ReturnsCorrectResult(int value, int other)
    {
        // Arrange
        TestInt32Of strong = new(value);

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
        TestInt32Of strong = new(value);

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
        TestInt32Of strong = new(value);

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
        TestInt32Of strong = new(value);

        // Assert
        Assert.Equal((strong.Value >= other), (strong >= other));
    }

    // --- StrongType-vs-StrongType comparisons (regression tests for copy-paste bugs) ---

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(5, 10, false)]
    [InlineData(5, 5, false)]
    public void OperatorGreaterThan_WithStrongType_ReturnsCorrectResult(int left, int right, bool expected)
    {
        TestInt32Of strongLeft = new(left);
        TestInt32Of strongRight = new(right);
        Assert.Equal(expected, strongLeft > (object)strongRight);
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(10, 5, false)]
    [InlineData(5, 5, false)]
    public void OperatorLessThan_WithStrongType_ReturnsCorrectResult(int left, int right, bool expected)
    {
        TestInt32Of strongLeft = new(left);
        TestInt32Of strongRight = new(right);
        Assert.Equal(expected, strongLeft < (object)strongRight);
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(5, 10, false)]
    [InlineData(5, 5, true)]
    public void OperatorGreaterThanOrEqual_WithStrongType_ReturnsCorrectResult(int left, int right, bool expected)
    {
        TestInt32Of strongLeft = new(left);
        TestInt32Of strongRight = new(right);
        Assert.Equal(expected, strongLeft >= (object)strongRight);
    }

    [Theory]
    [InlineData(5, 10, true)]
    [InlineData(10, 5, false)]
    [InlineData(5, 5, true)]
    public void OperatorLessThanOrEqual_WithStrongType_ReturnsCorrectResult(int left, int right, bool expected)
    {
        TestInt32Of strongLeft = new(left);
        TestInt32Of strongRight = new(right);
        Assert.Equal(expected, strongLeft <= (object)strongRight);
    }

    [Fact]
    public void OperatorEquals_WithUInt_ReturnsTrue()
    {
        TestInt32Of strong = new(42);
        uint uintValue = 42u;
        Assert.True(strong == (object)uintValue);
    }

    [Fact]
    public void OperatorEquals_WithUInt_ReturnsFalse()
    {
        TestInt32Of strong = new(42);
        uint uintValue = 99u;
        Assert.False(strong == (object)uintValue);
    }
}
