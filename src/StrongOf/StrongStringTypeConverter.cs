// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;

namespace StrongOf;

/// <summary>
/// A reusable <see cref="TypeConverter"/> for any <see cref="StrongString{TStrong}"/> type.
/// Eliminates per-type boilerplate: add <c>[TypeConverter(typeof(StrongStringTypeConverter&lt;MyType&gt;))]</c>
/// to your domain class instead of writing a dedicated converter class.
/// </summary>
/// <typeparam name="TStrong">The concrete strong-string type.</typeparam>
public class StrongStringTypeConverter<TStrong> : TypeConverter
    where TStrong : StrongString<TStrong>
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value is string s ? StrongOf<string, TStrong>.From(s) : base.ConvertFrom(context, culture, value);
}
