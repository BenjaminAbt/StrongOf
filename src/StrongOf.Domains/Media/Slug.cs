// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed URL slug — a URL-friendly string identifier.
/// </summary>
/// <remarks>
/// <para>
/// Slugs are used in URLs to identify resources in a human-readable way, e.g. "my-blog-post".
/// Valid slugs contain only lowercase letters, digits, and hyphens.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var slug = new Slug("my-blog-post");
/// bool isValid = slug.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Slug>))]
public sealed partial class Slug(string value) : StrongString<Slug>(value), IValidatable
{
    /// <summary>
    /// Regular expression pattern validating slug format: lowercase letters, digits, and hyphens.
    /// </summary>
    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex SlugRegex();

    /// <summary>
    /// Validates whether the slug has a valid URL-friendly format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && SlugRegex().IsMatch(Value);
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Slug? result)
    {
        if (value is not null)
        {
            Slug candidate = new(value);
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
