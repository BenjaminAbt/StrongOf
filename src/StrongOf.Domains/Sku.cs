// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed Stock Keeping Unit (SKU) / product code.
/// </summary>
/// <remarks>
/// <para>
/// A SKU is a unique identifier for a product variant used in inventory management.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var sku = new Sku("SHIRT-RED-L");
/// bool isValid = sku.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(SkuTypeConverter))]
public sealed partial class Sku(string value) : StrongString<Sku>(value)
{
    /// <summary>
    /// Regular expression pattern validating SKU format: alphanumeric characters and hyphens, 1–64 characters.
    /// </summary>
    [GeneratedRegex(@"^[A-Za-z0-9\-_]{1,64}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex SkuRegex();

    /// <summary>
    /// Validates whether the SKU has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && SkuRegex().IsMatch(Value);
}

/// <summary>
/// Type converter for <see cref="Sku"/>.
/// </summary>
public sealed class SkuTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Sku(s) : base.ConvertFrom(context, culture, value);
}
