// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Network;

/// <summary>
/// Represents a strongly-typed email address.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing an email address.
/// It provides validation methods to check if the email format is valid.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var email = new EmailAddress("user@example.com");
/// bool isValid = email.IsValidFormat();
/// string domain = email.GetDomain();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<EmailAddress>))]
public sealed partial class EmailAddress(string value) : StrongString<EmailAddress>(value), IValidatable
{
    /// <summary>
    /// Regular expression pattern for validating email addresses.
    /// </summary>
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex EmailRegex();

    /// <summary>
    /// Validates whether the email address has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the email address format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var email = new EmailAddress("user@example.com");
    /// bool isValid = email.IsValidFormat(); // true
    ///
    /// var invalid = new EmailAddress("not-an-email");
    /// bool isInvalid = invalid.IsValidFormat(); // false
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && EmailRegex().IsMatch(Value);

    /// <summary>
    /// Gets the domain part of the email address.
    /// </summary>
    /// <returns>The domain part after the @ symbol, or an empty string if invalid.</returns>
    /// <example>
    /// <code>
    /// var email = new EmailAddress("user@example.com");
    /// string domain = email.GetDomain(); // "example.com"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetDomain()
    {
        int atIndex = Value.IndexOf('@');
        return atIndex >= 0 ? Value[(atIndex + 1)..] : string.Empty;
    }

    /// <summary>
    /// Gets the local part (username) of the email address.
    /// </summary>
    /// <returns>The local part before the @ symbol, or the entire value if no @ found.</returns>
    /// <example>
    /// <code>
    /// var email = new EmailAddress("user@example.com");
    /// string local = email.GetLocalPart(); // "user"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetLocalPart()
    {
        int atIndex = Value.IndexOf('@');
        return atIndex >= 0 ? Value[..atIndex] : Value;
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out EmailAddress? result)
    {
        if (value is not null)
        {
            EmailAddress candidate = new(value);
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
