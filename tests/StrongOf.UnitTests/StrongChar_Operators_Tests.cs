using Xunit;

namespace StrongOf.UnitTests;

public class StrongChar_Operators_Tests
{
    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value) { }

    [Fact]
    public void EqualityOperator_WithNullStrongAndNullObject_ReturnsTrue()
    {
        // Arrange & Act
        bool result = (TestCharOf?)null == null;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_WithNonNullStrongAndNullObject_ReturnsFalse()
    {
        // Arrange
        TestCharOf strong = new('A');

        // Act
        bool result = strong == null;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_WithCharAndStrongChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf strong = new('A');
        char charValue = 'A';
        char differentCharValue = 'B';

        // Act
        bool result1 = strong == charValue;
        bool result2 = strong == new TestCharOf(differentCharValue);

        // Assert
        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void LessThanOperator_WithNonNullStrongCharAndNonNullOtherChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf strong = new('a');
        object other = 'b';

        // Act
        bool result = strong < other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOperator_WithNullStrongChar_ReturnsFalse()
    {
        // Arrange
        TestCharOf? strong = null;
        object? other = 'b';

        // Act
        bool result = strong < other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOperator_WithNonNullStrongCharAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestCharOf strong = new('a');
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
        TestCharOf? strong = null;
        object? other = null;

        // Act
        bool result = strong < other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNonNullStrongCharAndNonNullOtherChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf strong = new('b');
        object other = 'a';

        // Act
        bool result = strong > other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNullStrongChar_ReturnsFalse()
    {
        // Arrange
        TestCharOf? strong = null;
        object other = 'b';

        // Act
        bool result = strong > other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOperator_WithNonNullStrongCharAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestCharOf strong = new('b');
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
        TestCharOf? strong = null;
        object? other = null;

        // Act
        bool result = strong > other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNonNullStrongCharAndNonNullOtherChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf strong = new('a');
        object other = 'b';

        // Act
        bool result = strong <= other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNullStrongChar_ReturnsFalse()
    {
        // Arrange
        TestCharOf? strong = null;
        object other = 'b';

        // Act
        bool result = strong <= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void LessThanOrEqualOperator_WithNonNullStrongCharAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestCharOf strong = new('a');
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
        TestCharOf? strong = null;
        object? other = null;

        // Act
        bool result = strong <= other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithNonNullStrongCharAndNonNullOtherChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf? strong = new('b');
        object other = 'a';

        // Act
        bool result = strong >= other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithNullStrongChar_ReturnsFalse()
    {
        // Arrange
        TestCharOf? strong = null;
        object other = 'b';

        // Act
        bool result = strong >= other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithNonNullStrongCharAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestCharOf strong = new('b');
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
        TestCharOf? strong = null;
        object? other = null;

        // Act
        bool result = strong >= other;

        // Assert
        Assert.True(result);
    }
}
