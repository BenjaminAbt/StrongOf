// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed International Standard Book Number (ISBN).
/// Supports both ISBN-10 and ISBN-13 formats.
/// </summary>
/// <remarks>
/// <para>
/// ISBN-10: 10 digits with optional hyphens (e.g., "0-306-40615-2").
/// ISBN-13: 13 digits starting with 978 or 979 (e.g., "978-0-306-40615-7").
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var isbn = new Isbn("978-0-306-40615-7");
/// bool isValid = isbn.IsValidFormat();
/// bool is13 = isbn.IsIsbn13();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(IsbnTypeConverter))]
public sealed partial class Isbn(string value) : StrongString<Isbn>(value)
{
    /// <summary>
    /// Regular expression for ISBN-10.
    /// </summary>
    [GeneratedRegex(@"^(?:\d[\ |-]?){9}[\d|X]$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex Isbn10Regex();

    /// <summary>
    /// Regular expression for ISBN-13.
    /// </summary>
    [GeneratedRegex(@"^(?:97[89][\ |-]?)(?:\d[\ |-]?){9}\d$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex Isbn13Regex();

    /// <summary>
    /// Validates whether the value matches ISBN-10 or ISBN-13 format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && (Isbn10Regex().IsMatch(Value) || Isbn13Regex().IsMatch(Value));

    /// <summary>
    /// Determines whether this is an ISBN-13 number.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsIsbn13()
        => !string.IsNullOrWhiteSpace(Value) && Isbn13Regex().IsMatch(Value);

    /// <summary>
    /// Determines whether this is an ISBN-10 number.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsIsbn10()
        => !string.IsNullOrWhiteSpace(Value) && Isbn10Regex().IsMatch(Value);

    /// <summary>
    /// Returns the ISBN with all hyphens and spaces removed.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToNormalizedString()
        => Value.Replace("-", "").Replace(" ", "");
}

/// <summary>
/// Type converter for <see cref="Isbn"/>.
/// </summary>
public sealed class IsbnTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Isbn(s) : base.ConvertFrom(context, culture, value);
}
