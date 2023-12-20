using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strong type of string.
/// </summary>
/// <typeparam name="TStrong">The type of the strong string.</typeparam>
public abstract class StrongString<TStrong>(string Value) : StrongOf<string, TStrong>(Value), IComparable, IStrongString
    where TStrong : StrongString<TStrong>
{
    /// <summary>
    /// Creates a new instance of StrongString from a nullable string value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongString if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(string? value)
    {
        if (value != null)
        {
            TStrong strong = From(value);
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Creates a new instance of the strong string from the specified trimmed value.
    /// </summary>
    /// <param name="value">The value to create the strong string from.</param>
    /// <returns>A new instance of the strong string.</returns>
    public static TStrong FromTrimmed(string value)
      => From(value.Trim());

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
    /// Creates a new instance of the strong string that is empty.
    /// </summary>
    /// <returns>A new instance of the strong string.</returns>
    public static TStrong Empty()
        => From("");

    /// <summary>
    /// Determines whether the current instance is empty.
    /// </summary>
    /// <returns>True if the current instance is empty; otherwise, false.</returns>
    public bool IsEmpty()
        => Value == "";

    // Operators

    /// <summary>
    /// Determines whether the specified strong string and string are equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="value">The string to compare.</param>
    /// <returns>True if the specified strong string and string are equal; otherwise, false.</returns>
    public static bool operator ==(StrongString<TStrong> strong, string value)
    {
        if (strong is null)
        {
            return value is null;
        }

        return strong.Value == value;
    }

    /// <summary>
    /// Determines whether the specified strong string and string are not equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="other">The string to compare.</param>
    /// <returns>True if the specified strong string and string are not equal; otherwise, false.</returns>
    public static bool operator !=(StrongString<TStrong> strong, string other)
    {
        return (strong == other) is false;
    }

    /// <summary>
    /// Determines whether the specified strong string and string are equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="value">The string to compare.</param>
    /// <returns>True if the specified strong string and string are equal; otherwise, false.</returns>
    public static bool operator ==(StrongString<TStrong> strong, StrongString<TStrong>? other)
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
    /// Determines whether the specified strong string and string are not equal.
    /// </summary>
    /// <param name="strong">The strong string to compare.</param>
    /// <param name="other">The string to compare.</param>
    /// <returns>True if the specified strong string and string are not equal; otherwise, false.</returns>
    public static bool operator !=(StrongString<TStrong> strong, StrongString<TStrong> other)
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
