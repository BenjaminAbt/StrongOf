using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strong type of Int64.
/// </summary>
/// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
public abstract partial class StrongInt64<TStrong>(long Value) : StrongOf<long, TStrong>(Value), IComparable, IStrongInt64
    where TStrong : StrongInt64<TStrong>
{

    /// <summary>
    /// Returns the value of the strong type as a long.
    /// </summary>
    public long AsInt64() => Value;

    /// <summary>
    /// Returns the value of the strong type as a long.
    /// </summary>
    public long AsLong() => Value;

    /// <summary>
    /// Creates a new instance of StrongInt64 from a nullable Int64 value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongInt64 if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(long? value)
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
    /// Tries to parse an Int64 from a ReadOnlySpan of char and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing an Int64 to convert.</param>
    /// <param name="strong">When this method returns, contains the Int64 value equivalent to the Int64 contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (long.TryParse(content, out long value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
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
