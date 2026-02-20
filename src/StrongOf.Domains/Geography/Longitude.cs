// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a strongly-typed longitude value in decimal degrees.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [−180, +180].
/// Use <see cref="IsValidRange"/> to validate before use.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// Longitude lon = new(13.405m);
/// bool valid = lon.IsValidRange(); // true
/// Longitude clamped = new Longitude(300m).Clamp(); // 180
/// </code>
/// </example>
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
    /// Determines whether the longitude is within the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between −180 and +180 inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Clamps the longitude to the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns>A new <see cref="Longitude"/> whose value is within [−180, +180].</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Longitude Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}
