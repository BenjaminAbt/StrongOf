// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed middle name.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<MiddleName>))]
public sealed class MiddleName(string value) : StrongString<MiddleName>(value)
{
    /// <summary>
    /// Minimum length for a valid middle name.
    /// </summary>
    public const int MinLength = 1;

    /// <summary>
    /// Maximum length for a valid middle name.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Determines whether the middle name length is valid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidLength()
        => !string.IsNullOrWhiteSpace(Value) && Value.Length >= MinLength && Value.Length <= MaxLength;

    /// <summary>
    /// Returns the trimmed middle name.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Trimmed()
        => Value.Trim();
}
