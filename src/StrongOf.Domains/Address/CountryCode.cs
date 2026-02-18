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
[TypeConverter(typeof(CountryCodeTypeConverter))]
public sealed class CountryCode(string value) : StrongString<CountryCode>(value)
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
}

/// <summary>
/// Type converter for <see cref="CountryCode"/>.
/// </summary>
public sealed class CountryCodeTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new CountryCode(stringValue) : base.ConvertFrom(context, culture, value);
}
