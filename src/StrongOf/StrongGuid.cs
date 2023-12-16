﻿using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strong type of Guid.
/// </summary>
/// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
public abstract class StrongGuid<TStrong>(Guid Value) : StrongOf<Guid, TStrong>(Value), IComparable, IStrongGuid
    where TStrong : StrongGuid<TStrong>
{
    /// <summary>
    /// Initializes a new instance of the StrongGuid class using the specified string representation of a Guid.
    /// </summary>
    /// <param name="value">A string containing a Guid to use for initialization.</param>
    /// <remarks>Uses <see cref="Guid.Parse(string)"/></remarks>
    public StrongGuid(string value) : this(Guid.Parse(value)) { }

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
    /// Returns an empty Guid of the strong type.
    /// </summary>
    /// <returns>An empty Guid of the strong type.</returns>
    public static TStrong Empty()
        => From(Guid.Empty);

    /// <summary>
    /// Checks if the Guid is empty.
    /// </summary>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    public bool IsEmpty()
        => Value == Guid.Empty;

    /// <summary>
    /// Creates a new Guid of the strong type.
    /// </summary>
    /// <returns>A new Guid of the strong type.</returns>
    public static TStrong New()
         => From(Guid.NewGuid());

    /// <summary>
    /// Tries to parse a Guid from a string and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A string containing a Guid to convert.</param>
    /// <param name="strong">When this method returns, contains the Guid value equivalent to the Guid contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (Guid.TryParse(content, out Guid value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom format string.</param>
    /// <returns>The string representation of the value of this instance as specified by format.</returns>
    public string ToString(string format)
        => Value.ToString(format);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation with dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance with dashes.</returns>
    public string ToStringWithDashes()
        => Value.ToString("D");

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation without dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance without dashes.</returns>
    public string ToStringWithoutDashes()
        => Value.ToString("N");

    // Operators
    /// <summary>
    /// Determines whether two specified instances of StrongGuid are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="value">The second instance to compare.</param>
    /// <returns>True if strong and value represent the same Guid; otherwise, false.</returns>
    public static bool operator ==(StrongGuid<TStrong> strong, Guid value)
    {
        if (strong is null)
        {
            return true;
        }

        return strong.Value == value;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongGuid are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The second instance to compare.</param>
    /// <returns>True if strong and other do not represent the same Guid; otherwise, false.</returns>
    public static bool operator !=(StrongGuid<TStrong> strong, Guid other)
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
