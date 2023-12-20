using System.Globalization;
using Xunit;

namespace StrongOf.Tests;

public class StrongDateTimeOffsetTests
{
    private sealed class TestDateTimeOffsetOf(DateTimeOffset value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(value) { }
    private sealed class OtherTestDateTimeOffsetOf(DateTimeOffset value) : StrongDateTimeOffset<OtherTestDateTimeOffsetOf>(value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestDateTimeOffsetOf testOf = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        OtherTestDateTimeOffsetOf otherTestOf2 = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestDateTimeOffsetOf first = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        TestDateTimeOffsetOf second = new(new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero));

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidDateTimeOffset()
    {
        Assert.True(TestDateTimeOffsetOf.TryParse("2000-01-01T00:00:00+00:00", out TestDateTimeOffsetOf? strong));
        Assert.Equal(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), strong.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidDateTimeOffset()
    {
        Assert.False(TestDateTimeOffsetOf.TryParse("invalid", out TestDateTimeOffsetOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset == new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset != new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestDateTimeOffsetOf strongDateTimeOffset = new(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(strongDateTimeOffset != null);
        Assert.False(strongDateTimeOffset == null);
    }

    [Fact]
    public void ToString_Iso8601()
    {
        TestDateTimeOffsetOf strong = TestDateTimeOffsetOf.FromIso8601("2023-12-17T14:24:22.6412808+00:00");
        DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact("2023-12-17T14:24:22.6412808+00:00", "o",
            CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal);

        Assert.Equal("2023-12-17T14:24:22.6412808+00:00", strong.ToStringIso8601());
        Assert.Equal(dateTimeOffset.ToString("o"), strong.ToStringIso8601());
    }
}
