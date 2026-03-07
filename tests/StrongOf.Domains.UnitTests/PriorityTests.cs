// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

public class PriorityTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        Priority p = new Priority(3);
        Assert.Equal(3, p.Value);
    }

    // Note: lower numeric value = higher priority (1 = critical, 10 = low)

    [Fact]
    public void IsHigherThan_WhenNumericValueIsSmaller_ReturnsTrue()
    {
        Priority critical = new Priority(1);
        Priority low = new Priority(10);
        Assert.True(critical.IsHigherThan(low));
    }

    [Fact]
    public void IsHigherThan_WhenNumericValueIsGreater_ReturnsFalse()
    {
        Priority low = new Priority(10);
        Priority critical = new Priority(1);
        Assert.False(low.IsHigherThan(critical));
    }

    [Fact]
    public void IsHigherThan_WhenValuesAreEqual_ReturnsFalse()
    {
        Priority p1 = new Priority(5);
        Priority p2 = new Priority(5);
        Assert.False(p1.IsHigherThan(p2));
    }

    [Fact]
    public void IsLowerThan_WhenNumericValueIsGreater_ReturnsTrue()
    {
        Priority low = new Priority(10);
        Priority critical = new Priority(1);
        Assert.True(low.IsLowerThan(critical));
    }

    [Fact]
    public void IsLowerThan_WhenNumericValueIsSmaller_ReturnsFalse()
    {
        Priority critical = new Priority(1);
        Priority low = new Priority(10);
        Assert.False(critical.IsLowerThan(low));
    }

    [Fact]
    public void IsLowerThan_WhenValuesAreEqual_ReturnsFalse()
    {
        Priority p1 = new Priority(5);
        Priority p2 = new Priority(5);
        Assert.False(p1.IsLowerThan(p2));
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Priority p1 = new Priority(7);
        Priority p2 = new Priority(7);
        Assert.Equal(p1, p2);
        Assert.True(p1 == p2);
    }

    [Fact]
    public void CompareTo_SortsCorrectly()
    {
        Priority low = new Priority(1);
        Priority high = new Priority(10);
        Assert.True(low.CompareTo(high) < 0);
        Assert.True(high.CompareTo(low) > 0);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        StrongInt32TypeConverter<Priority> converter = new StrongInt32TypeConverter<Priority>();
        Assert.True(converter.CanConvertFrom(typeof(int)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        StrongInt32TypeConverter<Priority> converter = new StrongInt32TypeConverter<Priority>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsPriority()
    {
        StrongInt32TypeConverter<Priority> converter = new StrongInt32TypeConverter<Priority>();
        Priority? result = converter.ConvertFrom(5) as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsPriority()
    {
        StrongInt32TypeConverter<Priority> converter = new StrongInt32TypeConverter<Priority>();
        Priority? result = converter.ConvertFrom("5") as Priority;
        Assert.NotNull(result);
        Assert.Equal(5, result.Value);
    }
}
