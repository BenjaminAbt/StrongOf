// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Commerce.UnitTests;

public class PriorityTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var p = new Priority(3);
        Assert.Equal(3, p.Value);
    }

    // Note: lower numeric value = higher priority (1 = critical, 10 = low)

    [Fact]
    public void IsHigherThan_WhenNumericValueIsSmaller_ReturnsTrue()
    {
        var critical = new Priority(1);
        var low = new Priority(10);
        Assert.True(critical.IsHigherThan(low));
    }

    [Fact]
    public void IsHigherThan_WhenNumericValueIsGreater_ReturnsFalse()
    {
        var low = new Priority(10);
        var critical = new Priority(1);
        Assert.False(low.IsHigherThan(critical));
    }

    [Fact]
    public void IsHigherThan_WhenValuesAreEqual_ReturnsFalse()
    {
        var p1 = new Priority(5);
        var p2 = new Priority(5);
        Assert.False(p1.IsHigherThan(p2));
    }

    [Fact]
    public void IsLowerThan_WhenNumericValueIsGreater_ReturnsTrue()
    {
        var low = new Priority(10);
        var critical = new Priority(1);
        Assert.True(low.IsLowerThan(critical));
    }

    [Fact]
    public void IsLowerThan_WhenNumericValueIsSmaller_ReturnsFalse()
    {
        var critical = new Priority(1);
        var low = new Priority(10);
        Assert.False(critical.IsLowerThan(low));
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
        var converter = new StrongInt32TypeConverter<Priority>();
        Assert.True(converter.CanConvertFrom(typeof(int)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new StrongInt32TypeConverter<Priority>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsPriority()
    {
        var converter = new StrongInt32TypeConverter<Priority>();
        var result = converter.ConvertFrom(5) as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPriority()
    {
        var converter = new StrongInt32TypeConverter<Priority>();
        var result = converter.ConvertFrom("5") as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }
}
