// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Geography;

/// <summary>
/// Represents a strongly-typed latitude value.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(LatitudeTypeConverter))]
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

/// <summary>
/// Type converter for <see cref="Latitude"/>.
/// </summary>
public sealed class LatitudeTypeConverter : TypeConverter
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
            decimal d => new Latitude(d),
            double d => new Latitude((decimal)d),
            int i => new Latitude(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new Latitude(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
