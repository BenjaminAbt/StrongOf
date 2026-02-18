// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed ISO 3166-1 alpha-2 country code.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a two-letter country code (e.g., "US", "DE", "GB").
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var country = new CountryCode("US");
/// bool isValid = country.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<CountryCode>))]
public sealed class CountryCode(string value) : StrongString<CountryCode>(value), IValidatable
{
    /// <summary>
    /// The required length for a valid ISO 3166-1 alpha-2 country code.
    /// </summary>
    public const int RequiredLength = 2;

    /// <summary>
    /// Validates whether the country code has a valid format (exactly 2 uppercase letters).
    /// </summary>
    /// <returns><c>true</c> if the country code format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var country = new CountryCode("US");
    /// bool isValid = country.IsValidFormat(); // true
    ///
    /// var invalid = new CountryCode("USA");
    /// bool isInvalid = invalid.IsValidFormat(); // false (wrong length)
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
    {
        if (string.IsNullOrWhiteSpace(Value) || Value.Length != RequiredLength)
        {
            return false;
        }

        return char.IsLetter(Value[0]) && char.IsLetter(Value[1]);
    }

    /// <summary>
    /// Gets the country code in uppercase format.
    /// </summary>
    /// <returns>The country code in uppercase.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because CountryCode is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(CountryCode? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is CountryCode other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(CountryCode?)"/>.</remarks>
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out CountryCode? result)
    {
        if (value is not null)
        {
            CountryCode candidate = new(value);
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
