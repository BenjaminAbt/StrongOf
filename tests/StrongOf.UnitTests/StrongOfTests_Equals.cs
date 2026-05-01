// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongOfTests_Equals
{
    private sealed class TestOf(int Value) : StrongOf<int, TestOf>(Value), IStrongOf<int, TestOf>
    {
        public static TestOf Create(int value) => new(value);
    }

    private sealed class OtherTestOf(int Value) : StrongOf<int, OtherTestOf>(Value), IStrongOf<int, OtherTestOf>
    {
        public static OtherTestOf Create(int value) => new(value);
    }

    [Fact]
    public void Equals_WithSameReference_ReturnsTrue()
    {
        TestOf testOf = new(1);

        Assert.True(testOf.Equals(testOf));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestOf testOf = new(1);
        OtherTestOf otherTestOf2 = new(1);

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithDifferentStrongOf_ReturnsFalse()
    {
        TestOf testOf = new(1);
        object differentTypeObject = new();

        Assert.False(testOf.Equals(differentTypeObject));
    }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        TestOf testOf1 = new(1);
        TestOf testOf2 = new(1);

        Assert.True(testOf1.Equals(testOf2));
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        TestOf testOf1 = new(1);
        TestOf testOf2 = new(2);

        Assert.False(testOf1.Equals(testOf2));
    }

    [Fact]
    public void Equals_WithDifferentTypeButSameValue_ReturnsFalse()
    {
        TestOf testOf1 = new(1);
        OtherTestOf otherTestOf2 = new(1);

        Assert.False(testOf1.Equals(otherTestOf2));
    }
}
