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
        TimeZoneId id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.Equal(TimeZoneInfo.Utc.Id, id.Value);
    }

    [Fact]
    public void IsValidId_WithUtc_ReturnsTrue()
    {
        TimeZoneId id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.True(id.IsValidId());
    }

    [Fact]
    public void IsValidId_WithInvalid_ReturnsFalse()
    {
        TimeZoneId id = new TimeZoneId("Not/AZone");
        Assert.False(id.IsValidId());
    }

    [Fact]
    public void TryGetTimeZone_ReturnsExpected()
    {
        TimeZoneId id = new TimeZoneId(TimeZoneInfo.Utc.Id);
        Assert.True(id.TryGetTimeZone(out TimeZoneInfo? tz));
        Assert.NotNull(tz);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongStringTypeConverter<TimeZoneId> converter = new StrongStringTypeConverter<TimeZoneId>();
        TimeZoneId? result = converter.ConvertFrom(TimeZoneInfo.Utc.Id) as TimeZoneId;

        Assert.NotNull(result);
        Assert.Equal(TimeZoneInfo.Utc.Id, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongStringTypeConverter<TimeZoneId> converter = new StrongStringTypeConverter<TimeZoneId>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
