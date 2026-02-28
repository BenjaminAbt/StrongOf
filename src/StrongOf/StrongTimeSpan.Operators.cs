// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongTimeSpan<TStrong>
{
    /// <summary>
    /// Determines whether two <see cref="StrongTimeSpan{TStrong}"/> instances are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right as TStrong);
    }

    /// <summary>
    /// Determines whether two <see cref="StrongTimeSpan{TStrong}"/> instances are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
        => !(left == right);

    /// <summary>
    /// Determines whether the left instance is less than the right instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return left.Value < (right?.Value ?? TimeSpan.MaxValue);
    }

    /// <summary>
    /// Determines whether the left instance is greater than the right instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
    {
        if (left is null)
        {
            return false;
        }

        return left.Value > (right?.Value ?? TimeSpan.MinValue);
    }

    /// <summary>
    /// Determines whether the left instance is less than or equal to the right instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
        => !(left > right);

    /// <summary>
    /// Determines whether the left instance is greater than or equal to the right instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongTimeSpan<TStrong>? left, StrongTimeSpan<TStrong>? right)
        => !(left < right);
}
