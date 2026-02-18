// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Identity;

/// <summary>
/// Represents a strongly-typed username.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a user account name.
/// Usernames typically contain alphanumeric characters, underscores, and hyphens.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var username = new Username("john_doe123");
/// bool isValid = username.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Username>))]
public sealed partial class Username(string value) : StrongString<Username>(value), IValidatable
{
    /// <summary>
    /// The minimum length for a valid username.
    /// </summary>
    public const int MinLength = 3;

    /// <summary>
    /// The maximum length for a valid username.
    /// </summary>
    public const int MaxLength = 64;

    /// <summary>
    /// Regular expression pattern for validating usernames.
    /// Allows alphanumeric characters, underscores, and hyphens.
    /// </summary>
    [GeneratedRegex(@"^[a-zA-Z0-9_-]+$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex UsernameRegex();

    /// <summary>
    /// Validates whether the username has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the username format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var username = new Username("john_doe123");
    /// bool isValid = username.IsValidFormat(); // true
    ///
    /// var invalid = new Username("ab");
    /// bool isInvalid = invalid.IsValidFormat(); // false (too short)
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) &&
           Value.Length >= MinLength &&
           Value.Length <= MaxLength &&
           UsernameRegex().IsMatch(Value);

    /// <summary>
    /// Gets the username in lowercase format.
    /// </summary>
    /// <returns>The username in lowercase.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToLowerCase()
        => Value.ToLowerInvariant();
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Username? result)
    {
        if (value is not null)
        {
            Username candidate = new(value);
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
