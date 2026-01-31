// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongDateTime<TStrong>
{
    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is equal to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The <see cref="DateTime"/> to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow);
    /// bool isEqual = createdAt == DateTime.UtcNow;
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongDateTime<TStrong> strong, DateTime other)
    {
        if (strong is null)
        {
            return false;
        }

        return strong.Value == other;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is not equal to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The <see cref="DateTime"/> to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> do not represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongDateTime<TStrong> strong, DateTime other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongDateTime{TStrong}"/> are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongDateTime<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is DateTime dtValue)
        {
            return strong.Value == dtValue;
        }

        if (other is StrongDateTime<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongDateTime{TStrong}"/> are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> do not represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongDateTime<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is less than another object.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> is less than <paramref name="other"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var createdAt = new CreatedAt(DateTime.UtcNow.AddDays(-1));
    /// bool isEarlier = createdAt &lt; DateTime.UtcNow; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongDateTime<TStrong> strong, object other)
    {
        if (other is DateTime dtValue)
        {
            return strong.Value < dtValue;
        }

        if (other is StrongDateTime<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        if (other is DateTimeOffset dtoValue)
        {
            return strong.Value < dtoValue.Date;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is greater than another object.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> is greater than <paramref name="other"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongDateTime<TStrong> strong, object other)
    {
        if (other is DateTime dtValue)
        {
            return strong.Value > dtValue;
        }

        if (other is StrongDateTime<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        if (other is DateTimeOffset dtoValue)
        {
            return strong.Value > dtoValue.Date;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is less than or equal to another object.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> is less than or equal to <paramref name="other"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongDateTime<TStrong> strong, object other)
    {
        if (other is DateTime dtValue)
        {
            return strong.Value <= dtValue;
        }

        if (other is StrongDateTime<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        if (other is DateTimeOffset dtoValue)
        {
            return strong.Value <= dtoValue.Date;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDateTime{TStrong}"/> is greater than or equal to another object.
    /// </summary>
    /// <param name="strong">The strong type instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> is greater than or equal to <paramref name="other"/>;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongDateTime<TStrong> strong, object other)
    {
        if (other is DateTime dtValue)
        {
            return strong.Value >= dtValue;
        }

        if (other is StrongDateTime<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        if (other is DateTimeOffset dtoValue)
        {
            return strong.Value >= dtoValue.Date;
        }

        return false;
    }
}
