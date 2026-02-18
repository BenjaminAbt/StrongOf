// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
[TypeConverter(typeof(SlugTypeConverter))]
public sealed partial class Slug(string value) : StrongString<Slug>(value)
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
}

/// <summary>
/// Type converter for <see cref="Slug"/>.
/// </summary>
public sealed class SlugTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Slug(s) : base.ConvertFrom(context, culture, value);
}
