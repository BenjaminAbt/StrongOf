// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Finance;

/// <summary>
/// Represents a strongly-typed percentage value (0-100).
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a decimal value representing a percentage.
/// Values are expected to be in the range 0-100 (not 0-1).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var percentage = new Percentage(75.5m);
/// decimal fraction = percentage.ToFraction(); // 0.755
/// </code>
/// </example>
[DebuggerDisplay("{Value}%")]
[TypeConverter(typeof(PercentageTypeConverter))]
public sealed class Percentage(decimal value) : StrongDecimal<Percentage>(value)
{
    /// <summary>
    /// The minimum valid percentage value.
    /// </summary>
    public const decimal MinValue = 0m;

    /// <summary>
    /// The maximum valid percentage value.
    /// </summary>
    public const decimal MaxValue = 100m;

    /// <summary>
    /// Creates a <see cref="Percentage"/> from a fraction value (0-1).
    /// </summary>
    /// <param name="fraction">The fraction value (0-1).</param>
    /// <returns>A new <see cref="Percentage"/> instance.</returns>
    /// <example>
    /// <code>
    /// var percentage = Percentage.FromFraction(0.755m); // 75.5%
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Percentage FromFraction(decimal fraction)
        => new(fraction * 100m);

    /// <summary>
    /// Validates whether the percentage is within the valid range (0-100).
    /// </summary>
    /// <returns><c>true</c> if the percentage is within valid range; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var valid = new Percentage(75.5m);
    /// bool isValid = valid.IsValidRange(); // true
    ///
    /// var invalid = new Percentage(150m);
    /// bool isInvalid = invalid.IsValidRange(); // false
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Converts the percentage to a fraction (0-1).
    /// </summary>
    /// <returns>The percentage as a fraction.</returns>
    /// <example>
    /// <code>
    /// var percentage = new Percentage(75.5m);
    /// decimal fraction = percentage.ToFraction(); // 0.755
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public decimal ToFraction()
        => Value / 100m;

    /// <summary>
    /// Clamps the percentage to the valid range (0-100).
    /// </summary>
    /// <returns>A new <see cref="Percentage"/> clamped to the valid range.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Percentage Clamp()
        => new(Math.Clamp(Value, MinValue, MaxValue));
}

/// <summary>
/// Type converter for <see cref="Percentage"/>.
/// </summary>
public sealed class PercentageTypeConverter : TypeConverter
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
            decimal d => new Percentage(d),
            double d => new Percentage((decimal)d),
            int i => new Percentage(i),
            string s when decimal.TryParse(s, System.Globalization.NumberStyles.Number, culture, out decimal parsed) => new Percentage(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
