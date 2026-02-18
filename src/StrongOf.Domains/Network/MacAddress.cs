// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Network;

/// <summary>
/// Represents a strongly-typed MAC address.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a MAC (Media Access Control) address.
/// Supports formats: 00:11:22:33:44:55, 00-11-22-33-44-55, 001122334455
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var mac = new MacAddress("00:11:22:33:44:55");
/// bool isValid = mac.IsValidFormat();
/// string normalized = mac.GetNormalized();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(MacAddressTypeConverter))]
public sealed partial class MacAddress(string value) : StrongString<MacAddress>(value)
{
    /// <summary>
    /// Regular expression pattern for validating MAC addresses.
    /// Supports: 00:11:22:33:44:55, 00-11-22-33-44-55, 001122334455
    /// </summary>
    [GeneratedRegex(@"^([0-9A-Fa-f]{2}[:-]){5}[0-9A-Fa-f]{2}$|^[0-9A-Fa-f]{12}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex MacRegex();

    /// <summary>
    /// Validates whether the MAC address has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the MAC address format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var mac = new MacAddress("00:11:22:33:44:55");
    /// bool isValid = mac.IsValidFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && MacRegex().IsMatch(Value);

    /// <summary>
    /// Gets a normalized version of the MAC address (uppercase, colon-separated).
    /// </summary>
    /// <returns>The normalized MAC address in format 00:11:22:33:44:55.</returns>
    /// <example>
    /// <code>
    /// var mac = new MacAddress("00-11-22-33-44-55");
    /// string normalized = mac.GetNormalized(); // "00:11:22:33:44:55"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetNormalized()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return string.Empty;
        }

        // Remove separators and convert to uppercase
        string clean = Value.Replace(":", string.Empty, StringComparison.Ordinal)
                           .Replace("-", string.Empty, StringComparison.Ordinal)
                           .ToUpperInvariant();

        if (clean.Length != 12)
        {
            return Value;
        }

        // Format as XX:XX:XX:XX:XX:XX
        return string.Create(17, clean, (span, str) =>
        {
            int strIndex = 0;
            for (int i = 0; i < 17; i++)
            {
                if (i == 2 || i == 5 || i == 8 || i == 11 || i == 14)
                {
                    span[i] = ':';
                }
                else
                {
                    span[i] = str[strIndex++];
                }
            }
        });
    }

    /// <summary>
    /// Gets the MAC address without separators.
    /// </summary>
    /// <returns>The MAC address without separators.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetWithoutSeparators()
        => Value.Replace(":", string.Empty, StringComparison.Ordinal)
                .Replace("-", string.Empty, StringComparison.Ordinal)
                .ToUpperInvariant();
}

/// <summary>
/// Type converter for <see cref="MacAddress"/>.
/// </summary>
public sealed class MacAddressTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new MacAddress(stringValue) : base.ConvertFrom(context, culture, value);
}
