// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed country name.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(CountryNameTypeConverter))]
public sealed partial class CountryName(string value) : StrongString<CountryName>(value)
{
    /// <summary>
    /// Minimum length for a country name.
    /// </summary>
    public const int MinLength = 2;

    /// <summary>
    /// Maximum length for a country name.
    /// </summary>
    public const int MaxLength = 56;

    [GeneratedRegex(@"^[\p{L}][\p{L} '\-]{1,55}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex CountryNameRegex();

    /// <summary>
    /// Determines whether the country name has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && Value.Length >= MinLength && Value.Length <= MaxLength && CountryNameRegex().IsMatch(Value);

    /// <summary>
    /// Returns the country name in uppercase.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();
}

/// <summary>
/// Type converter for <see cref="CountryName"/>.
/// </summary>
public sealed class CountryNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new CountryName(stringValue) : base.ConvertFrom(context, culture, value);
}
