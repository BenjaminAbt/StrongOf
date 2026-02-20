// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Networking;

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
[TypeConverter(typeof(StrongStringTypeConverter<MacAddress>))]
public sealed partial class MacAddress(string value) : StrongString<MacAddress>(value), IValidatable
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

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because MacAddress is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(MacAddress? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is MacAddress other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(MacAddress?)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
        => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    /// <summary>
    /// Tries to create a new instance if <paramref name="value"/> satisfies the format constraint.
    /// </summary>
    /// <param name="value">The input string to validate and wrap.</param>
    /// <param name="result">
    /// When this method returns, contains the created instance if the format is valid;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if the value is non-null and passes <see cref="IsValidFormat"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryCreate(string? value, [NotNullWhen(true)] out MacAddress? result)
    {
        if (value is not null)
        {
            MacAddress candidate = new(value);
            if (candidate.IsValidFormat())
            {
                result = candidate;
                return true;
            }
        }
        result = null;
        return false;
    }
}
