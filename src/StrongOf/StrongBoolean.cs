// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="bool"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple boolean values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific boolean types like <c>IsActive</c>, <c>IsVerified</c>, <c>IsDeleted</c>, etc.
/// The compiler will prevent accidental mixing of different boolean types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed boolean types
/// public sealed class IsActive(bool value) : StrongBoolean&lt;IsActive&gt;(value) { }
/// public sealed class IsVerified(bool value) : StrongBoolean&lt;IsVerified&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public void UpdateUser(IsActive isActive, IsVerified isVerified)
/// {
///     // Cannot accidentally swap isActive and isVerified!
/// }
///
/// // Create instances
/// var isActive = new IsActive(true);          // Fastest
/// var isActive = IsActive.From(true);         // For generic scenarios
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="bool"/> value.</param>
public abstract partial class StrongBoolean<TStrong>(bool Value)
        : StrongOf<bool, TStrong>(Value), IEquatable<TStrong>, IStrongBoolean,
          IParsable<TStrong>, ISpanParsable<TStrong>
    where TStrong : StrongBoolean<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="bool"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="bool"/> value.</returns>
    /// <example>
    /// <code>
    /// var isActive = new IsActive(true);
    /// bool rawValue = isActive.AsBool(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool AsBool() => Value;

    /// <summary>
    /// Determines whether the underlying value is <c>true</c>.
    /// </summary>
    /// <returns><c>true</c> if the underlying value is <c>true</c>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsTrue() => Value;

    /// <summary>
    /// Determines whether the underlying value is <c>false</c>.
    /// </summary>
    /// <returns><c>true</c> if the underlying value is <c>false</c>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsFalse() => !Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="bool"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="bool"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(bool? value)
    {
        if (value.HasValue)
        {
            return From(value.Value);
        }

        return null;
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
    /// Tries to parse a <see cref="bool"/> from a character span and creates a strong type instance.
    /// </summary>
    /// <param name="content">The character span containing the value to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (bool.TryParse(content, out bool value))
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
    /// </summary>
    /// <param name="s">The string representation of a <see cref="bool"/>.</param>
    /// <param name="provider">An optional format provider (unused for boolean parsing).</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid boolean.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(bool.Parse(s));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="bool"/>.</param>
    /// <param name="provider">An optional format provider (unused for boolean parsing).</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && bool.TryParse(s, out bool value))
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
    /// <param name="s">The character span containing the boolean to parse.</param>
    /// <param name="provider">An optional format provider (unused for boolean parsing).</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span is not a valid boolean.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(bool.Parse(s));

    /// <summary>
    /// Tries to parse a character span to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The character span containing the boolean to parse.</param>
    /// <param name="provider">An optional format provider (unused for boolean parsing).</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (bool.TryParse(s, out bool value))
        {
            result = From(value);
            return true;
        }

        result = default;
        return false;
    }
}
