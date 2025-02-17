using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed DateTime value.
/// </summary>
/// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
/// <remarks>"The DateTime type has some design flaws. Please migrate to DateTimeOffset."</remarks>
public abstract partial class StrongDateTime<TStrong>(DateTime Value)
        : StrongOf<DateTime, TStrong>(Value), IComparable, IStrongDateTime
    where TStrong : StrongDateTime<TStrong>
{
    /// <summary>
    /// Returns the value of the strong type as a DateTime.
    /// </summary>
    public DateTime AsDateTime() => Value;

    /// <summary>
    /// Returns the value of the strong type as a DateTimeOffset.
    /// </summary>
    public DateTimeOffset AsDateTimeOffset() => new(Value);

    /// <summary>
    /// Returns the value of the strong type as a DateOnly.
    /// </summary>
    public DateOnly AsDate() => DateOnly.FromDateTime(Value);

    /// <summary>
    /// Returns the value of the strong type as a TimeOnly.
    /// </summary>
    public TimeOnly AsTime() => TimeOnly.FromDateTime(Value);

    /// <summary>
    /// Creates a new instance of StrongDateTime from a nullable DateTime value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongDateTime if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(DateTime? value)
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
        => From(DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal));

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

        throw new ArgumentException($"Object is not a {typeof(TStrong)}", nameof(other));
    }

    /// <summary>
    /// Tries to parse a DateTime from a ReadOnlySpan of char using the ISO 8601 format and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTime to convert.</param>
    /// <param name="strong">When this method returns, contains the DateTime value equivalent to the DateTime contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParseIso8601(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (TryParse(content, CultureInfo.InvariantCulture, out strong))
        {
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to convert the specified string to a DateTime and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">The string to convert.</param>
    /// <param name="format">The required format of the date and time string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="dateTimeStyles">A bitwise combination of enumeration values that indicates the permitted format of content.</param>
    /// <param name="strong">When this method returns, contains the DateTime value equivalent to the date and time contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParseExact(ReadOnlySpan<char> content, string format, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParseExact(content, format, provider, dateTimeStyles, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse the specified content into a <typeparamref name="TStrong"/> object.
    /// </summary>
    /// <param name="content">The content to parse.</param>
    /// <param name="strong">When this method returns, contains the parsed value if the parsing succeeded, or <c>null</c> if the parsing failed. The parsing is case-sensitive.</param>
    /// <param name="formatProvider">An optional <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (DateTime.TryParse(content, formatProvider, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a DateTime from a ReadOnlySpan of char using the provided IFormatProvider and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTime to convert.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="strong">When this method returns, contains the DateTime value equivalent to the DateTime contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParse(content, provider, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a DateTime from a ReadOnlySpan of char using the provided IFormatProvider and DateTimeStyles and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a DateTime to convert.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="dateTimeStyles">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is None.</param>
    /// <param name="strong">When this method returns, contains the DateTime value equivalent to the DateTime contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParse(content, provider, dateTimeStyles, out DateTime value))
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
    /// Converts the value of the current StrongDateTime object to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <returns>A string representation of value of the current StrongDateTime object as specified by format.</returns>
    public string ToString(string format, IFormatProvider? provider = null) => Value.ToString(format, provider);

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <returns>A string that represents the current object.</returns>
    public string ToString(IFormatProvider? provider) => Value!.ToString(provider)!;

    /// <summary>
    /// Converts the value of the current StrongDateTime object to its equivalent string representation in ISO 8601 format.
    /// </summary>
    /// <returns>A string representation of value of the current StrongDateTime object in ISO 8601 format.</returns>
    public string ToStringIso8601() => Value.ToString("o", CultureInfo.InvariantCulture);
}
