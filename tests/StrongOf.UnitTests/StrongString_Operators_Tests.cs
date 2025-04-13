// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongString_Operators_Tests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }

    [Fact]
    public void EqualityOperator_WithNonNullStrongStringAndNonNullStringOther_ReturnsCorrectResult()
    {
        // Arrange
        TestStringOf strong = new("hello");
        object other = "hello";

        // Act
        bool result = strong == other;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EqualityOperator_WithNonNullStrongStringAndNonNullStringOther_ReturnsIncorrectResult()
    {
        // Arrange
        TestStringOf strong = new("hello");
        object other = "world";

        // Act
        bool result = strong == other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_WithNullStrongStringAndNonNullOther_ReturnsFalse()
    {
        // Arrange
        TestStringOf? strong = null;
        object other = "world";

        // Act
        bool result = strong == other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_WithNonNullStrongStringAndNullOther_ReturnsFalse()
    {
        // Arrange
        TestStringOf strong = new("hello");
        object? other = null;

        // Act
        bool result = strong == other;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EqualityOperator_WithBothNull_ReturnsTrue()
    {
        // Arrange
        TestStringOf? strong = null;
        object? other = null;

        // Act
        bool result = strong == other;

        // Assert
        Assert.True(result);
    }
}
