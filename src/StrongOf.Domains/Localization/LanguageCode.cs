// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Localization;

/// <summary>
/// Represents a strongly-typed language code (e.g., "en", "de", "en-US").
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<LanguageCode>))]
[StrongString]
public sealed partial class LanguageCode : IValidatable
{
    [GeneratedRegex(@"^[A-Za-z]{2,3}(?:-[A-Za-z]{2})?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex LanguageCodeRegex();

    /// <summary>
    /// Determines whether the language code has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && LanguageCodeRegex().IsMatch(Value);

    /// <summary>
    /// Returns the language code in lowercase.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToLowerCase()
        => Value.ToLowerInvariant();

    /// <summary>
    /// Determines whether this instance equals the specified <paramref name="other"/> value.
    /// </summary>
    /// <param name="other">The value to compare with this instance.</param>
    /// <returns><see langword="true"/> if the values are equal; otherwise, <see langword="false"/>.</returns>
    /// <remarks>Comparison is case-insensitive because LanguageCode is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(LanguageCode? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is LanguageCode other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(LanguageCode?)"/>.</remarks>
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out LanguageCode? result)
    {
        if (value is not null)
        {
            LanguageCode candidate = new(value);
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
