// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

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
[TypeConverter(typeof(LocaleTypeConverter))]
public sealed partial class Locale(string value) : StrongString<Locale>(value)
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
}

/// <summary>
/// Type converter for <see cref="Locale"/>.
/// </summary>
public sealed class LocaleTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Locale(s) : base.ConvertFrom(context, culture, value);
}
