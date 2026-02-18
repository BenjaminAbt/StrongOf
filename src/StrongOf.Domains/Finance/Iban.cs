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
[TypeConverter(typeof(StrongStringTypeConverter<Iban>))]
public sealed partial class Iban(string value) : StrongString<Iban>(value), IValidatable
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Iban? result)
    {
        if (value is not null)
        {
            Iban candidate = new(value);
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
