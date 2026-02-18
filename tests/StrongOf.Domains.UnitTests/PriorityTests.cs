// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

public class PriorityTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var p = new Priority(3);
        Assert.Equal(3, p.Value);
    }

    [Fact]
    public void IsHigherThan_WhenValueIsGreater_ReturnsTrue()
    {
        var high = new Priority(10);
        var low = new Priority(1);
        Assert.True(high.IsHigherThan(low));
    }

    [Fact]
    public void IsHigherThan_WhenValueIsLower_ReturnsFalse()
    {
        var low = new Priority(1);
        var high = new Priority(10);
        Assert.False(low.IsHigherThan(high));
    }

    [Fact]
    public void IsHigherThan_WhenValuesAreEqual_ReturnsFalse()
    {
        var p1 = new Priority(5);
        var p2 = new Priority(5);
        Assert.False(p1.IsHigherThan(p2));
    }

    [Fact]
    public void IsLowerThan_WhenValueIsLower_ReturnsTrue()
    {
        var low = new Priority(1);
        var high = new Priority(10);
        Assert.True(low.IsLowerThan(high));
    }

    [Fact]
    public void IsLowerThan_WhenValueIsGreater_ReturnsFalse()
    {
        var high = new Priority(10);
        var low = new Priority(1);
        Assert.False(high.IsLowerThan(low));
    }

    [Fact]
    public void IsLowerThan_WhenValuesAreEqual_ReturnsFalse()
    {
        var p1 = new Priority(5);
        var p2 = new Priority(5);
        Assert.False(p1.IsLowerThan(p2));
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var p1 = new Priority(7);
        var p2 = new Priority(7);
        Assert.Equal(p1, p2);
        Assert.True(p1 == p2);
    }

    [Fact]
    public void CompareTo_SortsCorrectly()
    {
        var low = new Priority(1);
        var high = new Priority(10);
        Assert.True(low.CompareTo(high) < 0);
        Assert.True(high.CompareTo(low) > 0);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        var converter = new PriorityTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(int)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new PriorityTypeConverter();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsPriority()
    {
        var converter = new PriorityTypeConverter();
        var result = converter.ConvertFrom(5) as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPriority()
    {
        var converter = new PriorityTypeConverter();
        var result = converter.ConvertFrom("5") as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }
}
