// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongGuid_Operators_Tests
{
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value) { }

    private static readonly Guid s_guidA = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid s_guidB = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");

    // == / !=

    [Fact]
    public void EqualityOperator_WithEqualValues_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong == (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void EqualityOperator_WithDifferentValues_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.False(strong == (object)new TestGuidOf(s_guidB));
    }

    [Fact]
    public void EqualityOperator_WithRawGuid_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong == (object)s_guidA);
    }

    [Fact]
    public void EqualityOperator_WithNullStrongAndNullOther_ReturnsTrue()
    {
        TestGuidOf? strong = null;
        Assert.True(strong == null);
    }

    [Fact]
    public void EqualityOperator_WithNullStrongAndNonNullOther_ReturnsFalse()
    {
        TestGuidOf? strong = null;
        Assert.False(strong == (object)s_guidA);
    }

    [Fact]
    public void InequalityOperator_WithDifferentValues_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong != (object)new TestGuidOf(s_guidB));
    }

    // >

    [Fact]
    public void GreaterThanOperator_WithGreaterValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidB);
        Assert.True(strong > (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void GreaterThanOperator_WithLesserValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.False(strong > (object)new TestGuidOf(s_guidB));
    }

    [Fact]
    public void GreaterThanOperator_WithEqualValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.False(strong > (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void GreaterThanOperator_WithNullStrongAndNullOther_ReturnsFalse()
    {
        // null > null must be false - null is not strictly greater than null
        TestGuidOf? strong = null;
        Assert.False(strong > null);
    }

    [Fact]
    public void GreaterThanOperator_WithNullStrong_ReturnsFalse()
    {
        TestGuidOf? strong = null;
        Assert.False(strong > (object)s_guidA);
    }

    [Fact]
    public void GreaterThanOperator_WithRawGuid_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidB);
        Assert.True(strong > (object)s_guidA);
    }

    // <

    [Fact]
    public void LessThanOperator_WithLesserValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong < (object)new TestGuidOf(s_guidB));
    }

    [Fact]
    public void LessThanOperator_WithGreaterValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidB);
        Assert.False(strong < (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void LessThanOperator_WithEqualValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.False(strong < (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void LessThanOperator_WithNullStrongAndNullOther_ReturnsFalse()
    {
        // null < null must be false - null is not strictly less than null
        TestGuidOf? strong = null;
        Assert.False(strong < null);
    }

    [Fact]
    public void LessThanOperator_WithNullStrong_ReturnsFalse()
    {
        TestGuidOf? strong = null;
        Assert.False(strong < (object)s_guidA);
    }

    // >=

    [Fact]
    public void GreaterThanOrEqualOperator_WithGreaterValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidB);
        Assert.True(strong >= (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithEqualValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong >= (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithLesserValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.False(strong >= (object)new TestGuidOf(s_guidB));
    }

    [Fact]
    public void GreaterThanOrEqualOperator_WithBothNull_ReturnsTrue()
    {
        // null >= null: treating null as equal, so >= holds
        TestGuidOf? strong = null;
        Assert.True(strong >= null);
    }

    // <=

    [Fact]
    public void LessThanOrEqualOperator_WithLesserValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong <= (object)new TestGuidOf(s_guidB));
    }

    [Fact]
    public void LessThanOrEqualOperator_WithEqualValue_ReturnsTrue()
    {
        TestGuidOf strong = new(s_guidA);
        Assert.True(strong <= (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void LessThanOrEqualOperator_WithGreaterValue_ReturnsFalse()
    {
        TestGuidOf strong = new(s_guidB);
        Assert.False(strong <= (object)new TestGuidOf(s_guidA));
    }

    [Fact]
    public void LessThanOrEqualOperator_WithBothNull_ReturnsTrue()
    {
        // null <= null: treating null as equal, so <= holds
        TestGuidOf? strong = null;
        Assert.True(strong <= null);
    }
}
