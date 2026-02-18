// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed hex color value.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(ColorHexTypeConverter))]
public sealed partial class ColorHex(string value) : StrongString<ColorHex>(value)
{
    [GeneratedRegex(@"^#?[0-9A-Fa-f]{6}([0-9A-Fa-f]{2})?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ColorHexRegex();

    /// <summary>
    /// Determines whether the hex color has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && ColorHexRegex().IsMatch(Value);

    /// <summary>
    /// Normalizes the color to uppercase and ensures a leading '#'.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Normalize()
    {
        string normalized = Value.StartsWith("#", StringComparison.Ordinal) ? Value : "#" + Value;
        return normalized.ToUpperInvariant();
    }
}

/// <summary>
/// Type converter for <see cref="ColorHex"/>.
/// </summary>
public sealed class ColorHexTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new ColorHex(stringValue) : base.ConvertFrom(context, culture, value);
}
