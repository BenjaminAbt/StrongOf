// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed temperature in degrees Celsius.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [<see cref="MinValue"/>, <see cref="MaxValue"/>] (−273.15 to 1000 °C).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// TemperatureCelsius temp = new(100m);
/// bool valid = temp.IsValidRange(); // true
/// decimal fahrenheit = temp.ToFahrenheit(); // 212
/// decimal kelvin = temp.ToKelvin(); // 373.15
/// </code>
/// </example>
[DebuggerDisplay("{Value} °C")]
[TypeConverter(typeof(StrongDecimalTypeConverter<TemperatureCelsius>))]
public sealed class TemperatureCelsius(decimal value) : StrongDecimal<TemperatureCelsius>(value)
{
    /// <summary>
    /// Minimum valid temperature in Celsius.
    /// </summary>
    public const decimal MinValue = -273.15m;

    /// <summary>
    /// Maximum valid temperature in Celsius.
    /// </summary>
    public const decimal MaxValue = 1000m;

    /// <summary>
    /// Determines whether the temperature is within the valid range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between −273.15 and 1000 °C inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the temperature to degrees Fahrenheit.
    /// </summary>
    /// <returns><c>(Value × 9/5) + 32</c> as a <see cref="decimal"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToFahrenheit()
        => (Value * 9m / 5m) + 32m;

    /// <summary>
    /// Converts the temperature to Kelvin.
    /// </summary>
    /// <returns><c>Value + 273.15</c> as a <see cref="decimal"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToKelvin()
        => Value + 273.15m;
}
