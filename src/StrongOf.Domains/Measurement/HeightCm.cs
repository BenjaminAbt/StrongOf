// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed height in centimeters.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [<see cref="MinValue"/>, <see cref="MaxValue"/>] (0–300 cm).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// HeightCm height = new(175m);
/// bool valid = height.IsValidRange(); // true
/// decimal meters = height.ToMeters(); // 1.75
/// </code>
/// </example>
[DebuggerDisplay("{Value} cm")]
[TypeConverter(typeof(StrongDecimalTypeConverter<HeightCm>))]
public sealed class HeightCm(decimal value) : StrongDecimal<HeightCm>(value)
{
    /// <summary>
    /// Minimum valid height in centimeters.
    /// </summary>
    public const decimal MinValue = 0m;

    /// <summary>
    /// Maximum valid height in centimeters.
    /// </summary>
    public const decimal MaxValue = 300m;

    /// <summary>
    /// Determines whether the height is within the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between 0 and 300 cm inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the height to meters.
    /// </summary>
    /// <returns>The height value divided by 100.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToMeters()
        => Value / 100m;

    /// <summary>
    /// Clamps the height to the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns>A new <see cref="HeightCm"/> whose value is within [0, 300].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public HeightCm Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
