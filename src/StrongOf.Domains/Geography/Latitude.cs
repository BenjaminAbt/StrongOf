// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a strongly-typed latitude value.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongDecimalTypeConverter<Latitude>))]
public sealed class Latitude(decimal value) : StrongDecimal<Latitude>(value)
{
    /// <summary>
    /// Minimum valid latitude.
    /// </summary>
    public const decimal MinValue = -90m;

    /// <summary>
    /// Maximum valid latitude.
    /// </summary>
    public const decimal MaxValue = 90m;

    /// <summary>
    /// Determines whether the latitude is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Clamps the latitude to the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Latitude Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
