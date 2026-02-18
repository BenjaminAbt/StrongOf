// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed MIME type.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(MimeTypeTypeConverter))]
public sealed partial class MimeType(string value) : StrongString<MimeType>(value)
{
    [GeneratedRegex(@"^[A-Za-z0-9!#$&^_.+-]+\/[A-Za-z0-9!#$&^_.+-]+$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex MimeTypeRegex();

    /// <summary>
    /// Determines whether the MIME type has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && MimeTypeRegex().IsMatch(Value);

    /// <summary>
    /// Gets the type part of the MIME type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetTypePart()
    {
        int slashIndex = Value.IndexOf('/');
        return slashIndex >= 0 ? Value[..slashIndex] : string.Empty;
    }

    /// <summary>
    /// Gets the subtype part of the MIME type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetSubtypePart()
    {
        int slashIndex = Value.IndexOf('/');
        return slashIndex >= 0 ? Value[(slashIndex + 1)..] : string.Empty;
    }
}

/// <summary>
/// Type converter for <see cref="MimeType"/>.
/// </summary>
public sealed class MimeTypeTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new MimeType(stringValue) : base.ConvertFrom(context, culture, value);
}
