// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

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
[TypeConverter(typeof(IpAddressTypeConverter))]
public sealed class IpAddress(string value) : StrongString<IpAddress>(value)
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
}

/// <summary>
/// Type converter for <see cref="IpAddress"/>.
/// </summary>
public sealed class IpAddressTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new IpAddress(stringValue) : base.ConvertFrom(context, culture, value);
}
