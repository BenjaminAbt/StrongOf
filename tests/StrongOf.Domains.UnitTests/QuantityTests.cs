// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Commerce.UnitTests;

public class QuantityTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var q = new Quantity(5);
        Assert.Equal(5, q.Value);
    }

    [Fact]
    public void Zero_ReturnsQuantityWithValueZero()
    {
        Assert.Equal(0, Quantity.Zero.Value);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsPositive_ReturnsExpected(int value, bool expected)
    {
        var q = new Quantity(value);
        Assert.Equal(expected, q.IsPositive());
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(-1, false)]
    public void IsZero_ReturnsExpected(int value, bool expected)
    {
        var q = new Quantity(value);
        Assert.Equal(expected, q.IsZero());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        var q1 = new Quantity(10);
        var q2 = new Quantity(10);
        Assert.Equal(q1, q2);
        Assert.True(q1 == q2);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt()
    {
        var converter = new StrongInt32TypeConverter<Quantity>();
        Assert.True(converter.CanConvertFrom(typeof(int)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new StrongInt32TypeConverter<Quantity>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromInt_ReturnsQuantity()
    {
        var converter = new StrongInt32TypeConverter<Quantity>();
        var result = converter.ConvertFrom(42) as Quantity;
        Assert.NotNull(result);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsQuantity()
    {
        var converter = new StrongInt32TypeConverter<Quantity>();
        var result = converter.ConvertFrom("42") as Quantity;
        Assert.NotNull(result);
        Assert.Equal(42, result.Value);
    }
}
