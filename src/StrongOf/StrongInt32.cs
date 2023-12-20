using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strong type of Int32.
/// </summary>
/// <typeparam name="TStrong">The type of the strong Int32.</typeparam>
public abstract class StrongInt32<TStrong>(int Value) : StrongOf<int, TStrong>(Value), IComparable, IStrongInt32
    where TStrong : StrongInt32<TStrong>
{
    /// <summary>
    /// Creates a new instance of StrongInt32 from a nullable Int32 value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongInt32 if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(int? value)
    {
        if (value.HasValue)
        {
            TStrong strong = From(value.Value);
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(object? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is TStrong otherStrong)
        {
            return Value.CompareTo(otherStrong.Value);
        }

        throw new ArgumentException($"Object is not a {typeof(TStrong)}");
    }

    /// <summary>
    /// Tries to parse an Int32 from a ReadOnlySpan of char and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing an Int32 to convert.</param>
    /// <param name="strong">When this method returns, contains the Int32 value equivalent to the Int32 contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (int.TryParse(content, out int value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // Operators

    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same Int32; otherwise, false.</returns>
    public static bool operator ==(StrongInt32<TStrong>? strong, int other)
    {
        if (strong is null)
        {
            return false;
        }

        return strong.Value == other;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same Int32; otherwise, false.</returns>
    public static bool operator !=(StrongInt32<TStrong>? strong, int other)
    {
        return (strong == other) is false;
    }

    ///// <summary>
    ///// Determines whether two specified instances of StrongInt32 are equal.
    ///// </summary>
    ///// <param name="strong">The first instance to compare.</param>
    ///// <param name="other">The second instance to compare.</param>
    ///// <returns>True if strong and value represent the same Int32; otherwise, false.</returns>
    //public static bool operator ==(StrongInt32<TStrong>? strong, int? other)
    //{
    //    if (strong is null)
    //    {
    //        return other is null;
    //    }

    //    return strong.Value == other;
    //}

    ///// <summary>
    ///// Determines whether two specified instances of StrongInt32 are not equal.
    ///// </summary>
    ///// <param name="strong">The first instance to compare.</param>
    ///// <param name="other">The second instance to compare.</param>
    ///// <returns>True if strong and other do not represent the same Int32; otherwise, false.</returns>
    //public static bool operator !=(StrongInt32<TStrong>? strong, int? other)
    //{
    //    return (strong == other) is false;
    //}

    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="value">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same Int32; otherwise, false.</returns>
    public static bool operator ==(StrongInt32<TStrong>? strong, StrongInt32<TStrong>? other)
    {
        if (strong is null && other is null)
        {
            return true;
        }

        if (strong is not null && other is not null)
        {
            return strong.Value == other.Value;
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongInt32 are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same Int32; otherwise, false.</returns>
    public static bool operator !=(StrongInt32<TStrong>? strong, StrongInt32<TStrong>? other)
    {
        return (strong == other) is false;
    }

    // Equals

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => base.GetHashCode();
}
