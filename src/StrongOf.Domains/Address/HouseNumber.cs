// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed house number.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<HouseNumber>))]
public sealed partial class HouseNumber(string value) : StrongString<HouseNumber>(value), IValidatable
{
    /// <summary>
    /// Regular expression for house numbers such as "12", "12A", "12/3".
    /// </summary>
    [GeneratedRegex(@"^\d+[A-Za-z]?(?:[-/]\d+)?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex HouseNumberRegex();

    /// <summary>
    /// Determines whether the house number has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && HouseNumberRegex().IsMatch(Value);

    /// <summary>
    /// Gets the numeric part of the house number.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int GetNumericPart()
    {
        int i = 0;
        while (i < Value.Length && char.IsDigit(Value[i]))
        {
            i++;
        }

        return i == 0 ? 0 : int.Parse(Value[..i], System.Globalization.CultureInfo.InvariantCulture);
    }
    /// <summary>
    /// Tries to create a new instance if <paramref name="value"/> satisfies the format constraint.
    /// </summary>
    /// <param name="value">The input string to validate and wrap.</param>
    /// <param name="result">
    /// When this method returns, contains the created instance if the format is valid;
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns><see langword="true"/> if the value is non-null and passes <see cref="IsValidFormat"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryCreate(string? value, [NotNullWhen(true)] out HouseNumber? result)
    {
        if (value is not null)
        {
            HouseNumber candidate = new(value);
            if (candidate.IsValidFormat())
            {
                result = candidate;
                return true;
            }
        }
        result = null;
        return false;
    }
}
