// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed temperature in Celsius.
/// </summary>
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
    /// Determines whether the temperature is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the temperature to Fahrenheit.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToFahrenheit()
        => (Value * 9m / 5m) + 32m;

    /// <summary>
    /// Converts the temperature to Kelvin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToKelvin()
        => Value + 273.15m;
}
