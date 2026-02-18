// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed hex color value.
/// </summary>
/// <remarks>
/// <para>
/// Accepts 6-digit (<c>#RRGGBB</c>) and 8-digit (<c>#RRGGBBAA</c>) hex colors, with or without the leading <c>#</c>.
/// Use <see cref="Normalize"/> to ensure a consistent uppercase <c>#RRGGBB</c> representation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// ColorHex color = new("#ff5733");
/// bool valid = color.IsValidFormat(); // true
/// string norm = color.Normalize();    // "#FF5733"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<ColorHex>))]
public sealed partial class ColorHex(string value) : StrongString<ColorHex>(value), IValidatable
{
    [GeneratedRegex(@"^#?[0-9A-Fa-f]{6}([0-9A-Fa-f]{2})?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ColorHexRegex();

    /// <summary>
    /// Determines whether the hex color has a valid format.
    /// </summary>
    /// <returns><see langword="true"/> if the value matches <c>#RRGGBB</c> or <c>#RRGGBBAA</c> (with or without <c>#</c>); otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && ColorHexRegex().IsMatch(Value);

    /// <summary>
    /// Normalizes the color to uppercase and ensures a leading <c>#</c>.
    /// </summary>
    /// <returns>The color in uppercase with a leading <c>#</c>, e.g. <c>"#FF5733"</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Normalize()
    {
        string normalized = Value.StartsWith("#", StringComparison.Ordinal) ? Value : "#" + Value;
        return normalized.ToUpperInvariant();
    }
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out ColorHex? result)
    {
        if (value is not null)
        {
            ColorHex candidate = new(value);
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
