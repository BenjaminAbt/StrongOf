// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

public class StrongTimeSpanTests
{
    private sealed class TestTimeSpanOf(TimeSpan value) : StrongTimeSpan<TestTimeSpanOf>(value) { }
    private sealed class OtherTestTimeSpanOf(TimeSpan value) : StrongTimeSpan<OtherTestTimeSpanOf>(value) { }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf a = new(ts);
        TestTimeSpanOf b = new(ts);

        Assert.True(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentValue_ReturnsFalse()
    {
        TestTimeSpanOf a = new(TimeSpan.FromHours(1));
        TestTimeSpanOf b = new(TimeSpan.FromHours(2));

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf a = new(ts);
        OtherTestTimeSpanOf b = new(ts);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestTimeSpanOf first = new(TimeSpan.FromMinutes(30));
        TestTimeSpanOf second = new(TimeSpan.FromMinutes(60));

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void AsTimeSpan_ReturnsValue()
    {
        TimeSpan ts = TimeSpan.FromHours(2);
        TestTimeSpanOf strong = new(ts);

        Assert.Equal(ts, strong.AsTimeSpan());
    }

    [Fact]
    public void TotalDays_ReturnsCorrectValue()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromDays(2.5));

        Assert.Equal(2.5, strong.TotalDays());
    }

    [Fact]
    public void TotalHours_ReturnsCorrectValue()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromHours(3));

        Assert.Equal(3.0, strong.TotalHours());
    }

    [Fact]
    public void TotalMinutes_ReturnsCorrectValue()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromMinutes(90));

        Assert.Equal(90.0, strong.TotalMinutes());
    }

    [Fact]
    public void TotalSeconds_ReturnsCorrectValue()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromSeconds(120));

        Assert.Equal(120.0, strong.TotalSeconds());
    }

    [Fact]
    public void TotalMilliseconds_ReturnsCorrectValue()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromMilliseconds(500));

        Assert.Equal(500.0, strong.TotalMilliseconds());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsInstance()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf? result = TestTimeSpanOf.FromNullable(ts);

        Assert.NotNull(result);
        Assert.Equal(ts, result.Value);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        TestTimeSpanOf? result = TestTimeSpanOf.FromNullable(null);

        Assert.Null(result);
    }

    [Fact]
    public void TryParse_WithValid_ReturnsTrue()
    {
        Assert.True(TestTimeSpanOf.TryParse("01:30:00", null, out TestTimeSpanOf? strong));
        Assert.Equal(TimeSpan.FromMinutes(90), strong!.Value);
    }

    [Fact]
    public void TryParse_WithInvalid_ReturnsFalse()
    {
        Assert.False(TestTimeSpanOf.TryParse("not-a-timespan", null, out TestTimeSpanOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void Parse_WithValid_ReturnsInstance()
    {
        TestTimeSpanOf result = TestTimeSpanOf.Parse("01:30:00", CultureInfo.InvariantCulture);

        Assert.Equal(TimeSpan.FromMinutes(90), result.Value);
    }

    [Fact]
    public void OperatorEquals_ReturnsTrue()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf a = new(ts);
        TestTimeSpanOf b = new(ts);

        Assert.True(a == b);
    }

    [Fact]
    public void OperatorNotEquals_ReturnsFalse()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf a = new(ts);
        TestTimeSpanOf b = new(ts);

        Assert.False(a != b);
    }

    [Fact]
    public void OperatorLessThan_ReturnsCorrectResult()
    {
        TestTimeSpanOf a = new(TimeSpan.FromMinutes(30));
        TestTimeSpanOf b = new(TimeSpan.FromMinutes(60));

        Assert.True(a < b);
        Assert.False(b < a);
    }

    [Fact]
    public void OperatorGreaterThan_ReturnsCorrectResult()
    {
        TestTimeSpanOf a = new(TimeSpan.FromMinutes(30));
        TestTimeSpanOf b = new(TimeSpan.FromMinutes(60));

        Assert.True(b > a);
        Assert.False(a > b);
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestTimeSpanOf? strong = null;

        Assert.True(strong == null);
        Assert.False(strong != null);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHash()
    {
        TimeSpan ts = TimeSpan.FromHours(1);
        TestTimeSpanOf a = new(ts);
        TestTimeSpanOf b = new(ts);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromHours(1.5));

        Assert.Equal("01:30:00", strong.ToString());
    }

    [Fact]
    public void ToString_WithFormat_ReturnsFormattedString()
    {
        TestTimeSpanOf strong = new(TimeSpan.FromHours(1.5));
        string result = strong.ToString("c", CultureInfo.InvariantCulture);

        Assert.Equal("01:30:00", result);
    }
}
