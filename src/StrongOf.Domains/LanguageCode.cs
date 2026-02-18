// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed language code (e.g., "en", "de", "en-US").
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(LanguageCodeTypeConverter))]
public sealed partial class LanguageCode(string value) : StrongString<LanguageCode>(value)
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
}

/// <summary>
/// Type converter for <see cref="LanguageCode"/>.
/// </summary>
public sealed class LanguageCodeTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new LanguageCode(stringValue) : base.ConvertFrom(context, culture, value);
}
