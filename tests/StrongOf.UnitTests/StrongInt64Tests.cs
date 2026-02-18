// Copyright � Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt64Tests
{
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value) { }
    private sealed class OtherTestInt64Of(long Value) : StrongInt64<OtherTestInt64Of>(Value) { }

    [Fact]
    public void NewFrom_ShouldBeTheSame()
    {
        TestInt64Of strongInt1 = new(1);
        TestInt64Of strongInt2 = TestInt64Of.From(1);
        Assert.Equal(strongInt1.Value, strongInt2.Value);
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestInt64Of testOf = new(1);
        OtherTestInt64Of otherTestOf2 = new(1);

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestInt64Of testOf = new(1L);
        Assert.True(testOf.Equals(1L));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestInt64Of testOf = new(1L);
        Assert.False(testOf.Equals(2L));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestInt64Of strongInt1 = new(1);
        TestInt64Of strongInt2 = new(2);
        Assert.True(strongInt1.CompareTo(strongInt2) < 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidInt()
    {
        bool isValid = TestInt64Of.TryParse("123", out TestInt64Of? strongInt);
        Assert.True(isValid);
        Assert.NotNull(strongInt);
        Assert.Equal(123, strongInt.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidInt()
    {
        bool isValid = TestInt64Of.TryParse("X", out TestInt64Of? strongInt);
        Assert.False(isValid);
        Assert.Null(strongInt);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualValues()
    {
        TestInt64Of strongInt1 = new(123);
        TestInt64Of strongInt2 = new(123);
        Assert.True(strongInt1.Equals(strongInt2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualValues()
    {
        TestInt64Of strongInt1 = new(123);
        TestInt64Of strongInt2 = new(123);
        Assert.Equal(strongInt1.GetHashCode(), strongInt2.GetHashCode());
    }

    [Fact]
    public void CompareTo_WithNull_Returns1()
    {
        TestInt64Of strong = new(5L);
        Assert.Equal(1, strong.CompareTo(null));
    }

    [Fact]
    public void CompareTo_WithDifferentType_ThrowsArgumentException()
    {
        TestInt64Of strong = new(5L);
        Assert.Throws<ArgumentException>(() => strong.CompareTo(new object()));
    }

    [Fact]
    public void ToString_ReturnsStringRepresentation()
    {
        TestInt64Of strong = new(42L);
        Assert.Equal("42", strong.ToString());
    }
}
