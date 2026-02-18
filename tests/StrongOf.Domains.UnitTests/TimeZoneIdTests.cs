// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="TimeZoneId"/>.
/// </summary>
public class TimeZoneIdTests
{
    [Fact]
    public void Constructor_WithValue_SetsValue()
    {
        var id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.Equal(TimeZoneInfo.Utc.Id, id.Value);
    }

    [Fact]
    public void IsValidId_WithUtc_ReturnsTrue()
    {
        var id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.True(id.IsValidId());
    }

    [Fact]
    public void IsValidId_WithInvalid_ReturnsFalse()
    {
        var id = new TimeZoneId("Not/AZone");
        Assert.False(id.IsValidId());
    }

    [Fact]
    public void TryGetTimeZone_ReturnsExpected()
    {
        var id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.True(id.TryGetTimeZone(out TimeZoneInfo? tz));
        Assert.NotNull(tz);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        var converter = new TimeZoneIdTypeConverter();
        var result = converter.ConvertFrom(TimeZoneInfo.Utc.Id) as TimeZoneId;

        Assert.NotNull(result);
        Assert.Equal(TimeZoneInfo.Utc.Id, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        var converter = new TimeZoneIdTypeConverter();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
