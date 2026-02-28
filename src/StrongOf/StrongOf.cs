// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using StrongOf.Factories;

namespace StrongOf;

#pragma warning disable MA0049 // Type name should not match containing namespace

/// <summary>
/// Represents a strongly-typed wrapper around a primitive value, providing compile-time type safety
/// and preventing parameter order mistakes in method signatures.
/// </summary>
/// <typeparam name="TTarget">The underlying primitive type being wrapped (e.g., <see cref="string"/>, <see cref="Guid"/>, <see cref="int"/>).</typeparam>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// This class uses the Curiously Recurring Template Pattern (CRTP) to enable type-safe factory methods
/// and equality comparisons. All derived types should be sealed classes with primary constructors.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer using <c>new()</c> for instantiation over <see cref="From(TTarget)"/>
/// as direct construction is ~40% faster due to avoiding delegate invocation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define a strongly-typed UserId
/// public sealed class UserId(Guid value) : StrongGuid&lt;UserId&gt;(value) { }
///
/// // Usage with compile-time safety
/// public void ProcessUser(UserId userId, TenantId tenantId)
/// {
///     // Compiler prevents mixing up userId and tenantId!
/// }
/// </code>
/// </example>
/// <param name="value">The underlying value to wrap.</param>
public abstract class StrongOf<TTarget, TStrong>(TTarget value)
        : IStrongOf<TTarget, TStrong>, IEquatable<StrongOf<TTarget, TStrong>>, IEquatable<TTarget>
    where TStrong : StrongOf<TTarget, TStrong>
{
    private static readonly Func<TTarget, TStrong> s_factoryWithParameter;
    private static readonly EqualityComparer<TTarget> s_comparer = EqualityComparer<TTarget>.Default;

    /// <summary>
    /// Gets the underlying primitive value.
    /// </summary>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.NewGuid());
    /// Guid rawGuid = userId.Value; // Access the underlying Guid
    /// </code>
    /// </example>
    public TTarget Value { get; } = value;

    static StrongOf()
    {
        s_factoryWithParameter = StrongOfInstanceFactory.CreateWithOneParameterDelegate<TStrong, TTarget>();
    }

    // From

    /// <summary>
    /// Creates a new instance of the strong type from the specified value using a cached factory delegate.
    /// </summary>
    /// <param name="value">The underlying value to wrap.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/> wrapping the specified value.</returns>
    /// <remarks>
    /// <para>
    /// This method uses a pre-compiled expression delegate for instantiation, making it suitable
    /// for generic scenarios where <c>new()</c> cannot be used directly.
    /// </para>
    /// <para>
    /// <b>Performance:</b> While optimized with cached delegates, direct instantiation with <c>new()</c>
    /// is still ~40% faster. Use <c>From()</c> only when working with generic code.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Direct instantiation (preferred, faster)
    /// var userId = new UserId(Guid.NewGuid());
    ///
    /// // Factory method (for generic scenarios)
    /// var userId = UserId.From(Guid.NewGuid());
    ///
    /// // Generic usage where From() is required
    /// public static TStrong CreateDefault&lt;TStrong, TValue&gt;(TValue value)
    ///     where TStrong : StrongOf&lt;TValue, TStrong&gt;
    /// {
    ///     return TStrong.From(value);
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong From(TTarget value)
    {
        return s_factoryWithParameter(value);
    }

    /// <summary>
    /// Creates a new instance of the strong type from the specified value.
    /// This is the <see cref="IStrongOf{TTarget, TSelf}"/> interface implementation
    /// that enables compile-time safe factory usage in generic code.
    /// </summary>
    /// <param name="value">The underlying value to wrap.</param>
    /// <returns>A new instance of <typeparamref name="TStrong"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    static TStrong IStrongOf<TTarget, TStrong>.Create(TTarget value)
    {
        return s_factoryWithParameter(value);
    }

    /// <summary>
    /// Creates a list of strong types from the specified collection of underlying values.
    /// </summary>
    /// <param name="source">The source collection of values to convert. Can be <c>null</c>.</param>
    /// <returns>
    /// A new <see cref="List{T}"/> containing the converted strong types,
    /// or <c>null</c> if the source is <c>null</c>.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method optimizes memory allocation by pre-sizing the result list when the source
    /// implements <see cref="ICollection{T}"/> or <see cref="IReadOnlyCollection{T}"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// Guid[] guids = [Guid.NewGuid(), Guid.NewGuid()];
    /// List&lt;UserId&gt;? userIds = UserId.From(guids);
    ///
    /// // Also works with null
    /// List&lt;UserId&gt;? nullResult = UserId.From((IEnumerable&lt;Guid&gt;?)null); // Returns null
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    [return: NotNullIfNotNull(nameof(source))]
    public static List<TStrong>? From(IEnumerable<TTarget>? source)
    {
        if (source is null)
        {
            return null;
        }

        // Fast path for arrays - most common case
        if (source is TTarget[] array)
        {
            List<TStrong> list = new(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                list.Add(From(array[i]));
            }
            return list;
        }

        // Fast path for List<T> - direct indexer access
        if (source is List<TTarget> sourceList)
        {
            List<TStrong> list = new(sourceList.Count);
            for (int i = 0; i < sourceList.Count; i++)
            {
                list.Add(From(sourceList[i]));
            }
            return list;
        }

        // Pre-size list when possible and avoid LINQ iterator allocations
        if (source is ICollection<TTarget> c)
        {
            List<TStrong> list = new(c.Count);
            foreach (TTarget item in c)
            {
                list.Add(From(item));
            }
            return list;
        }

        if (source is IReadOnlyCollection<TTarget> roc)
        {
            List<TStrong> list = new(roc.Count);
            foreach (TTarget item in source)
            {
                list.Add(From(item));
            }
            return list;
        }

        // Fallback for unknown enumerable types
        {
            List<TStrong> list = [];
            foreach (TTarget item in source)
            {
                list.Add(From(item));
            }
            return list;
        }
    }

    // Operators

    /// <summary>
    /// Determines whether two specified instances of <see cref="StrongOf{TTarget, TStrong}"/> are equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare. Can be a <typeparamref name="TTarget"/> value or another <see cref="StrongOf{TTarget, TStrong}"/>.</param>
    /// <returns><c>true</c> if both values are equal; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var id1 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// var id2 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    ///
    /// bool equal = id1 == id2; // true
    /// bool equalToRaw = id1 == Guid.Parse("550e8400-e29b-41d4-a716-446655440000"); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
    /// Determines whether two specified instances of <see cref="StrongOf{TTarget, TStrong}"/> are not equal.
    /// </summary>
    /// <param name="strong">The first instance to compare.</param>
    /// <param name="other">The object to compare.</param>
    /// <returns><c>true</c> if the values are not equal; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool operator !=(StrongOf<TTarget, TStrong>? strong, object? other)
    {
        return (strong == other) is false;
    }

    // Equals

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method supports comparison with both the underlying <typeparamref name="TTarget"/> type
    /// and other <see cref="StrongOf{TTarget, TStrong}"/> instances.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
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
    /// Determines whether the specified strong type is equal to the current instance.
    /// </summary>
    /// <param name="other">The strong type to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified strong type has the same underlying value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public virtual bool Equals(StrongOf<TTarget, TStrong>? other)
    {
        if (other is null)
        {
            return false;
        }

        return s_comparer.Equals(Value, other.Value);
    }

    /// <summary>
    /// Determines whether the specified underlying value is equal to this instance's value.
    /// </summary>
    /// <param name="other">The underlying value to compare.</param>
    /// <returns><c>true</c> if the values are equal; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// bool isEqual = userId.Equals(Guid.Parse("550e8400-e29b-41d4-a716-446655440000")); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(TTarget? other)
    {
        if (other is null)
        {
            return false;
        }

        return s_comparer.Equals(Value, other);
    }

    /// <summary>
    /// Returns a hash code for this instance based on the underlying value.
    /// </summary>
    /// <returns>A hash code for the current instance.</returns>
    /// <remarks>
    /// Two instances with equal underlying values will have the same hash code.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
    {
        return s_comparer.GetHashCode(Value!);
    }

    // ToString

    /// <summary>
    /// Returns the string representation of the underlying value.
    /// </summary>
    /// <returns>A string that represents the underlying value.</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// string str = userId.ToString(); // "550e8400-e29b-41d4-a716-446655440000"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override string ToString()
    {
        return Value!.ToString()!;
    }
}
