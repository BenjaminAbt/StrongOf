// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="char"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with single character values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific character types like <c>GradeLevel</c>, <c>TypeIndicator</c>, etc.
/// The compiler will prevent accidental mixing of different character types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed char types
/// public sealed class GradeLevel(char value) : StrongChar&lt;GradeLevel&gt;(value) { }
/// public sealed class TypeIndicator(char value) : StrongChar&lt;TypeIndicator&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public string FormatEntry(GradeLevel grade, TypeIndicator type)
/// {
///     // Cannot accidentally swap grade and type!
///     return $"{grade.Value}-{type.Value}";
/// }
///
/// // Create instances
/// var grade = new GradeLevel('A');         // Fastest
/// var grade = GradeLevel.From('A');        // For generic scenarios
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="char"/> value.</param>
public abstract partial class StrongChar<TStrong>(char Value)
        : StrongOf<char, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongChar,
          IParsable<TStrong>, ISpanParsable<TStrong>
    where TStrong : StrongChar<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="char"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="char"/> value.</returns>
    /// <example>
    /// <code>
    /// var grade = new GradeLevel('A');
    /// char rawValue = grade.AsChar(); // 'A'
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public char AsChar() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="char"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="char"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// char? nullableGrade = GetOptionalGrade();
    /// GradeLevel? grade = GradeLevel.FromNullable(nullableGrade);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(char? value)
    {
        if (value.HasValue)
        {
            return From(value.Value);
        }

        return null;
    }

    /// <summary>
    /// Compares the current instance with another object and returns an integer indicating
    /// their relative position in the sort order.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A negative value if this instance precedes <paramref name="other"/>;
    /// zero if they are equal;
    /// a positive value if this instance follows <paramref name="other"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="other"/> is not of type <typeparamref name="TStrong"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
    /// Compares the current instance with another strong type of the same kind.
    /// </summary>
    /// <param name="other">The strong type to compare with this instance.</param>
    /// <returns>
    /// A negative value if this instance precedes <paramref name="other"/>;
    /// zero if they are equal;
    /// a positive value if this instance follows <paramref name="other"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int CompareTo(TStrong? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Determines whether the specified strong type instance is equal to the current instance.
    /// </summary>
    /// <param name="other">The strong type to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified instance is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var grade1 = new GradeLevel('A');
    /// var grade2 = new GradeLevel('A');
    /// bool areEqual = grade1.Equals(grade2); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(TStrong? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value == other.Value;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is TStrong other && Equals(other);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
        => Value.GetHashCode();

    /// <summary>
    /// Tries to parse a <see cref="char"/> from a string and creates a strong type instance.
    /// </summary>
    /// <param name="content">The string containing the character to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (GradeLevel.TryParse("A", out GradeLevel? grade))
    /// {
    ///     Console.WriteLine($"Grade: {grade}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(string content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (char.TryParse(content, out char value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// The string must contain exactly one character.
    /// </summary>
    /// <param name="s">A string containing exactly one character.</param>
    /// <param name="provider">An optional format provider (unused for <see cref="char"/> parsing).</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> does not contain exactly one character.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(char.Parse(s));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">A string containing exactly one character.</param>
    /// <param name="provider">An optional format provider (unused for <see cref="char"/> parsing).</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && char.TryParse(s, out char value))
        {
            result = From(value);
            return true;
        }

        result = default;
        return false;
    }

    // ISpanParsable<TStrong>

    /// <summary>
    /// Parses a character span to create a new instance of the strong type.
    /// The span must contain exactly one character.
    /// </summary>
    /// <param name="s">A character span containing exactly one character.</param>
    /// <param name="provider">An optional format provider (unused for <see cref="char"/> parsing).</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span does not contain exactly one character.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (s.Length != 1)
        {
            throw new FormatException($"The input span must contain exactly one character. Length was {s.Length}.");
        }

        return From(s[0]);
    }

    /// <summary>
    /// Tries to parse a character span to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">A character span containing exactly one character.</param>
    /// <param name="provider">An optional format provider (unused for <see cref="char"/> parsing).</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the span contains exactly one character; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s.Length == 1)
        {
            result = From(s[0]);
            return true;
        }

        result = default;
        return false;
    }
}
