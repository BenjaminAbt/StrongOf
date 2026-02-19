// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongDecimal<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongDecimal{TStrong}"/> are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var price1 = new Price(99.99m);
    /// var price2 = new Price(99.99m);
    /// bool areEqual = price1 == price2; // true
    /// bool sameAsRaw = price1 == 99.99m; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value == decimalValue;
        }

        if (other is StrongDecimal<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongDecimal{TStrong}"/> are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> do not represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongDecimal<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDecimal{TStrong}"/> object is less than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongDecimal{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is less than the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var price = new Price(50.00m);
    /// bool isLess = price &lt; 100.00m; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value < decimalValue;
        }

        if (other is StrongDecimal<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        if (other is int intValue)
        {
            return strong.Value < intValue;
        }

        if (other is long longValue)
        {
            return strong.Value < longValue;
        }

        if (other is uint uintValue)
        {
            return strong.Value < uintValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDecimal{TStrong}"/> object is greater than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongDecimal{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is greater than the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var price = new Price(150.00m);
    /// bool isGreater = price &gt; 100.00m; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value > decimalValue;
        }

        if (other is StrongDecimal<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        if (other is int intValue)
        {
            return strong.Value > intValue;
        }

        if (other is long longValue)
        {
            return strong.Value > longValue;
        }

        if (other is uint uintValue)
        {
            return strong.Value > uintValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDecimal{TStrong}"/> object is less than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongDecimal{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is less than or equal to the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var price = new Price(100.00m);
    /// bool isLessOrEqual = price &lt;= 100.00m; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value <= decimalValue;
        }

        if (other is StrongDecimal<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        if (other is int intValue)
        {
            return strong.Value <= intValue;
        }

        if (other is long longValue)
        {
            return strong.Value <= longValue;
        }

        if (other is uint uintValue)
        {
            return strong.Value <= uintValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongDecimal{TStrong}"/> object is greater than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongDecimal{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is greater than or equal to the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var price = new Price(100.00m);
    /// bool isGreaterOrEqual = price &gt;= 100.00m; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value >= decimalValue;
        }

        if (other is StrongDecimal<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        if (other is int intValue)
        {
            return strong.Value >= intValue;
        }

        if (other is long longValue)
        {
            return strong.Value >= longValue;
        }

        if (other is uint uintValue)
        {
            return strong.Value >= uintValue;
        }

        return false;
    }
}
