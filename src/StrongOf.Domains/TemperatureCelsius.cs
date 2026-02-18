// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed temperature in Celsius.
/// </summary>
[DebuggerDisplay("{Value} °C")]
[TypeConverter(typeof(TemperatureCelsiusTypeConverter))]
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

/// <summary>
/// Type converter for <see cref="TemperatureCelsius"/>.
/// </summary>
public sealed class TemperatureCelsiusTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(decimal) || sourceType == typeof(double) || sourceType == typeof(int) ||
           sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        return value switch
        {
            decimal d => new TemperatureCelsius(d),
            double d => new TemperatureCelsius((decimal)d),
            int i => new TemperatureCelsius(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new TemperatureCelsius(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
