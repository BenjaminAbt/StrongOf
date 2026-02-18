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
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<ColorHex>))]
public sealed partial class ColorHex(string value) : StrongString<ColorHex>(value), IValidatable
{
    [GeneratedRegex(@"^#?[0-9A-Fa-f]{6}([0-9A-Fa-f]{2})?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex ColorHexRegex();

    /// <summary>
    /// Determines whether the hex color has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && ColorHexRegex().IsMatch(Value);

    /// <summary>
    /// Normalizes the color to uppercase and ensures a leading '#'.
    /// </summary>
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
