// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongInt32{TStrong}"/> type.
/// Supports conversion from <see cref="int"/> and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-int32 type.</typeparam>
public class StrongInt32TypeConverter<TStrong> : TypeConverter
    where TStrong : StrongInt32<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            int i => StrongOf<int, TStrong>.From(i),
            string s when int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed)
                => StrongOf<int, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
