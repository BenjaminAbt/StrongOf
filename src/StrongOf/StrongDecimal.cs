﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed decimal.
/// </summary>
/// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
public abstract class StrongDecimal<TStrong>(decimal Value) : StrongOf<decimal, TStrong>(Value), IComparable, IStrongDecimal
    where TStrong : StrongDecimal<TStrong>
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
    /// Tries to parse a decimal from a ReadOnlySpan of char and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a decimal to convert.</param>
    /// <param name="strong">When this method returns, contains the decimal value equivalent to the decimal contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (decimal.TryParse(content, out decimal value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a decimal from a ReadOnlySpan of char using the provided IFormatProvider and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a decimal to convert.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="strong">When this method returns, contains the decimal value equivalent to the decimal contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (decimal.TryParse(content, provider, out decimal value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a decimal from a ReadOnlySpan of char using the provided NumberStyles and IFormatProvider and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing a decimal to convert.</param>
    /// <param name="numberStyles">A bitwise combination of enumeration values that defines how to interpret the parsed number. A typical value to specify is NumberStyles.Number.</param>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information.</param>
    /// <param name="strong">When this method returns, contains the decimal value equivalent to the decimal contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, NumberStyles numberStyles, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (decimal.TryParse(content, numberStyles, provider, out decimal value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // Operators
    /// <summary>
    /// Determines whether two specified instances of StrongDecimal are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="value">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same decimal; otherwise, false.</returns>
    public static bool operator ==(StrongDecimal<TStrong> strong, decimal value)
    {
        if (strong is null)
        {
            return true;
        }

        return strong.Value == value;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongDecimal are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same decimal; otherwise, false.</returns>
    public static bool operator !=(StrongDecimal<TStrong> strong, decimal other)
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
    /// Converts the value of the current StrongDecimal object to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <returns>A string representation of value of the current StrongDecimal object as specified by format.</returns>
    public string ToString(string format) => Value.ToString(format);
}
