using Xunit;

namespace StrongOf.UnitTests;

public class StrongDecimal_Operators_Tests
{
    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value) { }

    [Fact]
    public void LessThanOperator_WithNonNullStrongDecimalAndNonNullDecimalOther_ReturnsCorrectResult()
    {
        // Arrange
        TestDecimalOf strong = new(10.5m);
        object other = 15.7m;

        // Act
        bool result = strong < other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOperator_WithNullStrongDecimal_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object other = 15.7m;

        // Act
        bool result = strong < other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOperator_WithNonNullStrongDecimalAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf strong = new(10.5m);
        object? other = null;

        // Act
        bool result = strong < other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOperator_WithBothNull_ReturnsTrue()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object? other = null;

        // Act
        bool result = strong < other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNonNullStrongDecimalAndNonNullDecimalOther_ReturnsCorrectResult()
    {
        // Arrange
        TestDecimalOf strong = new(15.7m);
        object other = 10.5m;

        // Act
        bool result = strong > other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNullStrongDecimal_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object other = 15.7m;

        // Act
        bool result = strong > other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNonNullStrongDecimalAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf strong = new(15.7m);
        object? other = null;

        // Act
        bool result = strong > other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOperator_WithBothNull_ReturnsTrue()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object? other = null;

        // Act
        bool result = strong > other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNonNullStrongDecimalAndNonNullDecimalOther_ReturnsCorrectResult()
    {
        // Arrange
        TestDecimalOf strong = new(10.5m);
        object other = 15.7m;

        // Act
        bool result = strong <= other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNullStrongDecimal_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object other = 15.7m;

        // Act
        bool result = strong <= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNonNullStrongDecimalAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf strong = new(10.5m);
        object? other = null;

        // Act
        bool result = strong <= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithBothNull_ReturnsTrue()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object? other = null;

        // Act
        bool result = strong <= other;

        // Assert
        Assert.True(result);
    }
    [Fact]
    public void GreaterThanOrEqualOperator_WithNonNullStrongDecimalAndNonNullDecimalOther_ReturnsCorrectResult()
    {
        // Arrange
        TestDecimalOf strong = new(15.7m);
        object other = 10.5m;

        // Act
        bool result = strong >= other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithNullStrongDecimal_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object other = 15.7m;

        // Act
        bool result = strong >= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithNonNullStrongDecimalAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestDecimalOf strong = new(15.7m);
        object? other = null;

        // Act
        bool result = strong >= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithBothNull_ReturnsTrue()
    {
        // Arrange
        TestDecimalOf? strong = null;
        object? other = null;

        // Act
        bool result = strong >= other;

        // Assert
        Assert.True(result);
    }
}
