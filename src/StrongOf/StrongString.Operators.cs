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
}
