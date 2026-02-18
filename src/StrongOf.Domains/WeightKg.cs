// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed weight in kilograms.
/// </summary>
[DebuggerDisplay("{Value} kg")]
[TypeConverter(typeof(WeightKgTypeConverter))]
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
    /// Determines whether the weight is within the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the weight to grams.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToGrams()
        => Value * 1000m;

    /// <summary>
    /// Clamps the weight to the valid range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public WeightKg Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}

/// <summary>
/// Type converter for <see cref="WeightKg"/>.
/// </summary>
public sealed class WeightKgTypeConverter : TypeConverter
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
            decimal d => new WeightKg(d),
            double d => new WeightKg((decimal)d),
            int i => new WeightKg(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new WeightKg(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
