// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed quantity of items.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to represent countable quantities such as order line items, stock counts, or page counts.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var quantity = new Quantity(5);
/// bool isAvailable = quantity.Value > 0;
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(QuantityTypeConverter))]
public sealed class Quantity(int value) : StrongInt32<Quantity>(value)
{
    /// <summary>
    /// Gets a <see cref="Quantity"/> representing zero.
    /// </summary>
    public static Quantity Zero => new(0);

    /// <summary>
    /// Determines whether this quantity is greater than zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsPositive()
        => Value > 0;

    /// <summary>
    /// Determines whether this quantity is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsZero()
        => Value == 0;
}

/// <summary>
/// Type converter for <see cref="Quantity"/>.
/// </summary>
public sealed class QuantityTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value switch
        {
            int i => new Quantity(i),
            string s when int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int parsed) => new Quantity(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
