// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed street address.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a street address.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var street = new Street("123 Main Street");
/// bool isValid = street.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StreetTypeConverter))]
public sealed class Street(string value) : StrongString<Street>(value)
{
    /// <summary>
    /// Validates whether the street address has a valid format (non-empty).
    /// </summary>
    /// <returns><c>true</c> if the street address format is valid; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets a normalized version of the street address (trimmed).
    /// </summary>
    /// <returns>The normalized street address.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetNormalized()
        => Value.Trim();
}

/// <summary>
/// Type converter for <see cref="Street"/>.
/// </summary>
public sealed class StreetTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new Street(stringValue) : base.ConvertFrom(context, culture, value);
}
