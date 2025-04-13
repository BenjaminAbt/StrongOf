// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt32_As_Tests
{
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value) { }

    [Fact]
    public void AsInt_ReturnsCorrectResult()
    {
        // Arrange
        TestInt32Of strong = new(77);

        // Assert
        Assert.Equal(strong.Value, strong.AsInt());
    }

    [Fact]
    public void AsInt32_ReturnsCorrectResult()
    {
        // Arrange
        TestInt32Of strong = new(92);

        // Assert
        Assert.Equal(strong.Value, strong.AsInt32());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        // Arrange
        int value = 7;

        // Act
        TestInt32Of result = TestInt32Of.FromNullable(value);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        // Arrange
        int? value = null;

        // Act
        TestInt32Of? result = TestInt32Of.FromNullable(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromNullable_WithNotNull_ReturnsCorrectValue()
    {
        // Arrange
        int value = 7;

        // Act
        TestInt32Of result = TestInt32Of.FromNullable(value);

        // Assert
        Assert.Equal(value, result.Value);
    }
}
