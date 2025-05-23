﻿// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;

namespace StrongOf;

/// <summary>
/// Represents a strongly typed character.
/// </summary>
/// <typeparam name="TStrong">The type of the strong character.</typeparam>
public abstract partial class StrongChar<TStrong>(char Value)
        : StrongOf<char, TStrong>(Value), IComparable, IStrongChar
    where TStrong : StrongChar<TStrong>
{
    /// <summary>
    /// Returns the value of the strong type as a char.
    /// </summary>
    public char AsChar() => Value;

    /// <summary>
    /// Creates a new instance of StrongChar from a nullable char value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongChar if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    public static TStrong? FromNullable(char? value)
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
