// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongDecimal{TStrong}"/> type.
/// Supports conversion from <see cref="decimal"/>, <see cref="double"/>, <see cref="int"/>,
/// and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-decimal type.</typeparam>
public class StrongDecimalTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongDecimal<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(decimal)
           || sourceType == typeof(double)
           || sourceType == typeof(int)
           || sourceType == typeof(string)
           || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            decimal d => StrongOf<decimal, TStrong>.From(d),
            double d => StrongOf<decimal, TStrong>.From((decimal)d),
            int i => StrongOf<decimal, TStrong>.From(i),
            string s when decimal.TryParse(s, NumberStyles.Number, culture ?? CultureInfo.InvariantCulture, out decimal parsed)
                => StrongOf<decimal, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
