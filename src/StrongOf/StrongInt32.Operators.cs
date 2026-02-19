// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

public abstract partial class StrongInt32<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and value represent the same Int32; otherwise, false.</returns>
    public static bool operator ==(StrongInt32<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is int intValue)
        {
            return strong.Value == intValue;
        }

        if (other is StrongInt32<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        if (other is uint uintValue)
        {
            return strong.Value == uintValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same Int32; otherwise, false.</returns>
    public static bool operator !=(StrongInt32<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongInt32 object is less than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongInt32 object is less than the value of other; otherwise, false.</returns>
    public static bool operator <(StrongInt32<TStrong> strong, object other)
    {
        if (other is int intValue)
        {
            return strong.Value < intValue;
        }

        if (other is long longValue)
        {
            return strong.Value < longValue;
        }

        if (other is StrongInt32<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        if (other is uint uintValue)
        {
            return strong.Value < uintValue;
        }

        if (other is double doubleValue)
        {
            return strong.Value < doubleValue;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value < decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongInt32 object is greater than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongInt32 object is greater than the value of other; otherwise, false.</returns>
    public static bool operator >(StrongInt32<TStrong> strong, object other)
    {
        if (other is int intValue)
        {
            return strong.Value > intValue;
        }

        if (other is long longValue)
        {
            return strong.Value > longValue;
        }

        if (other is StrongInt32<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        if (other is uint uintValue)
        {
            return strong.Value > uintValue;
        }

        if (other is double doubleValue)
        {
            return strong.Value > doubleValue;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value > decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongInt32 object is less than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongInt32 object is less than or equal to the value of other; otherwise, false.</returns>
    public static bool operator <=(StrongInt32<TStrong> strong, object other)
    {
        if (other is int intValue)
        {
            return strong.Value <= intValue;
        }

        if (other is long longValue)
        {
            return strong.Value <= longValue;
        }

        if (other is StrongInt32<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        if (other is uint uintValue)
        {
            return strong.Value <= uintValue;
        }

        if (other is double doubleValue)
        {
            return strong.Value <= doubleValue;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value <= decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongInt32 object is greater than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongInt32 object is greater than or equal to the value of other; otherwise, false.</returns>
    public static bool operator >=(StrongInt32<TStrong> strong, object other)
    {
        if (other is int intValue)
        {
            return strong.Value >= intValue;
        }

        if (other is long longValue)
        {
            return strong.Value >= longValue;
        }

        if (other is StrongInt32<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        if (other is uint uintValue)
        {
            return strong.Value >= uintValue;
        }

        if (other is double doubleValue)
        {
            return strong.Value >= doubleValue;
        }

        if (other is decimal decimalValue)
        {
            return strong.Value >= decimalValue;
        }

        return false;
    }
}
