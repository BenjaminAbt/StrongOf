namespace StrongOf;

public abstract partial class StrongGuid<TStrong>
{
    /// <summary>
    /// Determines whether two specified instances of StrongGuid are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and value represent the same Guid; otherwise, false.</returns>
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
    /// Determines whether two specified instances of StrongGuid are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same Guid; otherwise, false.</returns>
    public static bool operator !=(StrongGuid<TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether a <see cref="StrongGuid{TStrong}"/> object is greater than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongGuid{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="strong"/> object is greater than the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="strong"/> is <see langword="null"/>, the method returns <see langword="true"/> if <paramref name="other"/> is also <see langword="null"/>, otherwise <see langword="false"/>.
    /// If <paramref name="other"/> is a <see cref="Guid"/>, the method compares the value of <paramref name="strong"/> with the <see cref="Guid"/> value.
    /// If <paramref name="other"/> is a <see cref="StrongGuid{TStrong}"/> object, the method compares the value of <paramref name="strong"/> with the value of the other <see cref="StrongGuid{TStrong}"/> object.
    /// </remarks>
    public static bool operator >(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
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
    /// Determines whether a <see cref="StrongGuid{TStrong}"/> object is less than another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongGuid{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="strong"/> object is less than the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="strong"/> is <see langword="null"/>, the method returns <see langword="true"/> if <paramref name="other"/> is also <see langword="null"/>, otherwise <see langword="false"/>.
    /// If <paramref name="other"/> is a <see cref="Guid"/>, the method compares the value of <paramref name="strong"/> with the <see cref="Guid"/> value.
    /// If <paramref name="other"/> is a <see cref="StrongGuid{TStrong}"/> object, the method compares the value of <paramref name="strong"/> with the value of the other <see cref="StrongGuid{TStrong}"/> object.
    /// </remarks>
    public static bool operator <(StrongGuid<TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
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
    /// Determines whether a <see cref="StrongGuid{TStrong}"/> object is greater than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongGuid{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="strong"/> object is greater than or equal to the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="strong"/> is <see langword="null"/>, the method returns <see langword="true"/> if <paramref name="other"/> is also <see langword="null"/>, otherwise <see langword="false"/>.
    /// If <paramref name="other"/> is a <see cref="Guid"/>, the method compares the value of <paramref name="strong"/> with the <see cref="Guid"/> value.
    /// If <paramref name="other"/> is a <see cref="StrongGuid{TStrong}"/> object, the method compares the value of <paramref name="strong"/> with the value of the other <see cref="StrongGuid{TStrong}"/> object.
    /// </remarks>
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
    /// Determines whether a <see cref="StrongGuid{TStrong}"/> object is less than or equal to another object.
    /// </summary>
    /// <param name="strong">The <see cref="StrongGuid{TStrong}"/> object to compare.</param>
    /// <param name="other">The object to compare with the <paramref name="strong"/> object.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="strong"/> object is less than or equal to the <paramref name="other"/> object;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// If <paramref name="strong"/> is <see langword="null"/>, the method returns <see langword="true"/> if <paramref name="other"/> is also <see langword="null"/>, otherwise <see langword="false"/>.
    /// If <paramref name="other"/> is a <see cref="Guid"/>, the method compares the value of <paramref name="strong"/> with the <see cref="Guid"/> value.
    /// If <paramref name="other"/> is a <see cref="StrongGuid{TStrong}"/> object, the method compares the value of <paramref name="strong"/> with the value of the other <see cref="StrongGuid{TStrong}"/> object.
    /// </remarks>
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
