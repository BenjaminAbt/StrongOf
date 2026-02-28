// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Localization;

/// <summary>
/// Represents a strongly-typed time zone identifier (IANA or Windows zone ID).
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IsValidId"/> to verify the identifier is recognized on the current system.
/// Use <see cref="TryGetTimeZone"/> to obtain the corresponding <see cref="TimeZoneInfo"/>.
/// Note that valid IDs are platform-dependent (IANA on Linux/macOS, Windows IDs on Windows).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// TimeZoneId tz = new("Europe/Berlin");
/// bool isValid = tz.IsValidId();
/// if (tz.TryGetTimeZone(out TimeZoneInfo? info))
/// {
///     DateTimeOffset local = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, info);
/// }
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<TimeZoneId>))]
public sealed class TimeZoneId(string value) : StrongString<TimeZoneId>(value), IValidatable
{
    /// <summary>
    /// Validates whether the time zone identifier is recognized on the current system.
    /// </summary>
    /// <returns><see langword="true"/> if the identifier is valid; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => IsValidId();

    /// <summary>
    /// Determines whether the time zone identifier is valid on the current system.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="TimeZoneInfo.FindSystemTimeZoneById"/> succeeds; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidId()
        => TryGetTimeZone(out _);

    /// <summary>
    /// Tries to resolve the time zone identifier to a <see cref="TimeZoneInfo"/>.
    /// </summary>
    /// <param name="timeZone">
    /// When this method returns <see langword="true"/>, contains the resolved <see cref="TimeZoneInfo"/>;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if the identifier is recognized on the current system; otherwise, <see langword="false"/>.</returns>
    public bool TryGetTimeZone(out TimeZoneInfo? timeZone)
    {
        try
        {
            timeZone = TimeZoneInfo.FindSystemTimeZoneById(Value);
            return true;
        }
        catch
        {
            timeZone = null;
            return false;
        }
    }
}
