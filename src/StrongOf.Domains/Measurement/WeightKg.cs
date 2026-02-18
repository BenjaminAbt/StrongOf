// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed weight in kilograms.
/// </summary>
[DebuggerDisplay("{Value} kg")]
[TypeConverter(typeof(StrongDecimalTypeConverter<WeightKg>))]
public sealed class WeightKg(decimal value) : StrongDecimal<WeightKg>(value)
{
    /// <summary>
    /// Minimum valid weight in kilograms.
    /// </summary>
    public const decimal MinValue = 0m;

    /// <summary>
    /// Maximum valid weight in kilograms.
    /// </summary>
    public const decimal MaxValue = 500m;

    /// <summary>
    /// Determines whether the weight is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the weight to grams.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToGrams()
        => Value * 1000m;

    /// <summary>
    /// Clamps the weight to the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public WeightKg Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
