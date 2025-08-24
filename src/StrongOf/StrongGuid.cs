// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strong type of Guid.
/// </summary>
/// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
public abstract partial class StrongGuid<TStrong>(Guid Value)
        : StrongOf<Guid, TStrong>(Value), IComparable, IStrongGuid
    where TStrong : StrongGuid<TStrong>
{
    /// <summary>
    /// Returns the value of the strong type as a Guid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Guid AsGuid() => Value;

    /// <summary>
    /// Converts a nullable Guid value to a strong type of Guid.
    /// </summary>
    /// <param name="value">The nullable Guid value to convert.</param>
    /// <returns>A strong type of Guid if the value has a Guid; otherwise, null.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong? FromGuid(Guid? value)
    {
        if (value.HasValue)
        {
            TStrong strong = From(value.Value);
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Converts a string to a strong type of Guid.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>A strong type of Guid if the string can be parsed to a Guid; otherwise, null.</returns>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong? FromString(string? value)
    {
        if (value is not null && Guid.TryParse(value, out Guid result))
        {
            TStrong strong = From(result);
            return strong;
        }

        return null;
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
    /// Returns an empty Guid of the strong type.
    /// </summary>
    /// <returns>An empty Guid of the strong type.</returns>
    private static readonly TStrong s_empty = From(Guid.Empty);

    /// <summary>
    /// Returns an empty Guid of the strong type.
    /// </summary>
    /// <returns>A cached empty Guid of the strong type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong Empty()
        => s_empty;

    /// <summary>
    /// Checks if the Guid is empty.
    /// </summary>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty()
        => Value == Guid.Empty;

    /// <summary>
    /// Creates a new Guid of the strong type.
    /// </summary>
    /// <returns>A new Guid of the strong type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong New()
         => From(Guid.NewGuid());

    /// <summary>
    /// Tries to parse a Guid from a string and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="content">A string containing a Guid to convert.</param>
    /// <param name="strong">When this method returns, contains the Guid value equivalent to the Guid contained in content, if the conversion succeeded, or null if the conversion failed.</param>
    /// <returns>True if content was converted successfully; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format)
        => Value.ToString(format);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation with dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance with dashes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToStringWithDashes()
        => Value.ToString("D");

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation without dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance without dashes.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToStringWithoutDashes()
        => Value.ToString("N");

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
