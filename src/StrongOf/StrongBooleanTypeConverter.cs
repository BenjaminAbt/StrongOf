// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongBoolean{TStrong}"/> type.
/// Supports conversion from <see cref="bool"/> and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-boolean type.</typeparam>
public class StrongBooleanTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongBoolean<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(bool) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            bool b => StrongOf<bool, TStrong>.From(b),
            string s when bool.TryParse(s, out bool parsed)
                => StrongOf<bool, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
