// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongGuid{TStrong}"/> type.
/// Supports conversion from <see cref="Guid"/> and <see cref="string"/>.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-guid type.</typeparam>
public class StrongGuidTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongGuid<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            Guid g => StrongOf<Guid, TStrong>.From(g),
            string s when Guid.TryParse(s, out Guid parsed) => StrongOf<Guid, TStrong>.From(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
