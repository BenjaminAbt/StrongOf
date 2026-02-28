// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="DateTime"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with date and time values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific DateTime types like <c>CreatedAt</c>, <c>ExpiresAt</c>, <c>BirthDate</c>, etc.
/// The compiler will prevent accidental mixing of different DateTime types.
/// </para>
/// <para>
/// <b>Design Note:</b> The <see cref="DateTime"/> type has some design limitations.
/// Consider using <see cref="StrongDateTimeOffset{TStrong}"/> for new code that requires timezone awareness.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed DateTime types
/// public sealed class CreatedAt(DateTime value) : StrongDateTime&lt;CreatedAt&gt;(value) { }
/// public sealed class ExpiresAt(DateTime value) : StrongDateTime&lt;ExpiresAt&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public bool IsExpired(CreatedAt createdAt, ExpiresAt expiresAt)
/// {
///     // Cannot accidentally swap createdAt and expiresAt!
///     return DateTime.Now > expiresAt.Value;
/// }
///
/// // Create instances
/// var createdAt = new CreatedAt(DateTime.UtcNow);         // Fastest
/// var createdAt = CreatedAt.From(DateTime.UtcNow);        // For generic scenarios
/// var createdAt = CreatedAt.FromIso8601("2024-01-15T10:30:00Z"); // From ISO 8601 string
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="DateTime"/> value.</param>
public abstract partial class StrongDateTime<TStrong>(DateTime Value)
        : StrongOf<DateTime, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongDateTime,
          IParsable<TStrong>, ISpanParsable<TStrong>, IFormattable
    where TStrong : StrongDateTime<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="DateTime"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="DateTime"/> value.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// DateTime rawValue = createdAt.AsDateTime();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateTime AsDateTime() => Value;

    /// <summary>
    /// Gets the value as a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <returns>A <see cref="DateTimeOffset"/> representing the same point in time.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// DateTimeOffset offset = createdAt.AsDateTimeOffset();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateTimeOffset AsDateTimeOffset() => new(Value);

    /// <summary>
    /// Gets the date portion of the value as a <see cref="DateOnly"/>.
    /// </summary>
    /// <returns>A <see cref="DateOnly"/> representing the date portion.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// DateOnly date = createdAt.AsDate();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateOnly AsDate() => DateOnly.FromDateTime(Value);

    /// <summary>
    /// Gets the time portion of the value as a <see cref="TimeOnly"/>.
    /// </summary>
    /// <returns>A <see cref="TimeOnly"/> representing the time portion.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// TimeOnly time = createdAt.AsTime();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TimeOnly AsTime() => TimeOnly.FromDateTime(Value);

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="DateTime"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// DateTime? nullableDate = GetOptionalDate();
    /// CreatedAt? createdAt = CreatedAt.FromNullable(nullableDate);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(DateTime? value)
    {
        if (value.HasValue)
        {
            return From(value.Value);
        }

        return null;
    }

    /// <summary>
    /// Creates a new instance from an ISO 8601 formatted string.
    /// </summary>
    /// <param name="value">The ISO 8601 string to convert (e.g., "2024-01-15T10:30:00Z").</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The string is not in a valid ISO 8601 format.</exception>
    /// <example>
    /// <code>
    /// var createdAt = CreatedAt.FromIso8601("2024-01-15T10:30:00Z");
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong FromIso8601(string value)
        => From(DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal));

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
    /// var created1 = new CreatedAt(new DateTime(2024, 1, 15));
    /// var created2 = new CreatedAt(new DateTime(2024, 1, 15));
    /// bool areEqual = created1.Equals(created2); // true
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
    /// Tries to parse a <see cref="DateTime"/> from an ISO 8601 formatted string.
    /// </summary>
    /// <param name="content">The character span containing the ISO 8601 date to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (CreatedAt.TryParseIso8601("2024-01-15T10:30:00Z", out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParseIso8601(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (TryParse(content, CultureInfo.InvariantCulture, out strong))
        {
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTime"/> using the exact specified format.
    /// </summary>
    /// <param name="content">The character span containing the DateTime to parse.</param>
    /// <param name="format">The required format of the date and time string.</param>
    /// <param name="provider">A format provider for culture-specific parsing.</param>
    /// <param name="dateTimeStyles">Styles to apply during parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (CreatedAt.TryParseExact("15/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParseExact(ReadOnlySpan<char> content, string format, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParseExact(content, format, provider, dateTimeStyles, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTime"/> from a character span.
    /// </summary>
    /// <param name="content">The character span containing the DateTime to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <param name="formatProvider">An optional format provider for culture-specific parsing.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (CreatedAt.TryParse("2024-01-15", out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (DateTime.TryParse(content, formatProvider, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTime"/> from a character span using the provided format provider.
    /// </summary>
    /// <param name="content">The character span containing the DateTime to parse.</param>
    /// <param name="provider">A format provider for culture-specific parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParse(content, provider, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTime"/> from a character span using the provided format provider and styles.
    /// </summary>
    /// <param name="content">The character span containing the DateTime to parse.</param>
    /// <param name="provider">A format provider for culture-specific parsing.</param>
    /// <param name="dateTimeStyles">Styles to apply during parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTime.TryParse(content, provider, dateTimeStyles, out DateTime value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Returns the string representation of the underlying value using the specified format and provider.
    /// </summary>
    /// <param name="format">A standard or custom date and time format string.</param>
    /// <param name="provider">An optional format provider for culture-specific formatting.</param>
    /// <returns>The formatted string representation of the value.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// string formatted = createdAt.ToString("yyyy-MM-dd"); // "2024-01-15"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="DateTime"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid date/time.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(DateTime.Parse(s, provider));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="DateTime"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && DateTime.TryParse(s, provider, out DateTime value))
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
    /// <param name="s">The character span containing the date/time to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span is not a valid date/time.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(DateTime.Parse(s, provider));

    /// <summary>
    /// Returns the string representation of the underlying value using the specified format provider.
    /// </summary>
    /// <param name="provider">A format provider for culture-specific formatting.</param>
    /// <returns>The formatted string representation of the value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(IFormatProvider? provider) => Value.ToString(provider)!;

    /// <summary>
    /// Returns the string representation of the underlying value in ISO 8601 format.
    /// </summary>
    /// <returns>The ISO 8601 formatted string (e.g., "2024-01-15T10:30:00.0000000Z").</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// string iso8601 = createdAt.ToStringIso8601(); // "2024-01-15T10:30:00.0000000Z"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToStringIso8601() => Value.ToString("o", CultureInfo.InvariantCulture);
}
