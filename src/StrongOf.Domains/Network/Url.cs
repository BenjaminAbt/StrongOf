// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Network;

/// <summary>
/// Represents a strongly-typed URL.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a URL/URI.
/// It provides methods to validate and parse URL components.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var url = new Url("https://example.com/path?query=value");
/// bool isValid = url.IsValidFormat();
/// string host = url.GetHost();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Url>))]
public sealed class Url(string value) : StrongString<Url>(value), IValidatable
{
    /// <summary>
    /// Validates whether the URL has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the URL format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var url = new Url("https://example.com");
    /// bool isValid = url.IsValidFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => Uri.TryCreate(Value, UriKind.Absolute, out Uri? uri) &&
           (string.Equals(uri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Validates whether the URL is a valid absolute URI.
    /// </summary>
    /// <returns><c>true</c> if the URL is a valid absolute URI; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsAbsoluteUri()
        => Uri.TryCreate(Value, UriKind.Absolute, out _);

    /// <summary>
    /// Gets the host part of the URL.
    /// </summary>
    /// <returns>The host, or an empty string if the URL is invalid.</returns>
    /// <example>
    /// <code>
    /// var url = new Url("https://example.com/path");
    /// string host = url.GetHost(); // "example.com"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetHost()
        => Uri.TryCreate(Value, UriKind.Absolute, out Uri? uri) ? uri.Host : string.Empty;

    /// <summary>
    /// Gets the scheme (protocol) of the URL.
    /// </summary>
    /// <returns>The scheme (e.g., "https"), or an empty string if the URL is invalid.</returns>
    /// <example>
    /// <code>
    /// var url = new Url("https://example.com");
    /// string scheme = url.GetScheme(); // "https"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetScheme()
        => Uri.TryCreate(Value, UriKind.Absolute, out Uri? uri) ? uri.Scheme : string.Empty;

    /// <summary>
    /// Gets the path component of the URL.
    /// </summary>
    /// <returns>The path, or an empty string if the URL is invalid.</returns>
    /// <example>
    /// <code>
    /// var url = new Url("https://example.com/api/users");
    /// string path = url.GetPath(); // "/api/users"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetPath()
        => Uri.TryCreate(Value, UriKind.Absolute, out Uri? uri) ? uri.AbsolutePath : string.Empty;

    /// <summary>
    /// Converts the URL string to a <see cref="Uri"/> object.
    /// </summary>
    /// <returns>A <see cref="Uri"/> object, or <c>null</c> if the URL is invalid.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Uri? ToUri()
        => Uri.TryCreate(Value, UriKind.Absolute, out Uri? uri) ? uri : null;
}
