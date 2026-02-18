// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.People;

/// <summary>
/// Represents a strongly-typed middle name.
/// </summary>
/// <remarks>
/// <para>
/// The middle name must be between <see cref="MinLength"/> and <see cref="MaxLength"/> characters (1–50).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// MiddleName name = new("James");
/// bool valid = name.IsValidLength(); // true
/// string trimmed = name.Trimmed();
/// </code>
/// </example>
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
    /// <returns>
    /// <see langword="true"/> if the value is non-empty and between
    /// <see cref="MinLength"/> and <see cref="MaxLength"/> characters inclusive;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidLength()
        => !string.IsNullOrWhiteSpace(Value) && Value.Length >= MinLength && Value.Length <= MaxLength;

    /// <summary>
    /// Returns the trimmed middle name.
    /// </summary>
    /// <returns>The <see cref="P:StrongOf.StrongOf{System.String,MiddleName}.Value"/> with leading and trailing white-space removed.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Trimmed()
        => Value.Trim();
}
