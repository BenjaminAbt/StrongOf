// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators
public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Returns a new instance with all leading and trailing whitespace removed.
    /// </summary>
    /// <returns>A new <typeparamref name="TStrong"/> with the trimmed value.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("  user@example.com  ");
    /// Email trimmed = email.Trim();
    /// Console.WriteLine(trimmed.Value); // "user@example.com"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong Trim()
        => From(Value.Trim());

    /// <summary>
    /// Returns a new instance with all leading whitespace removed.
    /// </summary>
    /// <returns>A new <typeparamref name="TStrong"/> with leading whitespace removed.</returns>
    /// <example>
    /// <code>
    /// var name = new FirstName("  John");
    /// FirstName trimmed = name.TrimStart();
    /// Console.WriteLine(trimmed.Value); // "John"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong TrimStart()
        => From(Value.TrimStart());

    /// <summary>
    /// Returns a new instance with all trailing whitespace removed.
    /// </summary>
    /// <returns>A new <typeparamref name="TStrong"/> with trailing whitespace removed.</returns>
    /// <example>
    /// <code>
    /// var name = new FirstName("John  ");
    /// FirstName trimmed = name.TrimEnd();
    /// Console.WriteLine(trimmed.Value); // "John"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong TrimEnd()
        => From(Value.TrimEnd());

    /// <summary>
    /// Determines whether this string equals the specified string using the given comparison type.
    /// </summary>
    /// <param name="value">The string to compare to this instance.</param>
    /// <param name="stringComparison">The comparison rules to use.</param>
    /// <returns><c>true</c> if the strings are equal according to the comparison; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("User@Example.com");
    /// bool equals = email.Equals("user@example.com", StringComparison.OrdinalIgnoreCase); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(string value, StringComparison stringComparison)
        => Value.Equals(value, stringComparison);

    /// <summary>
    /// Determines whether this string equals another strong string's value using the given comparison type.
    /// </summary>
    /// <typeparam name="T">The type of the strong string to compare.</typeparam>
    /// <param name="other">The strong string to compare to this instance.</param>
    /// <param name="stringComparison">The comparison rules to use.</param>
    /// <returns><c>true</c> if the underlying strings are equal; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email1 = new Email("User@Example.com");
    /// var email2 = new Email("user@example.com");
    /// bool equals = email1.Equals(email2, StringComparison.OrdinalIgnoreCase); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals<T>(T other, StringComparison stringComparison) where T : TStrong
        => Value.Equals(other.Value, stringComparison);

    /// <summary>
    /// Returns a new instance converted to lowercase using the specified culture.
    /// </summary>
    /// <param name="cultureInfo">The culture to use for the conversion, or <c>null</c> for the current culture.</param>
    /// <returns>A new <typeparamref name="TStrong"/> with the lowercase value.</returns>
    /// <example>
    /// <code>
    /// var name = new FirstName("JOHN");
    /// FirstName lower = name.ToLower(); // "john"
    /// FirstName lowerTurkish = name.ToLower(new CultureInfo("tr-TR")); // Turkish lowercase
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong ToLower(CultureInfo? cultureInfo = null)
        => From(Value.ToLower(cultureInfo));

    /// <summary>
    /// Returns a new instance converted to lowercase using invariant culture rules.
    /// </summary>
    /// <returns>A new <typeparamref name="TStrong"/> with the lowercase value.</returns>
    /// <remarks>
    /// Use this method for culture-independent lowercase conversions (e.g., identifiers, URLs).
    /// </remarks>
    /// <example>
    /// <code>
    /// var code = new ProductCode("ABC123");
    /// ProductCode lower = code.ToLowerInvariant(); // "abc123"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong ToLowerInvariant()
        => From(Value.ToLowerInvariant());

    /// <summary>
    /// Returns a new instance converted to uppercase using the specified culture.
    /// </summary>
    /// <param name="cultureInfo">The culture to use for the conversion, or <c>null</c> for the current culture.</param>
    /// <returns>A new <typeparamref name="TStrong"/> with the uppercase value.</returns>
    /// <example>
    /// <code>
    /// var name = new FirstName("john");
    /// FirstName upper = name.ToUpper(); // "JOHN"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong ToUpper(CultureInfo? cultureInfo = null)
        => From(Value.ToUpper(cultureInfo));

    /// <summary>
    /// Returns a new instance converted to uppercase using invariant culture rules.
    /// </summary>
    /// <returns>A new <typeparamref name="TStrong"/> with the uppercase value.</returns>
    /// <remarks>
    /// Use this method for culture-independent uppercase conversions.
    /// </remarks>
    /// <example>
    /// <code>
    /// var code = new ProductCode("abc123");
    /// ProductCode upper = code.ToUpperInvariant(); // "ABC123"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TStrong ToUpperInvariant()
        => From(Value.ToUpperInvariant());

    /// <summary>
    /// Returns the first character of the string.
    /// </summary>
    /// <returns>The first character of the underlying string.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the string is empty.</exception>
    /// <example>
    /// <code>
    /// var name = new FirstName("John");
    /// char first = name.FirstChar(); // 'J'
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public char FirstChar()
        => Value[0];

    /// <summary>
    /// Returns the first character converted to uppercase using invariant culture rules.
    /// </summary>
    /// <returns>The first character in uppercase.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if the string is empty.</exception>
    /// <example>
    /// <code>
    /// var name = new FirstName("john");
    /// char firstUpper = name.FirstCharUpperInvariant(); // 'J'
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public char FirstCharUpperInvariant()
        => char.ToUpperInvariant(Value[0]);

    /// <summary>
    /// Determines whether the string is null, empty, or consists only of whitespace characters.
    /// </summary>
    /// <returns><c>true</c> if the value is whitespace only; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var empty = new Email("");
    /// var whitespace = new Email("   ");
    /// var valid = new Email("user@example.com");
    ///
    /// empty.IsNullOrWhiteSpace();      // true
    /// whitespace.IsNullOrWhiteSpace(); // true
    /// valid.IsNullOrWhiteSpace();      // false
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsNullOrWhiteSpace()
        => string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Determines whether the string contains the specified character.
    /// </summary>
    /// <param name="value">The character to search for.</param>
    /// <returns><c>true</c> if the character is found; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("user@example.com");
    /// bool hasAt = email.Contains('@'); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Contains(char value)
        => Value.Contains(value);

    /// <summary>
    /// Determines whether the string contains the specified substring.
    /// </summary>
    /// <param name="value">The substring to search for.</param>
    /// <returns><c>true</c> if the substring is found; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("user@example.com");
    /// bool hasDomain = email.Contains("example"); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Contains(string value)
        => Value.Contains(value);

    /// <summary>
    /// Determines whether the string contains the specified substring using the specified comparison.
    /// </summary>
    /// <param name="value">The substring to search for.</param>
    /// <param name="comparison">The comparison rules to use.</param>
    /// <returns><c>true</c> if the substring is found; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("user@EXAMPLE.com");
    /// bool hasDomain = email.Contains("example", StringComparison.OrdinalIgnoreCase); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Contains(string value, StringComparison comparison)
        => Value.Contains(value, comparison);

    /// <summary>
    /// Determines whether the string starts with the specified character.
    /// </summary>
    /// <param name="value">The character to compare.</param>
    /// <returns><c>true</c> if the string starts with the character; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool StartsWith(char value)
        => Value.StartsWith(value);

    /// <summary>
    /// Determines whether the string starts with the specified substring using ordinal comparison.
    /// </summary>
    /// <param name="value">The substring to compare.</param>
    /// <returns><c>true</c> if the string starts with the substring; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool StartsWith(string value)
        => Value.StartsWith(value, StringComparison.Ordinal);

    /// <summary>
    /// Determines whether the string starts with the specified substring using the specified comparison.
    /// </summary>
    /// <param name="value">The substring to compare.</param>
    /// <param name="comparison">The comparison rules to use.</param>
    /// <returns><c>true</c> if the string starts with the substring; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool StartsWith(string value, StringComparison comparison)
        => Value.StartsWith(value, comparison);

    /// <summary>
    /// Determines whether the string ends with the specified character.
    /// </summary>
    /// <param name="value">The character to compare.</param>
    /// <returns><c>true</c> if the string ends with the character; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool EndsWith(char value)
        => Value.EndsWith(value);

    /// <summary>
    /// Determines whether the string ends with the specified substring using ordinal comparison.
    /// </summary>
    /// <param name="value">The substring to compare.</param>
    /// <returns><c>true</c> if the string ends with the substring; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool EndsWith(string value)
        => Value.EndsWith(value, StringComparison.Ordinal);

    /// <summary>
    /// Determines whether the string ends with the specified substring using the specified comparison.
    /// </summary>
    /// <param name="value">The substring to compare.</param>
    /// <param name="comparison">The comparison rules to use.</param>
    /// <returns><c>true</c> if the string ends with the substring; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool EndsWith(string value, StringComparison comparison)
        => Value.EndsWith(value, comparison);

    /// <summary>
    /// Determines whether the string contains characters not present in the allowed set.
    /// </summary>
    /// <param name="allowedChars">The set of allowed characters.</param>
    /// <param name="invalidCharacters">When this method returns <c>true</c>, contains the invalid characters found.</param>
    /// <returns><c>true</c> if invalid characters were found; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var code = new ProductCode("ABC-123");
    /// var allowed = new HashSet&lt;char&gt; { 'A', 'B', 'C', '1', '2', '3' };
    ///
    /// if (code.ContainsInvalidChars(allowed, out var invalid))
    /// {
    ///     Console.WriteLine($"Invalid chars: {string.Join(", ", invalid)}"); // "-"
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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
