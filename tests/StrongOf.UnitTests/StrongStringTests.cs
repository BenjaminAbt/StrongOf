// Copyright � Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongStringTests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }
    private sealed class OtherTestStringOf(string Value) : StrongString<OtherTestStringOf>(Value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestStringOf testOf = new("test");
        OtherTestStringOf otherTestOf2 = new("test");

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestStringOf testOf = new("test");
        Assert.True(testOf.Equals("test"));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestStringOf testOf = new("test");
        Assert.False(testOf.Equals("test2"));
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
    public void OperatorEquals_Null()
    {
        TestStringOf strongString = new("test");
        Assert.True(strongString != null);
        Assert.False(strongString == null);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestStringOf strongString = new("test");
        Assert.True(strongString != "other");
    }

    [Fact]
    public void TryParse_WithNonNullValue_ReturnsTrueAndValue()
    {
        bool result = TestStringOf.TryParse("hello", out TestStringOf? strong);
        Assert.True(result);
        Assert.NotNull(strong);
        Assert.Equal("hello", strong.Value);
    }

    [Fact]
    public void TryParse_WithNull_ReturnsFalseAndNull()
    {
        bool result = TestStringOf.TryParse(null, out TestStringOf? strong);
        Assert.False(result);
        Assert.Null(strong);
    }

    [Fact]
    public void CompareTo_WithNull_Returns1()
    {
        TestStringOf strongString = new("test");
        Assert.Equal(1, strongString.CompareTo(null));
    }

    [Fact]
    public void CompareTo_Object_WithDifferentType_ThrowsArgumentException()
    {
        TestStringOf strongString = new("test");
        Assert.Throws<ArgumentException>(() => strongString.CompareTo(new object()));
    }

    [Fact]
    public void FromTrimmed_WithAllWhitespace_ReturnsEmpty()
    {
        TestStringOf result = TestStringOf.FromTrimmed("   ");
        Assert.True(result.IsEmpty());
    }
}
