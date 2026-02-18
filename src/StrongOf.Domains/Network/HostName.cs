// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Network;

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
[TypeConverter(typeof(StrongStringTypeConverter<HostName>))]
public sealed partial class HostName(string value) : StrongString<HostName>(value), IValidatable
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

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because HostName is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(HostName? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is HostName other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(HostName?)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
        => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out HostName? result)
    {
        if (value is not null)
        {
            HostName candidate = new(value);
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
