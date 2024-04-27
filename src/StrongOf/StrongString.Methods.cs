using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators
public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Trims all leading and trailing white-space characters from the current string.
    /// </summary>
    /// <returns>A new strong string equivalent to the current string but without leading and trailing white-space characters.</returns>
    public TStrong Trim()
        => From(Value.Trim());

    /// <summary>
    /// Trims all leading white-space characters from the current string.
    /// </summary>
    /// <returns>A new strong string equivalent to the current string but without leading white-space characters.</returns>
    public TStrong TrimStart()
        => From(Value.TrimStart());

    /// <summary>
    /// Trims all trailing white-space characters from the current string.
    /// </summary>
    /// <returns>A new strong string equivalent to the current string but without trailing white-space characters.</returns>
    public TStrong TrimEnd()
        => From(Value.TrimEnd());

    /// <summary>
    /// Determines whether this string and a specified string have the same value.
    /// </summary>
    /// <param name="value">The string to compare to this instance.</param>
    /// <param name="stringComparison">One of the enumeration values that specifies the rules for the comparison.</param>
    /// <returns>true if the value of the value parameter is the same as this string; otherwise, false.</returns>
    public bool Equals(string value, StringComparison stringComparison)
        => Value.Equals(value, stringComparison);

    /// <summary>
    /// Determines whether this string and a specified StrongString object have the same value.
    /// </summary>
    /// <param name="other">The StrongString object to compare to this instance.</param>
    /// <param name="stringComparison">One of the enumeration values that specifies the rules for the comparison.</param>
    /// <returns>true if the value of the other parameter is the same as this string; otherwise, false.</returns>
    public bool Equals<T>(T other, StringComparison stringComparison) where T : TStrong
        => Value.Equals(other.Value, stringComparison);

    /// <summary>
    /// Returns a copy of this string converted to lowercase.
    /// </summary>
    /// <returns>A string in lowercase.</returns>
    public TStrong ToLower(CultureInfo? cultureInfo = null)
        => From(Value.ToLower(cultureInfo));

    /// <summary>
    /// Returns a copy of this string converted to lowercase, using the casing rules of the invariant culture.
    /// </summary>
    /// <returns>A string in lowercase.</returns>
    public TStrong ToLowerInvariant()
        => From(Value.ToLowerInvariant());

    /// <summary>
    /// Returns a copy of this string converted to uppercase.
    /// </summary>
    /// <returns>A string in uppercase.</returns>
    public TStrong ToUpper(CultureInfo? cultureInfo = null)
        => From(Value.ToUpper(cultureInfo));

    /// <summary>
    /// Returns a copy of this string converted to uppercase, using the casing rules of the invariant culture.
    /// </summary>
    /// <returns>A string in uppercase.</returns>
    public TStrong ToUpperInvariant()
        => From(Value.ToUpperInvariant());

    /// <summary>
    /// Returns the first character of the current string.
    /// </summary>
    /// <returns>The first character of the current string.</returns>
    public char FirstChar()
        => Value.First();

    /// <summary>
    /// Returns the first character of the current string converted to uppercase, using the casing rules of the invariant culture.
    /// </summary>
    /// <returns>The first character of the current string in uppercase.</returns>
    public char FirstCharUpperInvariant()
        => char.ToUpperInvariant(FirstChar());

    /// <summary>
    /// Determines whether the current string contains any characters not present in the provided set of allowed characters.
    /// </summary>
    /// <param name="allowedChars">A set of characters that are allowed in the string.</param>
    /// <param name="invalidCharacters">When this method returns, contains the collection of invalid characters, if any. This parameter is passed uninitialized.</param>
    /// <returns>true if the current string contains any characters not present in the allowedChars parameter; otherwise, false.</returns>
    public bool ContainsInvalidChars(ICollection<char> allowedChars, [NotNullWhen(true)] out ICollection<char>? invalidCharacters)
    {
        HashSet<char>? invalidChars = null;

        foreach (char c in Value)
        {
            if (!allowedChars.Contains(c))
            {
                invalidChars ??= [];
                invalidChars.Add(c);
            }
        }

        if (invalidChars is null)
        {
            invalidCharacters = null;
            return false;
        }

        invalidCharacters = invalidChars;
        return true;
    }
}
