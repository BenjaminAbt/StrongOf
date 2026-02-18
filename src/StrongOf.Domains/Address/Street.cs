// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace StrongOf.Domains.Address;

/// <summary>
/// Represents a strongly-typed street address.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a street address.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var street = new Street("123 Main Street");
/// bool isValid = street.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Street>))]
public sealed class Street(string value) : StrongString<Street>(value), IValidatable
{
    /// <summary>
    /// Validates whether the street address has a valid format (non-empty).
    /// </summary>
    /// <returns><c>true</c> if the street address format is valid; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value);

    /// <summary>
    /// Gets a normalized version of the street address (trimmed).
    /// </summary>
    /// <returns>The normalized street address.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetNormalized()
        => Value.Trim();
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Street? result)
    {
        if (value is not null)
        {
            Street candidate = new(value);
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
