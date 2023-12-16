using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed DateTimeOffset.
/// </summary>
/// <typeparam name="TStrong">The type of the strong DateTimeOffset.</typeparam>
public abstract class StrongDateTimeOffset<TStrong>(DateTimeOffset Value) : StrongOf<DateTimeOffset, TStrong>(Value), IComparable, IStrongDateTimeOffset
    where TStrong : StrongDateTimeOffset<TStrong>
{
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

    // Operators
    /// <summary>
    /// Determines whether two specified instances of StrongDateTimeOffset are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="value">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same DateTimeOffset; otherwise, false.</returns>
    public static bool operator ==(StrongDateTimeOffset<TStrong> strong, DateTimeOffset value)
    {
        if (strong is null)
        {
            return true;
        }

        return strong.Value == value;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongDateTime are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same DateTimeOffset; otherwise, false.</returns>
    public static bool operator !=(StrongDateTimeOffset<TStrong> strong, DateTimeOffset other)
    {
        return (strong == other) is false;
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
    /// <returns>A string representation of value of the current StrongDateTimeOffset object as specified by format.</returns>
    public string ToString(string format) => Value.ToString(format);
}
