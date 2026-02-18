// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a strongly-typed longitude value.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongDecimalTypeConverter<Longitude>))]
public sealed class Longitude(decimal value) : StrongDecimal<Longitude>(value)
{
    /// <summary>
    /// Minimum valid longitude.
    /// </summary>
    public const decimal MinValue = -180m;

    /// <summary>
    /// Maximum valid longitude.
    /// </summary>
    public const decimal MaxValue = 180m;

    /// <summary>
    /// Determines whether the longitude is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Clamps the longitude to the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Longitude Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
