using System.Globalization;
using Xunit;

namespace StrongOf.Tests;

public class StrongDateTime_Tests
{
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value) { }
    private sealed class OtherTestDateTimeOf(DateTime Value) : StrongDateTime<OtherTestDateTimeOf>(Value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestDateTimeOf testOf = new(new DateTime(2000, 1, 1, 0, 0, 0));
        OtherTestDateTimeOf otherTestOf2 = new(new DateTime(2000, 1, 1, 0, 0, 0));

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestDateTimeOf first = new(new DateTime(2000, 1, 1));
        TestDateTimeOf second = new(new DateTime(2001, 1, 1));

        Assert.True(first.CompareTo(second) < 0);
        Assert.True(second.CompareTo(first) > 0);
        Assert.True(first.CompareTo(first) == 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidDateTime()
    {
        Assert.True(TestDateTimeOf.TryParse("2000-01-01", out TestDateTimeOf? strong));
        Assert.Equal(new DateTime(2000, 1, 1), strong.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidDateTime()
    {
        Assert.False(TestDateTimeOf.TryParse("invalid", out TestDateTimeOf? strong));
        Assert.Null(strong);
    }

    [Fact]
    public void ToString_DelegatesCallToUnderlyingValue_WithDefaultProvider()
    {
        // Arrange
        TestDateTimeOf strong = new(new DateTime(2000, 1, 1));
        string expected = strong.Value.ToString();

        // Act
        string result = strong.ToString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_DelegatesCallToUnderlyingValue_WithCustomProvider()
    {
        // Arrange
        TestDateTimeOf strong = new(new DateTime(2000, 1, 1));
        string expected = strong.Value.ToString("o", CultureInfo.InvariantCulture);

        // Act
        string result = strong.ToString("o", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_Iso8601()
    {
        TestDateTimeOf strong = TestDateTimeOf.FromIso8601("2023-12-17T14:24:22.6412808+00:00");
        DateTime dateTime = DateTime.ParseExact("2023-12-17T14:24:22.6412808+00:00", "o",
            CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal);

        Assert.Equal("2023-12-17T14:24:22.6412808Z", strong.ToStringIso8601());
        Assert.Equal(dateTime.ToString("o"), strong.ToStringIso8601());
    }
}
