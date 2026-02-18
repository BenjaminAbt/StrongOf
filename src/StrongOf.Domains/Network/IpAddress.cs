// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace StrongOf.Domains.Network;

/// <summary>
/// Represents a strongly-typed IP address (v4 or v6).
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing an IP address.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var ip = new IpAddress("192.168.1.1");
/// bool isValid = ip.IsValidFormat();
/// bool isV4 = ip.IsIPv4();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<IpAddress>))]
public sealed class IpAddress(string value) : StrongString<IpAddress>(value), IValidatable
{
    /// <summary>
    /// Validates whether the IP address has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the IP address format is valid; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var ip = new IpAddress("192.168.1.1");
    /// bool isValid = ip.IsValidFormat(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => IPAddress.TryParse(Value, out _);

    /// <summary>
    /// Determines whether the IP address is IPv4.
    /// </summary>
    /// <returns><c>true</c> if the IP address is IPv4; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var ip = new IpAddress("192.168.1.1");
    /// bool isV4 = ip.IsIPv4(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsIPv4()
        => IPAddress.TryParse(Value, out IPAddress? ip) &&
           ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;

    /// <summary>
    /// Determines whether the IP address is IPv6.
    /// </summary>
    /// <returns><c>true</c> if the IP address is IPv6; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var ip = new IpAddress("::1");
    /// bool isV6 = ip.IsIPv6(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsIPv6()
        => IPAddress.TryParse(Value, out IPAddress? ip) &&
           ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;

    /// <summary>
    /// Determines whether the IP address is a loopback address.
    /// </summary>
    /// <returns><c>true</c> if the IP address is a loopback address; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsLoopback()
        => IPAddress.TryParse(Value, out IPAddress? ip) && IPAddress.IsLoopback(ip);

    /// <summary>
    /// Converts the IP address string to an <see cref="IPAddress"/> object.
    /// </summary>
    /// <returns>An <see cref="IPAddress"/> object, or <c>null</c> if the IP address is invalid.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public IPAddress? ToIPAddress()
        => IPAddress.TryParse(Value, out IPAddress? ip) ? ip : null;
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out IpAddress? result)
    {
        if (value is not null)
        {
            IpAddress candidate = new(value);
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
