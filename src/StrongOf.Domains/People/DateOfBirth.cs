// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed date of birth.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a <see cref="DateTime"/> value representing a date of birth.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var dob = new DateOfBirth(new DateTime(1990, 5, 12));
/// bool isPast = dob.IsInPast();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongDateTimeTypeConverter<DateOfBirth>))]
public sealed class DateOfBirth(DateTime value) : StrongDateTime<DateOfBirth>(value)
{
    /// <summary>
    /// Determines whether the date of birth is in the past (UTC).
    /// </summary>
    /// <returns><see langword="true"/> if the value is before <see cref="DateTime.UtcNow"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsInPast()
        => Value < DateTime.UtcNow;

    /// <summary>
    /// Determines whether the date of birth is in the future (UTC).
    /// </summary>
    /// <returns><see langword="true"/> if the value is after <see cref="DateTime.UtcNow"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsInFuture()
        => Value > DateTime.UtcNow;

    /// <summary>
    /// Checks whether the date of birth is within a reasonable age range.
    /// </summary>
    /// <param name="maxYears">Maximum age in years. Defaults to 130.</param>
    /// <returns>
    /// <see langword="true"/> if the value is not more than <paramref name="maxYears"/> years in the past
    /// and not in the future; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsReasonableAge(int maxYears = 130)
        => Value >= DateTime.UtcNow.AddYears(-maxYears) && Value <= DateTime.UtcNow;
}
