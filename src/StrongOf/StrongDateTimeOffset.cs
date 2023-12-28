using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed DateTimeOffset.
/// </summary>
/// <typeparam name="TStrong">The type of the strong DateTimeOffset.</typeparam>
public abstract partial class StrongDateTimeOffset<TStrong>(DateTimeOffset Value) : StrongOf<DateTimeOffset, TStrong>(Value), IComparable, IStrongDateTimeOffset
    where TStrong : StrongDateTimeOffset<TStrong>
{
    /// <summary>
    /// Creates a new instance of StrongDateTimeOffset from a nullable DateTimeOffset value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongDateTimeOffset if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(DateTimeOffset? value)
    {
        if (value.HasValue)
        {
            TStrong strong = From(value.Value);
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Creates a new instance of TStrong from an ISO 8601 string.
    /// </summary>
    /// <param name="value">The ISO 8601 string to convert.</param>
    /// <returns>A new instance of TStrong.</returns>
    public static TStrong FromIso8601(string value)
        => From(DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal));

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(object? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is TStrong otherStrong)
        {
            return Value.CompareTo(otherStrong.Value);
        }

        throw new ArgumentException($"Object is not a {typeof(TStrong)}");
    }

    /// <summary>
    /// Tries to parse a DateTimeOffset from a ReadOnlySpan of char using the ISO 8601 format and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTimeOffset to convert.</param>
    /// <param name="strong">When this method returns, contains the DateTimeOffset value equivalent to the DateTimeOffset contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParseIso8601(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (TryParseExact(content, "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out strong))
        {
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to convert the specified string representation of a date and time to its DateTimeOffset equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.
    /// </summary>
    /// <param name="content">A string containing a date and time to convert.</param>
    /// <param name="format">The required format of content.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information about content.</param>
    /// <param name="dateTimeStyles">A bitwise combination of enumeration values that indicates the permitted format of content.</param>
    /// <param name="strong">When this method returns, contains the DateTimeOffset value equivalent to the date and time contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParseExact(ReadOnlySpan<char> content, string format, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParseExact(content, format, provider, dateTimeStyles, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a DateTimeOffset from a ReadOnlySpan of char and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTimeOffset to convert.</param>
    /// <param name="strong">When this method returns, contains the DateTimeOffset value equivalent to the DateTimeOffset contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParse(content, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a DateTimeOffset from a ReadOnlySpan of char using the provided IFormatProvider and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTimeOffset to convert.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="strong">When this method returns, contains the DateTimeOffset value equivalent to the DateTimeOffset contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParse(content, provider, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a DateTimeOffset from a ReadOnlySpan of char using the provided IFormatProvider and DateTimeStyles and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTimeOffset to convert.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="dateTimeStyles">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is None.</param>
    /// <param name="strong">When this method returns, contains the DateTimeOffset value equivalent to the DateTimeOffset contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParse(content, provider, dateTimeStyles, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // Equals

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Converts the value of the current StrongDateTimeOffset object to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of the current StrongDateTimeOffset object as specified by format.</returns>
    public string ToString(string format, IFormatProvider? provider = null) => Value.ToString(format, provider);

    /// <summary>
    /// Converts the value of the current StrongDateTimeOffset object to its equivalent string representation using the ISO 8601 format.
    /// </summary>
    /// <returns>A string representation of value of the current StrongDateTimeOffset object as specified by the ISO 8601 format.</returns>
    public string ToStringIso8601() => Value.ToString("o", CultureInfo.InvariantCulture);
}
