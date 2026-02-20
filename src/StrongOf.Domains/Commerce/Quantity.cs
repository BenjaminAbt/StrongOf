// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Commerce;

/// <summary>
/// Represents a strongly-typed quantity of items.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to represent countable quantities such as order line items, stock counts, or page counts.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var quantity = new Quantity(5);
/// bool isAvailable = quantity.Value > 0;
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongInt32TypeConverter<Quantity>))]
public sealed class Quantity(int value) : StrongInt32<Quantity>(value)
{
    /// <summary>
    /// Gets a <see cref="Quantity"/> representing zero.
    /// </summary>
    public static Quantity Zero => new(0);

    /// <summary>
    /// Determines whether this quantity is greater than zero.
    /// </summary>
    /// <returns><see langword="true"/> if the value is greater than 0; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsPositive()
        => Value > 0;

    /// <summary>
    /// Determines whether this quantity is zero.
    /// </summary>
    /// <returns><see langword="true"/> if the value equals 0; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsZero()
        => Value == 0;
}
