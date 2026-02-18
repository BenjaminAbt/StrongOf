// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
[TypeConverter(typeof(UsernameTypeConverter))]
public sealed partial class Username(string value) : StrongString<Username>(value)
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
}

/// <summary>
/// Type converter for <see cref="Username"/>.
/// </summary>
public sealed class UsernameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new Username(stringValue) : base.ConvertFrom(context, culture, value);
}
