// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances are equal using ordinal comparison.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare. Can be a <see cref="string"/> or another <see cref="StrongString{TStrong}"/>.</param>
    /// <returns><c>true</c> if both represent the same string (ordinal comparison); otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email1 = new Email("user@example.com");
    /// var email2 = new Email("user@example.com");
    ///
    /// bool equal = email1 == email2;           // true
    /// bool equalRaw = email1 == "user@example.com"; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongString<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is string stringValue)
        {
            return string.Equals(strong.Value, stringValue, StringComparison.Ordinal);
        }

        if (other is StrongString<TStrong> otherStrong)
        {
            return string.Equals(strong.Value, otherStrong.Value, StringComparison.Ordinal);
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns><c>true</c> if the strings are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongString<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the left operand is less than the right operand using ordinal comparison.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> if left is less than right; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var a = new Email("a@example.com");
    /// var b = new Email("b@example.com");
    ///
    /// bool result = a &lt; b; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongString<TStrong> left, StrongString<TStrong> right)
        => string.CompareOrdinal(left.Value, right.Value) < 0;

    /// <summary>
    /// Determines whether the left operand is greater than the right operand using ordinal comparison.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> if left is greater than right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongString<TStrong> left, StrongString<TStrong> right)
        => string.CompareOrdinal(left.Value, right.Value) > 0;

    /// <summary>
    /// Determines whether the left operand is less than or equal to the right operand.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> if left is less than or equal to right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongString<TStrong> left, StrongString<TStrong> right)
        => string.CompareOrdinal(left.Value, right.Value) <= 0;

    /// <summary>
    /// Determines whether the left operand is greater than or equal to the right operand.
    /// </summary>
    /// <param name="left">The left-hand operand.</param>
    /// <param name="right">The right-hand operand.</param>
    /// <returns><c>true</c> if left is greater than or equal to right; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongString<TStrong> left, StrongString<TStrong> right)
        => string.CompareOrdinal(left.Value, right.Value) >= 0;
}
