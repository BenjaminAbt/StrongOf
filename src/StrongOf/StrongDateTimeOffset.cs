// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="DateTimeOffset"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with date and time values that include timezone information.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific DateTimeOffset types like <c>CreatedAt</c>, <c>LastModified</c>, <c>ScheduledAt</c>, etc.
/// The compiler will prevent accidental mixing of different DateTimeOffset types.
/// </para>
/// <para>
/// <b>Recommendation:</b> Prefer <see cref="StrongDateTimeOffset{TStrong}"/> over <see cref="StrongDateTime{TStrong}"/>
/// for new code that requires timezone awareness and proper handling of local vs UTC times.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed DateTimeOffset types
/// public sealed class CreatedAt(DateTimeOffset value) : StrongDateTimeOffset&lt;CreatedAt&gt;(value) { }
/// public sealed class LastModified(DateTimeOffset value) : StrongDateTimeOffset&lt;LastModified&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public TimeSpan GetAge(CreatedAt createdAt, LastModified lastModified)
/// {
///     // Cannot accidentally swap createdAt and lastModified!
///     return lastModified.Value - createdAt.Value;
/// }
///
/// // Create instances
/// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);         // Fastest
/// var createdAt = CreatedAt.From(DateTimeOffset.UtcNow);        // For generic scenarios
/// var createdAt = CreatedAt.FromIso8601("2024-01-15T10:30:00+00:00"); // From ISO 8601 string
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="DateTimeOffset"/> value.</param>
public abstract partial class StrongDateTimeOffset<TStrong>(DateTimeOffset Value)
        : StrongOf<DateTimeOffset, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongDateTimeOffset,
          IParsable<TStrong>, ISpanParsable<TStrong>, IFormattable
    where TStrong : StrongDateTimeOffset<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="DateTimeOffset"/> value.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// DateTimeOffset rawValue = createdAt.AsDateTimeOffset();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateTimeOffset AsDateTimeOffset() => Value;

    /// <summary>
    /// Gets the <see cref="DateTime"/> portion of the value (local time).
    /// </summary>
    /// <returns>The local <see cref="DateTime"/> value.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// DateTime localTime = createdAt.AsDateTime();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateTime AsDateTime() => Value.DateTime;

    /// <summary>
    /// Gets the <see cref="DateTime"/> portion of the value in UTC.
    /// </summary>
    /// <returns>The UTC <see cref="DateTime"/> value.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.Now);
    /// DateTime utcTime = createdAt.AsDateTimeUtc();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateTime AsDateTimeUtc() => Value.UtcDateTime;

    /// <summary>
    /// Gets the date portion of the value as a <see cref="DateOnly"/>.
    /// </summary>
    /// <returns>A <see cref="DateOnly"/> representing the date portion.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// DateOnly date = createdAt.AsDate();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public DateOnly AsDate() => DateOnly.FromDateTime(Value.Date);

    /// <summary>
    /// Gets the time portion of the value as a <see cref="TimeOnly"/>.
    /// </summary>
    /// <returns>A <see cref="TimeOnly"/> representing the time portion.</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// TimeOnly time = createdAt.AsTime();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public TimeOnly AsTime() => TimeOnly.FromDateTime(Value.DateTime);

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="DateTimeOffset"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="DateTimeOffset"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// DateTimeOffset? nullableDate = GetOptionalDate();
    /// CreatedAt? createdAt = CreatedAt.FromNullable(nullableDate);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(DateTimeOffset? value)
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
    /// <param name="value">The ISO 8601 string to convert (e.g., "2024-01-15T10:30:00+00:00").</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The string is not in a valid ISO 8601 format.</exception>
    /// <example>
    /// <code>
    /// var createdAt = CreatedAt.FromIso8601("2024-01-15T10:30:00+00:00");
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong FromIso8601(string value)
        => From(DateTimeOffset.ParseExact(value, "o", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal));

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
    /// var created1 = new CreatedAt(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero));
    /// var created2 = new CreatedAt(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero));
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
    /// Tries to parse a <see cref="DateTimeOffset"/> from an ISO 8601 formatted string.
    /// </summary>
    /// <param name="content">The character span containing the ISO 8601 date to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (CreatedAt.TryParseIso8601("2024-01-15T10:30:00+00:00", out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParseIso8601(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (TryParseExact(content, "o", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out strong))
        {
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTimeOffset"/> using the exact specified format.
    /// </summary>
    /// <param name="content">The character span containing the DateTimeOffset to parse.</param>
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
    /// if (CreatedAt.TryParseExact("15/01/2024 +01:00", "dd/MM/yyyy zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParseExact(ReadOnlySpan<char> content, string format, IFormatProvider? provider, DateTimeStyles dateTimeStyles, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParseExact(content, format, provider, dateTimeStyles, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTimeOffset"/> from a character span.
    /// </summary>
    /// <param name="content">The character span containing the DateTimeOffset to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <param name="formatProvider">An optional format provider for culture-specific parsing.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (CreatedAt.TryParse("2024-01-15T10:30:00+00:00", out CreatedAt? createdAt))
    /// {
    ///     Console.WriteLine($"Created: {createdAt}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (DateTimeOffset.TryParse(content, formatProvider, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTimeOffset"/> from a character span using the provided format provider.
    /// </summary>
    /// <param name="content">The character span containing the DateTimeOffset to parse.</param>
    /// <param name="provider">A format provider for culture-specific parsing.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, IFormatProvider? provider, [NotNullWhen(true)] out TStrong? strong)
    {
        if (DateTimeOffset.TryParse(content, provider, out DateTimeOffset value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Tries to parse a <see cref="DateTimeOffset"/> from a character span using the provided format provider and styles.
    /// </summary>
    /// <param name="content">The character span containing the DateTimeOffset to parse.</param>
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
        if (DateTimeOffset.TryParse(content, provider, dateTimeStyles, out DateTimeOffset value))
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
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// string formatted = createdAt.ToString("yyyy-MM-dd HH:mm:ss zzz"); // "2024-01-15 10:30:00 +00:00"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string? format, IFormatProvider? provider) => Value.ToString(format, provider);

    // IParsable<TStrong>

    /// <summary>
    /// Parses a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="DateTimeOffset"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
    /// <exception cref="FormatException"><paramref name="s"/> is not a valid date/time offset.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(string s, IFormatProvider? provider)
    {
        ArgumentNullException.ThrowIfNull(s);
        return From(DateTimeOffset.Parse(s, provider));
    }

    /// <summary>
    /// Tries to parse a string to create a new instance of the strong type.
    /// </summary>
    /// <param name="s">The string representation of a <see cref="DateTimeOffset"/>.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <param name="result">When this method returns, contains the parsed strong type if successful; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TStrong result)
    {
        if (s is not null && DateTimeOffset.TryParse(s, provider, out DateTimeOffset value))
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
    /// <param name="s">The character span containing the date/time offset to parse.</param>
    /// <param name="provider">An optional format provider for culture-specific parsing.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    /// <exception cref="FormatException">The span is not a valid date/time offset.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        => From(DateTimeOffset.Parse(s, provider));

    /// <summary>
    /// Returns the string representation of the underlying value in ISO 8601 format.
    /// </summary>
    /// <returns>The ISO 8601 formatted string (e.g., "2024-01-15T10:30:00.0000000+00:00").</returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTimeOffset.UtcNow);
    /// string iso8601 = createdAt.ToStringIso8601(); // "2024-01-15T10:30:00.0000000+00:00"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToStringIso8601() => Value.ToString("o", CultureInfo.InvariantCulture);
}
