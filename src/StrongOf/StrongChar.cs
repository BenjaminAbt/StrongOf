using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed character.
/// </summary>
/// <typeparam name="TStrong">The type of the strong character.</typeparam>
public abstract class StrongChar<TStrong>(char Value) : StrongOf<char, TStrong>(Value), IComparable, IStrongChar
    where TStrong : StrongChar<TStrong>
{
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
    /// Tries to parse an char from a ReadOnlySpan of char and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A ReadOnlySpan of char containing an char to convert.</param>
    /// <param name="strong">When this method returns, contains the char value equivalent to the char contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(string content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (char.TryParse(content, out char value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    // Operators
    /// <summary>
    /// Determines whether two specified instances of StrongChar are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="value">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same char; otherwise, false.</returns>
    public static bool operator ==(StrongChar<TStrong> strong, char value)
    {
        if (strong is null)
        {
            return true;
        }

        return strong.Value == value;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongChar are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same char; otherwise, false.</returns>
    public static bool operator !=(StrongChar<TStrong> strong, char other)
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
