// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

public abstract partial class StrongDecimal<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of StrongDecimal are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and value represent the same decimal; otherwise, false.</returns>
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
    /// Determines whether two specified instances of StrongDecimal are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same decimal; otherwise, false.</returns>
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
    /// <see langword="true"/> if the <paramref name="strong"/> object is less than the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator <(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
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
    /// <see langword="true"/> if the <paramref name="strong"/> object is greater than the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator >(StrongDecimal<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
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
    /// <see langword="true"/> if the <paramref name="strong"/> object is less than or equal to the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
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
    /// <see langword="true"/> if the <paramref name="strong"/> object is greater than or equal to the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
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
