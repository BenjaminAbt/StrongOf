// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="decimal"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with monetary or precise decimal values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific decimal types like <c>Price</c>, <c>Amount</c>, <c>Percentage</c>, etc.
/// The compiler will prevent accidental mixing of different decimal types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed decimal types
/// public sealed class Price(decimal value) : StrongDecimal&lt;Price&gt;(value) { }
/// public sealed class Discount(decimal value) : StrongDecimal&lt;Discount&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public decimal CalculateTotal(Price price, Discount discount)
/// {
///     // Cannot accidentally swap price and discount!
///     return price.Value - discount.Value;
/// }
///
/// // Create instances
/// var price = new Price(99.99m);         // Fastest
/// var price = Price.From(99.99m);        // For generic scenarios
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="decimal"/> value.</param>
public abstract partial class StrongDecimal<TStrong>(decimal Value)
        : StrongOf<decimal, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongDecimal,
          IParsable<TStrong>, ISpanParsable<TStrong>, IFormattable,
          IUtf8SpanFormattable, IUtf8SpanParsable<TStrong>
    where TStrong : StrongDecimal<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="decimal"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="decimal"/> value.</returns>
    /// <example>
    /// <code>
    /// var price = new Price(99.99m);
    /// decimal rawValue = price.AsDecimal(); // 99.99
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal AsDecimal() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="decimal"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="decimal"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// decimal? nullablePrice = GetOptionalPrice();
    /// Price? price = Price.FromNullable(nullablePrice);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(decimal? value)
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
    /// var price1 = new Price(99.99m);
    /// var price2 = new Price(99.99m);
    /// bool areEqual = price1.Equals(price2); // true
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
    /// Tries to parse a <see cref="decimal"/> from a character span and creates a strong type instance.
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
    /// if (Price.TryParse("99.99", out Price? price))
    /// {
    ///     Console.WriteLine($"Price: {price}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
    /// Tries to parse a <see cref="decimal"/> from a character span using the provided format provider.
    /// </summary>
    /// <param name="content">The character span containing the number to parse.</param>
    /// <param name="provider">The format provider for culture-specific parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
    /// Tries to parse a <see cref="decimal"/> from a character span using the specified style and format provider.
    /// </summary>
    /// <param name="content">The character span containing the number to parse.</param>
    /// <param name="numberStyles">The number styles to allow during parsing.</param>
    /// <param name="provider">The format provider for culture-specific parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (Price.TryParse("$1,234.56", NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"), out Price? price))
    /// {
    ///     Console.WriteLine($"Price: {price.Value}"); // 1234.56
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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

    /// <summary>
    /// Returns the string representation of the underlying value using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <returns>The formatted string representation of the value.</returns>
    /// <example>
    /// <code>
    /// var price = new Price(1234.5m);
    /// string formatted = price.ToString("C2"); // "$1,234.50" (culture-dependent)
    /// string fixed2 = price.ToString("F2");    // "1234.50"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string format) => Value.ToString(format);

    /// <summary>
    /// Formats the value using the specified format and culture-specific format information.
    /// </summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="formatProvider">An object that provides culture-specific formatting information.</param>
    /// <returns>The formatted string representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString(format, formatProvider);

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="decimal"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid decimal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(decimal.Parse(s, provider));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="decimal"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && decimal.TryParse(s, provider, out decimal value))
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
    /// <exception cref="FormatException">The span is not a valid decimal.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(decimal.Parse(s, provider));

    // IUtf8SpanFormattable

    /// <summary>
    /// Tries to format the value into the provided UTF-8 byte span.
    /// </summary>
    /// <param name="utf8Destination">The destination span of UTF-8 bytes.</param>
    /// <param name="bytesWritten">The number of bytes written to the destination.</param>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <param name="provider">An object that provides culture-specific formatting information.</param>
    /// <returns><c>true</c> if the formatting was successful; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => ((IUtf8SpanFormattable)Value).TryFormat(utf8Destination, out bytesWritten, format, provider);

    // IUtf8SpanParsable<TStrong>

    /// <summary>
    /// Parses a UTF-8 byte span to create a new instance of the strong type.
    /// </summary>
    /// <param name="utf8Text">The UTF-8 encoded text to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        => From(decimal.Parse(utf8Text, provider));

    /// <summary>
    /// Tries to parse a UTF-8 byte span to create a new instance of the strong type.
    /// </summary>
    /// <param name="utf8Text">The UTF-8 encoded text to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (decimal.TryParse(utf8Text, provider, out decimal value))
        {
            result = From(value);
            return true;
        }

        result = default;
        return false;
    }
}
