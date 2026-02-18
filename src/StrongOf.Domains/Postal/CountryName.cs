// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Postal;

/// <summary>
/// Represents a strongly-typed country name (e.g. "Germany", "United States").
/// </summary>
/// <remarks>
/// <para>
/// Valid country names are between <see cref="MinLength"/> and <see cref="MaxLength"/> characters
/// and consist only of Unicode letters, spaces, hyphens, and apostrophes.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// CountryName name = new("Germany");
/// bool valid = name.IsValidFormat(); // true
/// string upper = name.ToUpperCase(); // "GERMANY"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<CountryName>))]
public sealed partial class CountryName(string value) : StrongString<CountryName>(value), IValidatable
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
    /// <returns>
    /// <see langword="true"/> if the value is non-empty, within length bounds, and matches the
    /// allowed character pattern; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && Value.Length >= MinLength && Value.Length <= MaxLength && CountryNameRegex().IsMatch(Value);

    /// <summary>
    /// Returns the country name in uppercase.
    /// </summary>
    /// <returns>The country name converted to uppercase using the invariant culture.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out CountryName? result)
    {
        if (value is not null)
        {
            CountryName candidate = new(value);
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
