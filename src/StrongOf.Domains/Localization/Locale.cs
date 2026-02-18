// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Localization;

/// <summary>
/// Represents a strongly-typed locale identifier (e.g. "en-US", "de-DE").
/// </summary>
/// <remarks>
/// <para>
/// Locales follow the IETF BCP 47 language tag format. Common examples: "en-US", "de-DE", "fr-FR", "zh-CN".
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var locale = new Locale("en-US");
/// bool isValid = locale.IsValidFormat();
/// var culture = locale.ToCultureInfo();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Locale>))]
public sealed partial class Locale(string value) : StrongString<Locale>(value), IValidatable
{
    /// <summary>
    /// Regular expression pattern validating BCP 47 locale format.
    /// </summary>
    [GeneratedRegex(@"^[a-zA-Z]{2,8}(-[a-zA-Z0-9]{2,8})*$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex LocaleRegex();

    /// <summary>
    /// Validates whether the locale has a valid BCP 47 format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && LocaleRegex().IsMatch(Value);

    /// <summary>
    /// Attempts to convert this locale to a <see cref="System.Globalization.CultureInfo"/> instance.
    /// </summary>
    /// <returns>The corresponding <see cref="System.Globalization.CultureInfo"/>, or <c>null</c> if invalid.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public System.Globalization.CultureInfo? ToCultureInfo()
    {
        try
        {
            return System.Globalization.CultureInfo.GetCultureInfo(Value);
        }
        catch (System.Globalization.CultureNotFoundException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because Locale is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(Locale? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is Locale other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(Locale?)"/>.</remarks>
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Locale? result)
    {
        if (value is not null)
        {
            Locale candidate = new(value);
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
