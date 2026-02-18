// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
[TypeConverter(typeof(CurrencyCodeTypeConverter))]
public sealed class CurrencyCode(string value) : StrongString<CurrencyCode>(value)
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
}

/// <summary>
/// Type converter for <see cref="CurrencyCode"/>.
/// </summary>
public sealed class CurrencyCodeTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new CurrencyCode(stringValue) : base.ConvertFrom(context, culture, value);
}
