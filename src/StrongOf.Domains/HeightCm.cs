// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed height in centimeters.
/// </summary>
[DebuggerDisplay("{Value} cm")]
[TypeConverter(typeof(HeightCmTypeConverter))]
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
    /// Determines whether the height is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the height to meters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToMeters()
        => Value / 100m;

    /// <summary>
    /// Clamps the height to the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public HeightCm Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}

/// <summary>
/// Type converter for <see cref="HeightCm"/>.
/// </summary>
public sealed class HeightCmTypeConverter : TypeConverter
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
            decimal d => new HeightCm(d),
            double d => new HeightCm((decimal)d),
            int i => new HeightCm(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new HeightCm(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
