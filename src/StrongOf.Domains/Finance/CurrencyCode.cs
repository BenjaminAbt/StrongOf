// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace StrongOf.Domains.Finance;

/// <summary>
/// Represents a strongly-typed ISO 4217 currency code.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a three-letter currency code (e.g., "USD", "EUR", "GBP").
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var currency = new CurrencyCode("USD");
/// bool isValid = currency.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<CurrencyCode>))]
public sealed class CurrencyCode(string value) : StrongString<CurrencyCode>(value), IValidatable
{
    /// <summary>
    /// The required length for a valid ISO 4217 currency code.
    /// </summary>
    public const int RequiredLength = 3;

    /// <summary>
    /// Validates whether the currency code has a valid format (exactly 3 uppercase letters).
    /// </summary>
    /// <returns><c>true</c> if the currency code format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var currency = new CurrencyCode("USD");
    /// bool isValid = currency.IsValidFormat(); // true
    ///
    /// var invalid = new CurrencyCode("US");
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

        return char.IsLetter(Value[0]) && char.IsLetter(Value[1]) && char.IsLetter(Value[2]);
    }

    /// <summary>
    /// Gets the currency code in uppercase format.
    /// </summary>
    /// <returns>The currency code in uppercase.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because CurrencyCode is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(CurrencyCode? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is CurrencyCode other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(CurrencyCode?)"/>.</remarks>
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out CurrencyCode? result)
    {
        if (value is not null)
        {
            CurrencyCode candidate = new(value);
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
