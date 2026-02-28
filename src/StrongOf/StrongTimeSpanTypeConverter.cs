// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A <see cref="TypeConverter"/> for <see cref="StrongTimeSpan{TStrong}"/> types.
/// Supports conversion from <see cref="TimeSpan"/>, <see cref="string"/>, and <see cref="long"/> (ticks).
/// </summary>
/// <typeparam name="TStrong">The concrete strong TimeSpan type.</typeparam>
public sealed class StrongTimeSpanTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongTimeSpan<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(TimeSpan)
        || sourceType == typeof(string)
        || sourceType == typeof(long)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            TimeSpan ts => StrongTimeSpan<TStrong>.From(ts),
            long ticks => StrongTimeSpan<TStrong>.From(TimeSpan.FromTicks(ticks)),
            string s when TimeSpan.TryParse(s, culture ?? CultureInfo.InvariantCulture, out TimeSpan parsed) => StrongTimeSpan<TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
