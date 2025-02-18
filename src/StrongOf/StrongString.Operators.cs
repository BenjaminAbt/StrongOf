namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Determines whether the specified strong string and string are equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the specified strong string and string are equal; otherwise, false.</returns>
    public static bool operator ==(StrongString<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is string stringValue)
        {
            return strong.Value.Equals(stringValue, StringComparison.Ordinal);
        }

        if (other is StrongString<TStrong> otherStrong)
        {
            return string.Equals(strong.Value, otherStrong.Value, StringComparison.Ordinal);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified strong string and string are not equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if the specified strong string and string are not equal; otherwise, false.</returns>
    public static bool operator !=(StrongString<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the specified <see cref="StrongString{TStrong}"/> instance is less than another.
    /// </summary>
    /// <param name="left">The left-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <param name="right">The right-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <(StrongString<TStrong> left, StrongString<TStrong> right)
        => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the specified <see cref="StrongString{TStrong}"/> instance is greater than another.
    /// </summary>
    /// <param name="left">The left-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <param name="right">The right-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >(StrongString<TStrong> left, StrongString<TStrong> right)
        => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the specified <see cref="StrongString{TStrong}"/> instance is less than or equal to another.
    /// </summary>
    /// <param name="left">The left-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <param name="right">The right-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator <=(StrongString<TStrong> left, StrongString<TStrong> right)
        => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the specified <see cref="StrongString{TStrong}"/> instance is greater than or equal to another.
    /// </summary>
    /// <param name="left">The left-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <param name="right">The right-hand <see cref="StrongString{TStrong}"/> instance.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool operator >=(StrongString<TStrong> left, StrongString<TStrong> right)
        => left.CompareTo(right) >= 0;
}
