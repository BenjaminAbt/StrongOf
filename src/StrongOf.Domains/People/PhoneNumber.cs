// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed phone number.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a phone number.
/// It supports various international formats including country codes.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var phone = new PhoneNumber("+1-555-123-4567");
/// string normalized = phone.GetNormalized();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<PhoneNumber>))]
public sealed partial class PhoneNumber(string value) : StrongString<PhoneNumber>(value), IValidatable
{
    /// <summary>
    /// Regular expression pattern for validating phone numbers.
    /// Supports formats like: +1-555-123-4567, (555) 123-4567, 555.123.4567, +49 123 456789
    /// </summary>
    [GeneratedRegex(@"^[\+]?[(]?[0-9]{1,4}[)]?[-\s\.]?[(]?[0-9]{1,4}[)]?[-\s\.]?[0-9]{1,9}([-\s\.]?[0-9]{1,9})*$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex PhoneRegex();

    /// <summary>
    /// Validates whether the phone number has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the phone number format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var phone = new PhoneNumber("+1-555-123-4567");
    /// bool isValid = phone.IsValidFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && PhoneRegex().IsMatch(Value);

    /// <summary>
    /// Gets a normalized version of the phone number containing only digits and optional leading +.
    /// </summary>
    /// <returns>A normalized phone number string.</returns>
    /// <example>
    /// <code>
    /// var phone = new PhoneNumber("+1-555-123-4567");
    /// string normalized = phone.GetNormalized(); // "+15551234567"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetNormalized()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return string.Empty;
        }

        bool hasPlus = Value.StartsWith('+');
        string digitsOnly = DigitsOnlyRegex().Replace(Value, string.Empty);
        return hasPlus ? "+" + digitsOnly : digitsOnly;
    }

    [GeneratedRegex(@"[^\d]", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex DigitsOnlyRegex();
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out PhoneNumber? result)
    {
        if (value is not null)
        {
            PhoneNumber candidate = new(value);
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
