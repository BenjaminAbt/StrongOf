// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongGuid<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare. Can be a <see cref="Guid"/> or another <see cref="StrongGuid{TStrong}"/>.</param>
    /// <returns><c>true</c> if both represent the same <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var id1 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// var id2 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// var rawGuid = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
    ///
    /// bool equal = id1 == id2;      // true
    /// bool equalRaw = id1 == rawGuid; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is Guid guidValue)
        {
            return strong.Value == guidValue;
        }

        if (other is StrongGuid<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns><c>true</c> if they do not represent the same <see cref="Guid"/>; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongGuid<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the left operand is greater than the right operand.
    /// </summary>
    /// <param name="strong">The left-hand operand.</param>
    /// <param name="other">The right-hand operand. Can be a <see cref="Guid"/> or <see cref="StrongGuid{TStrong}"/>.</param>
    /// <returns><c>true</c> if left is greater than right; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var id1 = new UserId(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"));
    /// var id2 = new UserId(Guid.Parse("00000000-0000-0000-0000-000000000000"));
    ///
    /// bool result = id1 > id2; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is Guid guidValue)
        {
            return strong.Value > guidValue;
        }

        if (other is StrongGuid<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the left operand is less than the right operand.
    /// </summary>
    /// <param name="strong">The left-hand operand.</param>
    /// <param name="other">The right-hand operand.</param>
    /// <returns><c>true</c> if left is less than right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is Guid guidValue)
        {
            return strong.Value < guidValue;
        }

        if (other is StrongGuid<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the left operand is greater than or equal to the right operand.
    /// </summary>
    /// <param name="strong">The left-hand operand.</param>
    /// <param name="other">The right-hand operand.</param>
    /// <returns><c>true</c> if left is greater than or equal to right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is Guid guidValue)
        {
            return strong.Value >= guidValue;
        }

        if (other is StrongGuid<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the left operand is less than or equal to the right operand.
    /// </summary>
    /// <param name="strong">The left-hand operand.</param>
    /// <param name="other">The right-hand operand.</param>
    /// <returns><c>true</c> if left is less than or equal to right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is Guid guidValue)
        {
            return strong.Value <= guidValue;
        }

        if (other is StrongGuid<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        return false;
    }
}
