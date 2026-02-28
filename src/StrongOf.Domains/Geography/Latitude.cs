// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a strongly-typed latitude value in decimal degrees.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [−90, +90].
/// Use <see cref="IsValidRange"/> to validate before use.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// Latitude lat = new(52.52m);
/// bool valid = lat.IsValidRange(); // true
/// Latitude clamped = new Latitude(200m).Clamp(); // 90
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongDecimalTypeConverter<Latitude>))]
public sealed class Latitude(decimal value) : StrongDecimal<Latitude>(value), IValidatable
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
    /// Validates whether the latitude is within the valid range [−90, +90].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between −90 and +90 inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => IsValidRange();

    /// <inheritdoc cref="IsValidFormat"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Clamps the latitude to the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns>A new <see cref="Latitude"/> whose value is within [−90, +90].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Latitude Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
