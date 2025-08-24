// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using StrongOf.Factories;
using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0049 // Type name should not match containing namespace

/// <summary>
/// Represents a strong type of TTarget.
/// </summary>
/// <typeparam name="TTarget">The type of the target.</typeparam>
/// <typeparam name="TStrong">The type of the strong.</typeparam>
/// <remarks>
/// Initializes a new instance of the StrongOf class.
/// </remarks>
/// <param name="value">The value of the strong type.</param>
public abstract class StrongOf<TTarget, TStrong>(TTarget value)
        : IStrongOf, IEquatable<StrongOf<TTarget, TStrong>>
    where TStrong : StrongOf<TTarget, TStrong>
{
    private static readonly Func<TTarget, TStrong> s_factoryWithParameter;
    private static readonly EqualityComparer<TTarget> s_comparer = EqualityComparer<TTarget>.Default;

    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    public TTarget Value { get; } = value;

    static StrongOf()
    {
        s_factoryWithParameter = StrongOfInstanceFactory.CreateWithOneParameterDelegate<TStrong, TTarget>();
    }

    // From

    /// <summary>
    /// Creates a new instance of the strong type from the specified value.
    /// </summary>
    /// <param name="value">The value to create the strong type from.</param>
    /// <returns>A new instance of the strong type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong From(TTarget value)
    {
        return s_factoryWithParameter(value);
    }

    /// <summary>
    /// Creates a list of strong types from the specified source.
    /// </summary>
    /// <param name="source">The source to create the list of strong types from.</param>
    /// <returns>A list of strong types.</returns>
    [return: NotNullIfNotNull(nameof(source))]
    public static List<TStrong>? From(IEnumerable<TTarget>? source)
    {
        if (source is null)
        {
            return null;
        }

        // Pre-size list when possible and avoid LINQ iterator allocations
        if (source is ICollection<TTarget> c)
        {
            List<TStrong> list = new List<TStrong>(c.Count);
            foreach (TTarget item in c)
            {
                list.Add(From(item));
            }

            return list;
        }

        if (source is IReadOnlyCollection<TTarget> roc)
        {
            List<TStrong> list = new List<TStrong>(roc.Count);
            foreach (TTarget item in source)
            {
                list.Add(From(item));
            }

            return list;
        }

        {
            List<TStrong> list = new List<TStrong>();
            foreach (TTarget item in source)
            {
                list.Add(From(item));
            }

            return list;
        }
    }

    // Operators

    /// <summary>
    /// Determines whether two specified instances of StrongOf are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other represent the same value; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(StrongOf<TTarget, TStrong>? strong, object? other)
    {
        if (strong is null)
        {
            return other is null;
        }

        if (other is TTarget targetValue)
        {
            return s_comparer.Equals(strong.Value, targetValue);
        }

        if (other is StrongOf<TTarget, TStrong> otherStrong)
        {
            return s_comparer.Equals(strong.Value, otherStrong.Value);
        }

        return false;
    }

    /// <summary>
    /// Determines whether two specified instances of StrongOf are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns>True if strong and other do not represent the same value; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(StrongOf<TTarget, TStrong> strong, object? other)
    {
        return (strong == other) is false;
    }

    // Equals

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is StrongOf<TTarget, TStrong> strong)
        {
            return s_comparer.Equals(Value, strong.Value);
        }

        if (other is TTarget target)
        {
            return s_comparer.Equals(Value, target);
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified StrongOf is equal to the current StrongOf.
    /// </summary>
    /// <param name="other">The StrongOf to compare with the current StrongOf.</param>
    /// <returns>True if the specified StrongOf is equal to the current StrongOf; otherwise, false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual bool Equals(StrongOf<TTarget, TStrong>? other)
    {
        if (other is null)
        {
            return false;
        }

        return s_comparer.Equals(Value, other.Value);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return s_comparer.GetHashCode(Value!);
    }

    // ToString

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Value!.ToString()!;
    }
}
