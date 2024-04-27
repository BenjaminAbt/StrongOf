using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed decimal.
/// </summary>
/// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
public abstract partial class StrongDecimal<TStrong>(decimal Value)
        : StrongOf<decimal, TStrong>(Value), IComparable, IStrongDecimal
    where TStrong : StrongDecimal<TStrong>
{
    /// <summary>
    /// Returns the value of the strong type as a Decimal.
    /// </summary>
    public decimal AsDecimal() => Value;

    /// <summary>
    /// Creates a new instance of StrongDecimal from a nullable decimal value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongDecimal if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(decimal? value)
    {
        if (value.HasValue)
        {
            TStrong strong = From(value.Value);
            return strong;
        }

        return null;
    }

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
    /// Tries to parse the specified content into a <typeparamref name="TStrong"/> object.
    /// </summary>
    /// <param name="content">The content to parse.</param>
    /// <param name="strong">When this method returns, contains the parsed value if the parsing succeeded, or <c>null</c> if the parsing failed. The parsing is case-sensitive.</param>
    /// <param name="formatProvider">An optional <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (decimal.TryParse(content, formatProvider, out decimal value))
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
