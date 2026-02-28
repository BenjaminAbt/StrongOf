// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="TimeSpan"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple duration values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific duration types like <c>Duration</c>, <c>Timeout</c>, <c>RetryDelay</c>, etc.
/// The compiler will prevent accidental mixing of different duration types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed TimeSpan types
/// public sealed class Duration(TimeSpan value) : StrongTimeSpan&lt;Duration&gt;(value) { }
/// public sealed class Timeout(TimeSpan value) : StrongTimeSpan&lt;Timeout&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public void Configure(Duration maxDuration, Timeout requestTimeout)
/// {
///     // Cannot accidentally swap maxDuration and requestTimeout!
/// }
///
/// // Create instances
/// var duration = new Duration(TimeSpan.FromMinutes(30));         // Fastest
/// var duration = Duration.From(TimeSpan.FromMinutes(30));        // For generic scenarios
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="TimeSpan"/> value.</param>
public abstract partial class StrongTimeSpan<TStrong>(TimeSpan Value)
        : StrongOf<TimeSpan, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongTimeSpan,
          IParsable<TStrong>, ISpanParsable<TStrong>, IFormattable
    where TStrong : StrongTimeSpan<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="TimeSpan"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="TimeSpan"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TimeSpan AsTimeSpan() => Value;

    /// <summary>
    /// Gets the total number of days represented by this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double TotalDays() => Value.TotalDays;

    /// <summary>
    /// Gets the total number of hours represented by this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double TotalHours() => Value.TotalHours;

    /// <summary>
    /// Gets the total number of minutes represented by this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double TotalMinutes() => Value.TotalMinutes;

    /// <summary>
    /// Gets the total number of seconds represented by this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double TotalSeconds() => Value.TotalSeconds;

    /// <summary>
    /// Gets the total number of milliseconds represented by this instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public double TotalMilliseconds() => Value.TotalMilliseconds;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="TimeSpan"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="TimeSpan"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(TimeSpan? value)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(TStrong? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value.Equals(other.Value);
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
    /// Tries to parse a <see cref="TimeSpan"/> from a character span.
    /// </summary>
    /// <param name="content">The character span containing the TimeSpan to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <param name="formatProvider">An optional format provider for culture-specific parsing.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (TimeSpan.TryParse(content, formatProvider, out TimeSpan value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // IFormattable

    /// <summary>
    /// Formats the value using the specified format and culture-specific format information.
    /// </summary>
    /// <param name="format">A standard or custom TimeSpan format string.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <returns>The formatted string representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="TimeSpan"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid TimeSpan.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(TimeSpan.Parse(s, provider));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="TimeSpan"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && TimeSpan.TryParse(s, provider, out TimeSpan value))
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
    /// </summary>
    /// <param name="s">The character span containing the TimeSpan to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span is not a valid TimeSpan.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(TimeSpan.Parse(s, provider));

    /// <summary>
    /// Tries to parse a character span to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The character span containing the TimeSpan to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (TimeSpan.TryParse(s, provider, out TimeSpan value))
        {
            result = From(value);
            return true;
        }

        result = default;
        return false;
    }
}
