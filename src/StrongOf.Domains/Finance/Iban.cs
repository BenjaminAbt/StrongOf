// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
/// <para>
/// This type validates structural format only. Financial correctness checks like mod-97
/// checksum validation are intentionally left to domain/application policies.
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
[StrongString]
public sealed partial class Iban : IValidatable
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
    /// Full mod-97 checksum validation is intentionally not enforced here so callers can
    /// decide when deeper financial validation is required.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        // Normalize to canonical matching form before applying structural regex.
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
    /// <remarks>
    /// The grouping algorithm emits 4-character chunks and trims the last chunk length via
    /// <see cref="Math.Min(int, int)"/> to avoid overrun when the input length is not divisible by 4.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToFormattedString()
    {
        string normalized = Value.Replace(" ", "");
        // Build chunk indexes first, then slice each chunk deterministically.
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
