// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed network port number.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps an integer value representing a network port (0-65535).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var port = new Port(443);
/// bool isWellKnown = port.IsWellKnownPort();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(PortTypeConverter))]
public sealed class Port(int value) : StrongInt32<Port>(value)
{
    /// <summary>
    /// The minimum valid port number.
    /// </summary>
    public const int MinValue = 0;

    /// <summary>
    /// The maximum valid port number.
    /// </summary>
    public const int MaxValue = 65535;

    /// <summary>
    /// The upper bound for well-known ports (0-1023).
    /// </summary>
    public const int WellKnownPortMax = 1023;

    /// <summary>
    /// The upper bound for registered ports (1024-49151).
    /// </summary>
    public const int RegisteredPortMax = 49151;

    /// <summary>
    /// Common HTTP port.
    /// </summary>
    public static Port Http => new(80);

    /// <summary>
    /// Common HTTPS port.
    /// </summary>
    public static Port Https => new(443);

    /// <summary>
    /// Common SSH port.
    /// </summary>
    public static Port Ssh => new(22);

    /// <summary>
    /// Common FTP port.
    /// </summary>
    public static Port Ftp => new(21);

    /// <summary>
    /// Validates whether the port number is within the valid range (0-65535).
    /// </summary>
    /// <returns><c>true</c> if the port is within valid range; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Determines whether the port is a well-known port (0-1023).
    /// </summary>
    /// <returns><c>true</c> if the port is a well-known port; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var port = new Port(80);
    /// bool isWellKnown = port.IsWellKnownPort(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsWellKnownPort()
        => Value >= MinValue && Value <= WellKnownPortMax;

    /// <summary>
    /// Determines whether the port is a registered port (1024-49151).
    /// </summary>
    /// <returns><c>true</c> if the port is a registered port; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsRegisteredPort()
        => Value > WellKnownPortMax && Value <= RegisteredPortMax;

    /// <summary>
    /// Determines whether the port is a dynamic/private port (49152-65535).
    /// </summary>
    /// <returns><c>true</c> if the port is a dynamic port; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsDynamicPort()
        => Value > RegisteredPortMax && Value <= MaxValue;
}

/// <summary>
/// Type converter for <see cref="Port"/>.
/// </summary>
public sealed class PortTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        return value switch
        {
            int i => new Port(i),
            string s when int.TryParse(s, System.Globalization.NumberStyles.Integer, culture, out int parsed) => new Port(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
