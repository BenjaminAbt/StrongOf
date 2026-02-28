// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

public class StrongDoubleTests
{
    private sealed class TestDoubleOf(double value) : StrongDouble<TestDoubleOf>(value) { }
    private sealed class OtherTestDoubleOf(double value) : StrongDouble<OtherTestDoubleOf>(value) { }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        TestDoubleOf a = new(3.14);
        TestDoubleOf b = new(3.14);

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        TestDoubleOf a = new(3.14);
        TestDoubleOf b = new(2.71);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestDoubleOf a = new(3.14);
        OtherTestDoubleOf b = new(3.14);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestDoubleOf testOf = new(3.14);
        Assert.True(testOf.Equals(3.14));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestDoubleOf testOf = new(3.14);
        Assert.False(testOf.Equals(2.71));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestDoubleOf first = new(1.0);
        TestDoubleOf second = new(2.0);

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void TryParse_WithValidDouble_ReturnsTrue()
    {
        Assert.True(TestDoubleOf.TryParse("3.14", CultureInfo.InvariantCulture, out TestDoubleOf? strong));

        Assert.Equal(3.14, strong!.Value);
    }

    [Fact]
    public void TryParse_WithInvalid_ReturnsFalse()
    {
        Assert.False(TestDoubleOf.TryParse("not-a-number", CultureInfo.InvariantCulture, out TestDoubleOf? strong));

        Assert.Null(strong);
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsInstance()
    {
        TestDoubleOf? result = TestDoubleOf.FromNullable(3.14);

        Assert.NotNull(result);
        Assert.Equal(3.14, result.Value);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        TestDoubleOf? result = TestDoubleOf.FromNullable(null);

        Assert.Null(result);
    }

    [Fact]
    public void Parse_WithValidDouble_ReturnsInstance()
    {
        TestDoubleOf result = TestDoubleOf.Parse("3.14", CultureInfo.InvariantCulture);

        Assert.Equal(3.14, result.Value);
    }

    [Fact]
    public void OperatorEquals_WithDouble_ReturnsTrue()
    {
        TestDoubleOf strong = new(3.14);

        Assert.True(strong == 3.14);
    }

    [Fact]
    public void OperatorNotEquals_WithDouble_ReturnsFalse()
    {
        TestDoubleOf strong = new(3.14);

        Assert.False(strong != 3.14);
    }

    [Fact]
    public void OperatorLessThan_ReturnsCorrectResult()
    {
        TestDoubleOf a = new(1.0);
        TestDoubleOf b = new(2.0);

        Assert.True(a < b);
        Assert.False(b < a);
    }

    [Fact]
    public void OperatorGreaterThan_ReturnsCorrectResult()
    {
        TestDoubleOf a = new(1.0);
        TestDoubleOf b = new(2.0);

        Assert.True(b > a);
        Assert.False(a > b);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHash()
    {
        TestDoubleOf a = new(3.14);
        TestDoubleOf b = new(3.14);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void AsDouble_ReturnsValue()
    {
        TestDoubleOf strong = new(3.14);

        Assert.Equal(3.14, strong.AsDouble());
    }
}
