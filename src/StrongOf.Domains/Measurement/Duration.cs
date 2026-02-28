// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Measurement;

/// <summary>
/// Represents a strongly-typed duration based on <see cref="TimeSpan"/>.
/// </summary>
/// <remarks>
/// <para>
/// Use this type for domain-specific durations such as processing time, response latency,
/// or any measured time interval.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// Duration duration = new(TimeSpan.FromMinutes(5));
/// double seconds = duration.TotalSeconds(); // 300
/// bool isPositive = duration.IsPositive(); // true
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongTimeSpanTypeConverter<Duration>))]
public sealed class Duration(TimeSpan value) : StrongTimeSpan<Duration>(value)
{
    /// <summary>
    /// Determines whether the duration is positive (greater than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <returns><see langword="true"/> if the value is positive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsPositive()
        => Value > TimeSpan.Zero;

    /// <summary>
    /// Determines whether the duration is zero.
    /// </summary>
    /// <returns><see langword="true"/> if the value is <see cref="TimeSpan.Zero"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsZero()
        => Value == TimeSpan.Zero;
}
