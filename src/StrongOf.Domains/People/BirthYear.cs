// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed year of birth.
/// </summary>
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
    /// Determines whether the year is within the supported range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinYear && Value <= MaxYear;

    /// <summary>
    /// Determines whether the year is a leap year.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsLeapYear()
        => DateTime.IsLeapYear(Value);
}
