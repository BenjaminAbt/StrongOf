// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed year of birth.
/// </summary>
/// <remarks>
/// <para>
/// Valid values are in the range [<see cref="MinYear"/>, <see cref="MaxYear"/>] (1900–2100).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// BirthYear year = new(1990);
/// bool valid = year.IsValidRange(); // true
/// bool leap = year.IsLeapYear();    // false
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongInt32TypeConverter<BirthYear>))]
public sealed class BirthYear(int value) : StrongInt32<BirthYear>(value)
{
    /// <summary>
    /// Minimum supported year.
    /// </summary>
    public const int MinYear = 1900;

    /// <summary>
    /// Maximum supported year.
    /// </summary>
    public const int MaxYear = 2100;

    /// <summary>
    /// Determines whether the year is within the supported range [<see cref="MinYear"/>, <see cref="MaxYear"/>].
    /// </summary>
    /// <returns><see langword="true"/> if the value is between 1900 and 2100 inclusive; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinYear && Value <= MaxYear;

    /// <summary>
    /// Determines whether the year is a leap year.
    /// </summary>
    /// <returns><see langword="true"/> if the year is divisible by 4 (with century rules); otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsLeapYear()
        => DateTime.IsLeapYear(Value);
}
