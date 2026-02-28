// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around an <see cref="int"/> (Int32) value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple integer values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific integer types like <c>Age</c>, <c>Quantity</c>, <c>Priority</c>, etc.
/// The compiler will prevent accidental mixing of different integer types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed integer types
/// public sealed class Age(int value) : StrongInt32&lt;Age&gt;(value) { }
/// public sealed class Quantity(int value) : StrongInt32&lt;Quantity&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public void CreateProduct(Quantity quantity, Age minAge)
/// {
///     // Cannot accidentally swap quantity and minAge!
/// }
///
/// // Create instances
/// var quantity = new Quantity(10);         // Fastest
/// var quantity = Quantity.From(10);        // For generic scenarios
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="int"/> value.</param>
public abstract partial class StrongInt32<TStrong>(int Value)
        : StrongOf<int, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongInt32,
          IParsable<TStrong>, ISpanParsable<TStrong>, IFormattable
    where TStrong : StrongInt32<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="int"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="int"/> value.</returns>
    /// <example>
    /// <code>
    /// var quantity = new Quantity(10);
    /// int rawValue = quantity.AsInt(); // 10
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int AsInt() => Value;

    /// <summary>
    /// Gets the underlying <see cref="int"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="int"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int AsInt32() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="int"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="int"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// int? nullableQty = GetOptionalQuantity();
    /// Quantity? quantity = Quantity.FromNullable(nullableQty);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(int? value)
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
    /// var qty1 = new Quantity(10);
    /// var qty2 = new Quantity(10);
    /// bool areEqual = qty1.Equals(qty2); // true
    /// </code>
    /// </example>
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
    /// Tries to parse an <see cref="int"/> from a character span and creates a strong type instance.
    /// </summary>
    /// <param name="content">The character span containing the number to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <param name="formatProvider">An optional format provider for culture-specific parsing.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (Quantity.TryParse("42", out Quantity? quantity))
    /// {
    ///     Console.WriteLine($"Parsed: {quantity}"); // 42
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (int.TryParse(content, formatProvider, out int value))
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
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <returns>The formatted string representation of the value.</returns>
    /// <example>
    /// <code>
    /// var quantity = new Quantity(1234);
    /// string formatted = quantity.ToString("N0", CultureInfo.InvariantCulture); // "1,234"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string? format, IFormatProvider? formatProvider)
        => Value.ToString(format, formatProvider);

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of an <see cref="int"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid integer.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(int.Parse(s, provider));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of an <see cref="int"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && int.TryParse(s, provider, out int value))
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
    /// <param name="s">The character span containing the number to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span is not a valid integer.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(int.Parse(s, provider));

    /// <summary>
    /// Tries to parse a character span to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The character span containing the number to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (int.TryParse(s, provider, out int value))
        {
            result = From(value);
            return true;
        }

        result = default;
        return false;
    }
}
