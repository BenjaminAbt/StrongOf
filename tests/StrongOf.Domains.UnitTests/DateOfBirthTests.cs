// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="DateOfBirth"/>.
/// </summary>
public class DateOfBirthTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        DateTime value = new DateTime(1990, 5, 12, 0, 0, 0, DateTimeKind.Utc);

        // Act
        DateOfBirth dob = new DateOfBirth(value);

        // Assert
        Assert.Equal(value, dob.Value);
    }

    [Fact]
    public void IsInPast_WithPastDate_ReturnsTrue()
    {
        DateOfBirth dob = new DateOfBirth(DateTime.UtcNow.AddDays(-1));
        Assert.True(dob.IsInPast());
    }

    [Fact]
    public void IsInFuture_WithFutureDate_ReturnsTrue()
    {
        DateOfBirth dob = new DateOfBirth(DateTime.UtcNow.AddDays(1));
        Assert.True(dob.IsInFuture());
    }

    [Fact]
    public void IsReasonableAge_WithOldDate_ReturnsFalse()
    {
        DateOfBirth dob = new DateOfBirth(DateTime.UtcNow.AddYears(-200));
        Assert.False(dob.IsReasonableAge());
    }

    [Fact]
    public void IsReasonableAge_WithRecentDate_ReturnsTrue()
    {
        DateOfBirth dob = new DateOfBirth(DateTime.UtcNow.AddYears(-30));
        Assert.True(dob.IsReasonableAge());
    }

    [Fact]
    public void TypeConverter_CanConvertFromDateTime()
    {
        StrongDateTimeTypeConverter<DateOfBirth> converter = new StrongDateTimeTypeConverter<DateOfBirth>();
        Assert.True(converter.CanConvertFrom(typeof(DateTime)));
    }

    [Fact]
    public void TypeConverter_ConvertFromDateTime_ReturnsInstance()
    {
        StrongDateTimeTypeConverter<DateOfBirth> converter = new StrongDateTimeTypeConverter<DateOfBirth>();
        DateTime value = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        DateOfBirth? result = converter.ConvertFrom(value) as DateOfBirth;

        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsInstance()
    {
        StrongDateTimeTypeConverter<DateOfBirth> converter = new StrongDateTimeTypeConverter<DateOfBirth>();
        DateOfBirth? result = converter.ConvertFrom(null, System.Globalization.CultureInfo.InvariantCulture, "2000-01-01") as DateOfBirth;

        Assert.NotNull(result);
        Assert.Equal(new DateTime(2000, 1, 1), result.Value.Date);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        StrongDateTimeTypeConverter<DateOfBirth> converter = new StrongDateTimeTypeConverter<DateOfBirth>();
        Assert.False(converter.CanConvertFrom(typeof(int)));
    }
}
