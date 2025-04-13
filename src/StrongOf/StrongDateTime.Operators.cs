// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

public abstract partial class StrongDateTime<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of StrongDateTime are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same DateTime; otherwise, false.</returns>
    public static bool operator ==(StrongDateTime<TStrong> strong, DateTime other)
    {
        if (strong is null)
        {
            return false;
        }

        return strong.Value == other;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongDateTime are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same DateTime; otherwise, false.</returns>
    public static bool operator !=(StrongDateTime<TStrong> strong, DateTime other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongDateTime are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and value represent the same DateTime; otherwise, false.</returns>
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
    /// Determines whether two specified instances of StrongDateTime are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same DateTime; otherwise, false.</returns>
    public static bool operator !=(StrongDateTime<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the value of the current StrongDateTime object is less than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDateTime object is less than the value of other; otherwise, false.</returns>
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
    /// Determines whether the value of the current StrongDateTime object is greater than the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDateTime object is greater than the value of other; otherwise, false.</returns>
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
    /// Determines whether the value of the current StrongDateTime object is less than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDateTime object is less than or equal to the value of other; otherwise, false.</returns>
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
    /// Determines whether the value of the current StrongDateTime object is greater than or equal to the value of a specified object.
    /// </summary>
    /// <param name="strong">The current instance.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the value of the current StrongDateTime object is greater than or equal to the value of other; otherwise, false.</returns>
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
