using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strong type of Int64.
/// </summary>
/// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
public abstract partial class StrongInt64<TStrong>(long Value)
        : StrongOf<long, TStrong>(Value), IComparable, IStrongInt64
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

        throw new ArgumentException($"Object is not a {typeof(TStrong)}", nameof(other));
    }

    /// <summary>
    /// Tries to parse the specified content into a <typeparamref name="TStrong"/> object.
    /// </summary>
    /// <param name="content">The content to parse.</param>
    /// <param name="strong">When this method returns, contains the parsed value if the parsing succeeded, or <see langword="null">null</see> if the parsing failed. The parsing is case-sensitive.</param>
    /// <param name="formatProvider">An optional <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns><see langword="true">true</see> if the parsing was successful; otherwise, <see langword="false">false</see>.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong, IFormatProvider? formatProvider = null)
    {
        if (long.TryParse(content, formatProvider, out long value))
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
