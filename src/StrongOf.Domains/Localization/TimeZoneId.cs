// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Localization;

/// <summary>
/// Represents a strongly-typed time zone identifier.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<TimeZoneId>))]
public sealed class TimeZoneId(string value) : StrongString<TimeZoneId>(value)
{
    /// <summary>
    /// Determines whether the time zone identifier is valid on the current system.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidId()
        => TryGetTimeZone(out _);

    /// <summary>
    /// Tries to resolve the time zone identifier.
    /// </summary>
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
