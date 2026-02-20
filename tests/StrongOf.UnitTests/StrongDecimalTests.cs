// Copyright � Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

public class StrongDecimalTests
{
    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value) { }
    private sealed class OtherTestDecimalOf(decimal Value) : StrongDecimal<OtherTestDecimalOf>(Value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestDecimalOf testOf = new(1m);
        OtherTestDecimalOf otherTestOf2 = new(1m);

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestDecimalOf testOf = new(1m);
        Assert.True(testOf.Equals(1m));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestDecimalOf testOf = new(1m);
        Assert.False(testOf.Equals(2m));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestDecimalOf first = new(1m);
        TestDecimalOf second = new(2m);

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidDecimal_US()
    {
        Assert.True(TestDecimalOf.TryParse("1.23", NumberStyles.Number,
            CultureInfo.GetCultureInfo("en-US").NumberFormat, out TestDecimalOf? strong));

        Assert.Equal(1.23m, strong.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidDecimal_DE()
    {
        Assert.True(TestDecimalOf.TryParse("1.23", NumberStyles.Number,
            CultureInfo.GetCultureInfo("de_DE").NumberFormat, out TestDecimalOf? strong));

        Assert.Equal(123, strong.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidDecimal()
    {
        Assert.False(TestDecimalOf.TryParse("invalid", out TestDecimalOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestDecimalOf strongDecimal = new(1.23m);
        Assert.True(strongDecimal == 1.23m);
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestDecimalOf strongDecimal = new(1.23m);
        Assert.NotNull(strongDecimal);
        Assert.False(strongDecimal == null);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestDecimalOf strongDecimal = new(1.23m);
        Assert.True(strongDecimal != 2.34m);
    }

    [Fact]
    public void AsDecimal_ReturnsValue()
    {
        TestDecimalOf strong = new(3.14m);
        Assert.Equal(3.14m, strong.AsDecimal());
        Assert.Equal(strong.Value, strong.AsDecimal());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        decimal value = 1.5m;
        TestDecimalOf result = TestDecimalOf.FromNullable(value);
        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        decimal? value = null;
        TestDecimalOf? result = TestDecimalOf.FromNullable(value);
        Assert.Null(result);
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        TestDecimalOf strong = new(1234.5m);
        Assert.Equal("1234.50", strong.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
    }

    [Fact]
    public void CompareTo_WithNull_Returns1()
    {
        TestDecimalOf strong = new(1.0m);
        Assert.Equal(1, strong.CompareTo(null));
    }

    [Fact]
    public void CompareTo_WithDifferentType_ThrowsArgumentException()
    {
        TestDecimalOf strong = new(1.0m);
        Assert.Throws<ArgumentException>(() => strong.CompareTo(new object()));
    }
}
