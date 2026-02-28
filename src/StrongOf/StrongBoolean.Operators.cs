// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongBoolean<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongBoolean{TStrong}"/> are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other represent the same Boolean; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongBoolean<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is bool boolValue)
        {
            return strong.Value == boolValue;
        }

        if (other is StrongBoolean<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongBoolean{TStrong}"/> are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same Boolean; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongBoolean<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }
}
