// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Person;

/// <summary>
/// Represents a strongly-typed full name combining first and last name.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a person's full name.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var fullName = new FullName("John Smith");
/// string value = fullName.Value;
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<FullName>))]
public sealed class FullName(string value) : StrongString<FullName>(value), IValidatable
{
    /// <summary>
    /// Creates a <see cref="FullName"/> from a <see cref="FirstName"/> and <see cref="LastName"/>.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <returns>A new <see cref="FullName"/> instance.</returns>
    /// <example>
    /// <code>
    /// var firstName = new FirstName("John");
    /// var lastName = new LastName("Smith");
    /// var fullName = FullName.FromNames(firstName, lastName); // "John Smith"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static FullName FromNames(FirstName firstName, LastName lastName)
        => new($"{firstName.Value} {lastName.Value}");

    /// <summary>
    /// Validates whether the full name has a valid format (non-empty and contains at least one space).
    /// </summary>
    /// <returns><c>true</c> if the full name format is valid; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && Value.Contains(' ', StringComparison.Ordinal);

    /// <summary>
    /// Gets the first part of the full name (typically the first name).
    /// </summary>
    /// <returns>The first part before the first space, or the entire value if no space found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetFirstPart()
    {
        int spaceIndex = Value.IndexOf(' ', StringComparison.Ordinal);
        return spaceIndex >= 0 ? Value[..spaceIndex] : Value;
    }

    /// <summary>
    /// Gets the last part of the full name (typically the last name).
    /// </summary>
    /// <returns>The part after the last space, or the entire value if no space found.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetLastPart()
    {
        int spaceIndex = Value.LastIndexOf(' ');
        return spaceIndex >= 0 ? Value[(spaceIndex + 1)..] : Value;
    }

    /// <summary>
    /// Gets the initials from the full name.
    /// </summary>
    /// <returns>The initials (first letter of each word).</returns>
    /// <example>
    /// <code>
    /// var fullName = new FullName("John Michael Smith");
    /// string initials = fullName.GetInitials(); // "JMS"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetInitials()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return string.Empty;
        }

        string[] parts = Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        char[] initials = new char[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            initials[i] = char.ToUpperInvariant(parts[i][0]);
        }
        return new string(initials);
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out FullName? result)
    {
        if (value is not null)
        {
            FullName candidate = new(value);
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
