using Xunit;

namespace StrongOf.Tests;

public class StrongCharTests
{
    private sealed class TestCharOf(char value) : StrongChar<TestCharOf>(value) { }
    private sealed class OtherTestCharOf(char value) : StrongChar<OtherTestCharOf>(value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestCharOf testOf = new('a');
        OtherTestCharOf otherTestOf2 = new('a');

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestCharOf testOf = new('a');

        Assert.True(testOf.Equals('a'));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestCharOf testOf = new('a');

        Assert.False(testOf.Equals('b'));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestCharOf first = new('a');
        TestCharOf second = new('b');

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidChar()
    {
        Assert.True(TestCharOf.TryParse("a", out TestCharOf? strong));
        Assert.Equal('a', strong.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidChar()
    {
        Assert.False(TestCharOf.TryParse("invalid", out TestCharOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestCharOf strongChar = new('a');
        Assert.True(strongChar == 'a');
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestCharOf strongChar = new('a');
        Assert.True(strongChar != 'b');
    }
}
