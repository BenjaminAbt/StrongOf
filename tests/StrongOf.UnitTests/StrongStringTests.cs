using Xunit;

namespace StrongOf.UnitTests;

public class StrongStringTests
{
    private sealed class TestStringOf(string value) : StrongString<TestStringOf>(value) { }
    private sealed class OtherTestStringOf(string value) : StrongString<OtherTestStringOf>(value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestStringOf testOf = new("test");
        OtherTestStringOf otherTestOf2 = new("test");

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void FromTrimmed_ShouldReturnTrimmedValue()
    {
        TestStringOf strongString = TestStringOf.FromTrimmed("  test  ");
        Assert.Equal("test", strongString.Value);
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestStringOf strongString1 = new("a");
        TestStringOf strongString2 = new("b");
        Assert.True(strongString1.CompareTo(strongString2) < 0);
    }

    [Fact]
    public void Empty_ShouldReturnEmptyString()
    {
        TestStringOf strongString = TestStringOf.Empty();
        Assert.Equal("", strongString.Value);
    }

    [Fact]
    public void IsEmpty_ShouldReturnTrueForEmptyString()
    {
        TestStringOf strongString = TestStringOf.Empty();
        Assert.True(strongString.IsEmpty());
    }

    [Fact]
    public void IsNotEmpty_ShouldReturnTrueForNotEmptyString()
    {
        TestStringOf strongString = new("a");
        Assert.False(strongString.IsEmpty());
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestStringOf strongString = new("test");
        Assert.True(strongString == "test");
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestStringOf strongString = new("test");
        Assert.True(strongString != "other");
    }
}
