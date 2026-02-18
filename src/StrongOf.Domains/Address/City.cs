// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed city name.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a city name.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var city = new City("New York");
/// bool isValid = city.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<City>))]
public sealed class City(string value) : StrongString<City>(value), IValidatable
{
    /// <summary>
    /// Validates whether the city name has a valid format.
    /// </summary>
    /// <returns><c>true</c> if the city name format is valid; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        foreach (char c in Value)
        {
            if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\'')
            {
                return false;
            }
        }
        return true;
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out City? result)
    {
        if (value is not null)
        {
            City candidate = new(value);
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
