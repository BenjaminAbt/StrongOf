// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

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
[TypeConverter(typeof(StrongStringTypeConverter<Isbn>))]
public sealed partial class Isbn(string value) : StrongString<Isbn>(value), IValidatable
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Isbn? result)
    {
        if (value is not null)
        {
            Isbn candidate = new(value);
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
