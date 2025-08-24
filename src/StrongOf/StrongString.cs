// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

/// <summary>
/// Represents a strong type of string.
/// </summary>
/// <typeparam name="TStrong">The type of the strong string.</typeparam>
public abstract partial class StrongString<TStrong>(string Value)
        : StrongOf<string, TStrong>(Value), IComparable, IStrongString
    where TStrong : StrongString<TStrong>
{
    /// <summary>
    /// Returns the value of the strong type as a string.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string AsString() => Value;

    /// <summary>
    /// Creates a new instance of StrongString from a nullable string value.
    /// </summary>
    /// <param name="value">The nullable char value.</param>
    /// <returns>A new instance of StrongString if the value has a value, null otherwise.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong? FromNullable(string? value)
    {
        if (value is not null)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TStrong FromTrimmed(string value)
        {
            string trimmed = value.Trim();
            return trimmed.Length == 0 ? Empty() : From(trimmed);
        }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    /// Creates a new instance of the strong string that is empty.
    /// </summary>
    /// <returns>A new instance of the strong string.</returns>
    private static readonly TStrong s_empty = From("");

    /// <summary>
    /// Creates a new instance of the strong string that is empty.
    /// </summary>
    /// <returns>A cached empty instance of the strong string.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong Empty()
        => s_empty;

    /// <summary>
    /// Determines whether the current instance is empty.
    /// </summary>
    /// <returns>True if the current instance is empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty()
        => Value is "";

    // Equals

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => base.GetHashCode();
}
