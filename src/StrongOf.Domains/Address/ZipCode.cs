// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed ZIP/postal code.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a postal or ZIP code.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var zipCode = new ZipCode("12345");
/// bool isValid = zipCode.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(ZipCodeTypeConverter))]
public sealed class ZipCode(string value) : StrongString<ZipCode>(value)
{
    /// <summary>
    /// Validates whether the ZIP code has a valid format (non-empty alphanumeric with optional spaces/hyphens).
    /// </summary>
    /// <returns><c>true</c> if the ZIP code format is valid; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        foreach (char c in Value)
        {
            if (!char.IsLetterOrDigit(c) && c != ' ' && c != '-')
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Validates whether the ZIP code has a valid US format (5 digits or 5+4 format).
    /// </summary>
    /// <returns><c>true</c> if the ZIP code is a valid US format; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var zip = new ZipCode("12345");
    /// bool isValid = zip.IsValidUsFormat(); // true
    ///
    /// var zip2 = new ZipCode("12345-6789");
    /// bool isValid2 = zip2.IsValidUsFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidUsFormat()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        // 5-digit format
        if (Value.Length == 5 && Value.All(char.IsDigit))
        {
            return true;
        }

        // 5+4 format (12345-6789)
        if (Value.Length == 10 && Value[5] == '-')
        {
            return Value[..5].All(char.IsDigit) && Value[6..].All(char.IsDigit);
        }

        return false;
    }

    /// <summary>
    /// Gets a normalized version of the ZIP code (uppercase, trimmed).
    /// </summary>
    /// <returns>The normalized ZIP code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetNormalized()
        => Value.Trim().ToUpperInvariant();
}

/// <summary>
/// Type converter for <see cref="ZipCode"/>.
/// </summary>
public sealed class ZipCodeTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new ZipCode(stringValue) : base.ConvertFrom(context, culture, value);
}
