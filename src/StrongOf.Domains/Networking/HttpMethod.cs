// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Networking;

/// <summary>
/// Represents a strongly-typed HTTP method string (e.g. GET, POST, PUT, DELETE).
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IsStandard"/> to verify that the value is one of the nine standard HTTP/1.1 methods.
/// The comparison is case-insensitive.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// HttpMethod method = new("GET");
/// bool isStandard = method.IsStandard(); // true
/// string upper = method.ToUpperCase();   // "GET"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<HttpMethod>))]
public sealed class HttpMethod(string value) : StrongString<HttpMethod>(value)
{
    private static readonly HashSet<string> s_standardMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS", "TRACE", "CONNECT"
    };

    /// <summary>
    /// Determines whether the HTTP method is a standard method.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the value is non-empty and equals one of:
    /// GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, TRACE, CONNECT (case-insensitive);
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsStandard()
        => !string.IsNullOrWhiteSpace(Value) && s_standardMethods.Contains(Value);

    /// <summary>
    /// Returns the method in uppercase.
    /// </summary>
    /// <returns>The HTTP method string converted to uppercase invariant culture.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();
}
