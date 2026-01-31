// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="long"/> (Int64) value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple long integer values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific long integer types like <c>FileSize</c>, <c>Timestamp</c>, etc.
/// The compiler will prevent accidental mixing of different long types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed long types
/// public sealed class FileSize(long value) : StrongInt64&lt;FileSize&gt;(value) { }
/// public sealed class Timestamp(long value) : StrongInt64&lt;Timestamp&gt;(value) { }
///
/// // Usage
/// var fileSize = new FileSize(1024 * 1024); // 1 MB
/// var timestamp = Timestamp.From(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="long"/> value.</param>
public abstract partial class StrongInt64<TStrong>(long Value)
        : StrongOf<long, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongInt64
    where TStrong : StrongInt64<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="long"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="long"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public long AsInt64() => Value;

    /// <summary>
    /// Gets the underlying <see cref="long"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="long"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public long AsLong() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="long"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="long"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// long? nullableSize = GetOptionalFileSize();
    /// FileSize? size = FileSize.FromNullable(nullableSize);
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(long? value)
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
    /// var size1 = new FileSize(1024);
    /// var size2 = new FileSize(1024);
    /// bool areEqual = size1.Equals(size2); // true
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
    /// Tries to parse a <see cref="long"/> from a character span and creates a strong type instance.
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
    /// if (FileSize.TryParse("1048576", out FileSize? size))
    /// {
    ///     Console.WriteLine($"File size: {size} bytes");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (long.TryParse(content, formatProvider, out long value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }
}
