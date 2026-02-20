// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongDateTime{TStrong}"/> type.
/// Supports conversion from <see cref="DateTime"/> and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-datetime type.</typeparam>
public class StrongDateTimeTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongDateTime<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(DateTime) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            DateTime dt => StrongOf<DateTime, TStrong>.From(dt),
            string s when DateTime.TryParse(s, culture, DateTimeStyles.RoundtripKind, out DateTime parsed)
                => StrongOf<DateTime, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
