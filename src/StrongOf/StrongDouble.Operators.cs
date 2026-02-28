// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongDouble<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of StrongDouble are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and value represent the same Double; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongDouble<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is double doubleValue)
        {
            return strong.Value == doubleValue;
        }

        if (other is StrongDouble<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        if (other is float floatValue)
        {
            return strong.Value == floatValue;
        }

        if (other is int intValue)
        {
            return strong.Value == intValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongDouble are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same Double; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongDouble<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongDouble object is less than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDouble object is less than the value of other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongDouble<TStrong> strong, object other)
    {
        if (other is double doubleValue)
        {
            return strong.Value < doubleValue;
        }

        if (other is StrongDouble<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        if (other is float floatValue)
        {
            return strong.Value < floatValue;
        }

        if (other is int intValue)
        {
            return strong.Value < intValue;
        }

        if (other is decimal decimalValue)
        {
            return (decimal)strong.Value < decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongDouble object is greater than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDouble object is greater than the value of other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongDouble<TStrong> strong, object other)
    {
        if (other is double doubleValue)
        {
            return strong.Value > doubleValue;
        }

        if (other is StrongDouble<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        if (other is float floatValue)
        {
            return strong.Value > floatValue;
        }

        if (other is int intValue)
        {
            return strong.Value > intValue;
        }

        if (other is decimal decimalValue)
        {
            return (decimal)strong.Value > decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongDouble object is less than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDouble object is less than or equal to the value of other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongDouble<TStrong> strong, object other)
    {
        if (other is double doubleValue)
        {
            return strong.Value <= doubleValue;
        }

        if (other is StrongDouble<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        if (other is float floatValue)
        {
            return strong.Value <= floatValue;
        }

        if (other is int intValue)
        {
            return strong.Value <= intValue;
        }

        if (other is decimal decimalValue)
        {
            return (decimal)strong.Value <= decimalValue;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongDouble object is greater than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDouble object is greater than or equal to the value of other; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongDouble<TStrong> strong, object other)
    {
        if (other is double doubleValue)
        {
            return strong.Value >= doubleValue;
        }

        if (other is StrongDouble<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        if (other is float floatValue)
        {
            return strong.Value >= floatValue;
        }

        if (other is int intValue)
        {
            return strong.Value >= intValue;
        }

        if (other is decimal decimalValue)
        {
            return (decimal)strong.Value >= decimalValue;
        }

        return false;
    }
}
