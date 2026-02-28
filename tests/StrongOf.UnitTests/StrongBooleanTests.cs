// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongBooleanTests
{
    private sealed class TestBoolOf(bool value) : StrongBoolean<TestBoolOf>(value) { }
    private sealed class OtherTestBoolOf(bool value) : StrongBoolean<OtherTestBoolOf>(value) { }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        TestBoolOf a = new(true);
        TestBoolOf b = new(true);

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        TestBoolOf a = new(true);
        TestBoolOf b = new(false);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestBoolOf a = new(true);
        OtherTestBoolOf b = new(true);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        TestBoolOf a = new(true);

        Assert.False(a.Equals((TestBoolOf?)null));
    }

    [Fact]
    public void AsBool_ReturnsValue()
    {
        TestBoolOf a = new(true);

        Assert.True(a.AsBool());
    }

    [Fact]
    public void IsTrue_WhenTrue_ReturnsTrue()
    {
        TestBoolOf a = new(true);

        Assert.True(a.IsTrue());
    }

    [Fact]
    public void IsTrue_WhenFalse_ReturnsFalse()
    {
        TestBoolOf a = new(false);

        Assert.False(a.IsTrue());
    }

    [Fact]
    public void IsFalse_WhenFalse_ReturnsTrue()
    {
        TestBoolOf a = new(false);

        Assert.True(a.IsFalse());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsInstance()
    {
        TestBoolOf? result = TestBoolOf.FromNullable(true);

        Assert.NotNull(result);
        Assert.True(result.Value);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        TestBoolOf? result = TestBoolOf.FromNullable(null);

        Assert.Null(result);
    }

    [Fact]
    public void TryParse_WithTrue_ReturnsTrue()
    {
        Assert.True(TestBoolOf.TryParse("True", out TestBoolOf? strong));
        Assert.True(strong!.Value);
    }

    [Fact]
    public void TryParse_WithFalse_ReturnsTrue()
    {
        Assert.True(TestBoolOf.TryParse("False", out TestBoolOf? strong));
        Assert.False(strong!.Value);
    }

    [Fact]
    public void TryParse_WithInvalid_ReturnsFalse()
    {
        Assert.False(TestBoolOf.TryParse("invalid", out TestBoolOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void Parse_WithValidBool_ReturnsInstance()
    {
        TestBoolOf result = TestBoolOf.Parse("True", null);

        Assert.True(result.Value);
    }

    [Fact]
    public void Parse_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => TestBoolOf.Parse(null!, null));
    }

    [Fact]
    public void OperatorEquals_WithBool_ReturnsCorrectResult()
    {
        TestBoolOf strong = new(true);

        Assert.True(strong == true);
        Assert.False(strong == false);
    }

    [Fact]
    public void OperatorNotEquals_WithBool_ReturnsCorrectResult()
    {
        TestBoolOf strong = new(true);

        Assert.False(strong != true);
        Assert.True(strong != false);
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestBoolOf? strong = null;

        Assert.True(strong == null);
        Assert.False(strong != null);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHash()
    {
        TestBoolOf a = new(true);
        TestBoolOf b = new(true);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsValueString()
    {
        TestBoolOf strong = new(true);

        Assert.Equal("True", strong.ToString());
    }
}
