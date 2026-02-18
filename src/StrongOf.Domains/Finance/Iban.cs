// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Finance;

/// <summary>
/// Represents a strongly-typed International Bank Account Number (IBAN).
/// </summary>
/// <remarks>
/// <para>
/// The IBAN format consists of a 2-letter country code, 2 check digits, and a BBAN (Basic Bank Account Number).
/// Maximum length is 34 characters.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var iban = new Iban("DE89370400440532013000");
/// bool isValid = iban.IsValidFormat();
/// string country = iban.GetCountryCode();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(IbanTypeConverter))]
public sealed partial class Iban(string value) : StrongString<Iban>(value)
{
    /// <summary>
    /// Regular expression pattern validating basic IBAN structure.
    /// </summary>
    [GeneratedRegex(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]{1,30}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex IbanRegex();

    /// <summary>
    /// Validates whether the IBAN has a valid structural format.
    /// </summary>
    /// <remarks>
    /// This checks the basic structure only (country code + digits + alphanumeric BBAN).
    /// For full mod-97 checksum validation, additional logic is required.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        string normalized = Value.Replace(" ", "").ToUpperInvariant();
        return IbanRegex().IsMatch(normalized);
    }

    /// <summary>
    /// Gets the 2-letter country code from the IBAN.
    /// </summary>
    /// <returns>The country code (e.g., "DE"), or an empty string if the value is too short.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetCountryCode()
        => Value.Length >= 2 ? Value[..2].ToUpperInvariant() : string.Empty;

    /// <summary>
    /// Returns the IBAN with spaces every 4 characters for readability.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToFormattedString()
    {
        string normalized = Value.Replace(" ", "");
        return string.Join(" ", Enumerable.Range(0, ((normalized.Length + 3) / 4))
            .Select(i => normalized.Substring(i * 4, Math.Min(4, normalized.Length - (i * 4)))));
    }
}

/// <summary>
/// Type converter for <see cref="Iban"/>.
/// </summary>
public sealed class IbanTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Iban(s) : base.ConvertFrom(context, culture, value);
}
