// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using Xunit;

namespace StrongOf.UnitTests;

public class StrongDateTimeOffset_Tests
{
    [Theory]
    [InlineData("2026-10-04T12:34:56+02:00", 120)]
    [InlineData("2026-10-04T12:34:56.1-05:30", -330)]
    [InlineData("2026-10-04T12:34:56.123+02:00", 120)]
    [InlineData("2026-10-04T12:34:56.1234567+02:00", 120)]
    [InlineData("2026-10-04T12:34:56Z", 0)]
    [InlineData("2026-10-04T12:34:56.1234567Z", 0)]
    public void Iso8601_PreservesOffsetAndTimestamp(string input, int offsetMinutes)
    {
        Assert.True(TestDateTimeOffsetOf.TryParseIso8601(input, out TestDateTimeOffsetOf? parsed));
        DateTimeOffset expected = DateTimeOffset.Parse(input, CultureInfo.InvariantCulture);
        Assert.Equal(TimeSpan.FromMinutes(offsetMinutes), parsed.Value.Offset);
        Assert.True(expected.EqualsExact(parsed.Value));
        Assert.True(expected.EqualsExact(TestDateTimeOffsetOf.FromIso8601(input).Value));
        Assert.True(expected.EqualsExact(TestDateTimeOffsetOf.FromIso8601(parsed.ToStringIso8601()).Value));
    }

    [Theory]
    [InlineData("not a date")]
    [InlineData("2026-02-30T12:34:56+02:00")]
    [InlineData("10/04/2026 12:34:56")]
    [InlineData("2026-10-04T12:34:56")]
    public void Iso8601_RejectsInvalidOrMissingOffset(string input)
    {
        Assert.False(TestDateTimeOffsetOf.TryParseIso8601(input, out _));
        Assert.Throws<FormatException>(() => TestDateTimeOffsetOf.FromIso8601(input));
    }

    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value), IStrongOf<DateTimeOffset, TestDateTimeOffsetOf>
    {
        public static TestDateTimeOffsetOf Create(DateTimeOffset value) => new(value);
    }

    private sealed class OtherTestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<OtherTestDateTimeOffsetOf>(Value), IStrongOf<DateTimeOffset, OtherTestDateTimeOffsetOf>
    {
        public static OtherTestDateTimeOffsetOf Create(DateTimeOffset value) => new(value);
    }

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
        Assert.Equal(0, first.CompareTo(first));
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
    public void ToString_Iso8601()
    {
        TestDateTimeOffsetOf strong = TestDateTimeOffsetOf.FromIso8601("2023-12-17T14:24:22.6412808+00:00");
        DateTimeOffset dateTimeOffset = DateTimeOffset.ParseExact("2023-12-17T14:24:22.6412808+00:00", "o",
            CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal);

        Assert.Equal("2023-12-17T14:24:22.6412808+00:00", strong.ToStringIso8601());
        Assert.Equal(dateTimeOffset.ToString("o"), strong.ToStringIso8601());
    }
}
