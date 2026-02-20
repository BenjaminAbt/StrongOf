// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed weight in kilograms.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [<see cref="MinValue"/>, <see cref="MaxValue"/>] (0–500 kg).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// WeightKg weight = new(70m);
/// bool valid = weight.IsValidRange(); // true
/// decimal grams = weight.ToGrams(); // 70000
/// </code>
/// </example>
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
    /// Determines whether the weight is within the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between 0 and 500 kg inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the weight to grams.
    /// </summary>
    /// <returns>The weight value multiplied by 1000.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToGrams()
        => Value * 1000m;

    /// <summary>
    /// Clamps the weight to the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns>A new <see cref="WeightKg"/> whose value is within [0, 500].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public WeightKg Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
