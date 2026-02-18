// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed longitude value.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(LongitudeTypeConverter))]
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

/// <summary>
/// Type converter for <see cref="Longitude"/>.
/// </summary>
public sealed class LongitudeTypeConverter : TypeConverter
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
            decimal d => new Longitude(d),
            double d => new Longitude((decimal)d),
            int i => new Longitude(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new Longitude(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
