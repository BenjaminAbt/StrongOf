// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed network host name.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a network host name (DNS name).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var host = new HostName("www.example.com");
/// bool isValid = host.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(HostNameTypeConverter))]
public sealed partial class HostName(string value) : StrongString<HostName>(value)
{
    /// <summary>
    /// Maximum length for a valid hostname.
    /// </summary>
    public const int MaxLength = 253;

    /// <summary>
    /// Maximum length for a single label (part between dots).
    /// </summary>
    public const int MaxLabelLength = 63;

    /// <summary>
    /// Regular expression pattern for validating hostnames.
    /// </summary>
    [GeneratedRegex(@"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.[A-Za-z0-9-]{1,63})*$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex HostNameRegex();

    /// <summary>
    /// Validates whether the hostname has a valid format according to RFC 1123.
    /// </summary>
    /// <returns><c>true</c> if the hostname format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var host = new HostName("www.example.com");
    /// bool isValid = host.IsValidFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) &&
           Value.Length <= MaxLength &&
           HostNameRegex().IsMatch(Value);

    /// <summary>
    /// Gets the hostname in lowercase format.
    /// </summary>
    /// <returns>The hostname in lowercase.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToLowerCase()
        => Value.ToLowerInvariant();

    /// <summary>
    /// Gets the top-level domain (TLD) of the hostname.
    /// </summary>
    /// <returns>The TLD, or an empty string if not found.</returns>
    /// <example>
    /// <code>
    /// var host = new HostName("www.example.com");
    /// string tld = host.GetTopLevelDomain(); // "com"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetTopLevelDomain()
    {
        int lastDot = Value.LastIndexOf('.');
        return lastDot >= 0 ? Value[(lastDot + 1)..] : string.Empty;
    }
}

/// <summary>
/// Type converter for <see cref="HostName"/>.
/// </summary>
public sealed class HostNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new HostName(stringValue) : base.ConvertFrom(context, culture, value);
}
