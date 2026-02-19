// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

public abstract partial class StrongChar<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongChar{TStrong}"/> are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var grade1 = new GradeLevel('A');
    /// var grade2 = new GradeLevel('A');
    /// bool areEqual = grade1 == grade2; // true
    /// bool sameAsRaw = grade1 == 'A'; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator ==(StrongChar<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is char charValue)
        {
            return strong.Value == charValue;
        }

        if (other is StrongChar<TStrong> otherStrong)
        {
            return strong.Value == otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongChar{TStrong}"/> are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> and <paramref name="other"/> do not represent the same value;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongChar<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongChar{TStrong}"/> object is less than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongChar{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is less than the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var grade = new GradeLevel('A');
    /// bool isLess = grade &lt; 'B'; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <(StrongChar<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is char charValue)
        {
            return strong.Value < charValue;
        }

        if (other is StrongChar<TStrong> otherStrong)
        {
            return strong.Value < otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongChar{TStrong}"/> object is greater than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongChar{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is greater than the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var grade = new GradeLevel('B');
    /// bool isGreater = grade &gt; 'A'; // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >(StrongChar<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return false;
        }

        if (other is char charValue)
        {
            return strong.Value > charValue;
        }

        if (other is StrongChar<TStrong> otherStrong)
        {
            return strong.Value > otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongChar{TStrong}"/> object is less than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongChar{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is less than or equal to the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator <=(StrongChar<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is char charValue)
        {
            return strong.Value <= charValue;
        }

        if (other is StrongChar<TStrong> otherStrong)
        {
            return strong.Value <= otherStrong.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongChar{TStrong}"/> object is greater than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongChar{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> object is greater than or equal to the <paramref name="other"/> object;
    /// otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator >=(StrongChar<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is char charValue)
        {
            return strong.Value >= charValue;
        }

        if (other is StrongChar<TStrong> otherStrong)
        {
            return strong.Value >= otherStrong.Value;
        }

        return false;
    }
}
