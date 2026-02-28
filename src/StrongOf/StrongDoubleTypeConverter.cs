// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongDouble{TStrong}"/> type.
/// Supports conversion from <see cref="double"/>, <see cref="float"/>, <see cref="int"/>,
/// and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-double type.</typeparam>
public class StrongDoubleTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongDouble<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(double)
           || sourceType == typeof(float)
           || sourceType == typeof(int)
           || sourceType == typeof(string)
           || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            double d => StrongOf<double, TStrong>.From(d),
            float f => StrongOf<double, TStrong>.From(f),
            int i => StrongOf<double, TStrong>.From(i),
            string s when double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, culture ?? CultureInfo.InvariantCulture, out double parsed)
                => StrongOf<double, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
