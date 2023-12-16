using Xunit;

namespace StrongOf.Tests;

public class StrongDecimalTests
{
    private sealed class TestDecimalOf(decimal value) : StrongDecimal<TestDecimalOf>(value) { }
    private sealed class OtherTestDecimalOf(decimal value) : StrongDecimal<OtherTestDecimalOf>(value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestDecimalOf testOf = new(1m);
        OtherTestDecimalOf otherTestOf2 = new(1m);

        Assert.False(testOf.Equals(otherTestOf2));
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
    public void TryParse_ShouldReturnTrueForValidDecimal()
    {
        Assert.True(TestDecimalOf.TryParse("1.23", System.Globalization.NumberStyles.Number, null, out TestDecimalOf? strong));
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
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestDecimalOf strongDecimal = new(1.23m);
        Assert.True(strongDecimal != 2.34m);
    }
}
