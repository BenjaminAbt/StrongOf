// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Person;

/// <summary>
/// Represents a strongly-typed first name.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a person's first name.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var firstName = new FirstName("John");
/// string value = firstName.Value;
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<FirstName>))]
public sealed class FirstName(string value) : StrongString<FirstName>(value), IValidatable
{
    /// <summary>
    /// Validates whether the first name has a valid format (non-empty and contains only letters, spaces, hyphens, or apostrophes).
    /// </summary>
    /// <returns><c>true</c> if the first name format is valid; otherwise, <c>false</c>.</returns>
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
    /// Gets the first name with proper title case formatting.
    /// </summary>
    /// <returns>The first name formatted with title case.</returns>
    /// <example>
    /// <code>
    /// var name = new FirstName("john");
    /// string formatted = name.ToTitleCase(); // "John"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToTitleCase()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return Value;
        }

        return char.ToUpperInvariant(Value[0]) + Value[1..].ToLowerInvariant();
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out FirstName? result)
    {
        if (value is not null)
        {
            FirstName candidate = new(value);
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
