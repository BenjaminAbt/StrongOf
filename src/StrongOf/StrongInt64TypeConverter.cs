// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongInt64{TStrong}"/> type.
/// Supports conversion from <see cref="long"/> and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-int64 type.</typeparam>
public class StrongInt64TypeConverter<TStrong> : TypeConverter
    where TStrong : StrongInt64<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(long) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            long l => StrongOf<long, TStrong>.From(l),
            string s when long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsed)
                => StrongOf<long, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
