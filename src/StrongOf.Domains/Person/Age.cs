// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Person;

/// <summary>
/// Represents a strongly-typed age value.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps an integer value representing a person's age in years.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var age = new Age(25);
/// bool isAdult = age.IsAdult();
/// </code>
/// </example>
[DebuggerDisplay("{Value} years")]
[TypeConverter(typeof(StrongInt32TypeConverter<Age>))]
public sealed class Age(int value) : StrongInt32<Age>(value)
{
    /// <summary>
    /// The minimum valid age value.
    /// </summary>
    public const int MinValue = 0;

    /// <summary>
    /// The maximum valid age value.
    /// </summary>
    public const int MaxValue = 150;

    /// <summary>
    /// The default adult age threshold.
    /// </summary>
    public const int AdultAge = 18;

    /// <summary>
    /// Creates an <see cref="Age"/> from a birth date.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <returns>A new <see cref="Age"/> instance.</returns>
    /// <example>
    /// <code>
    /// var birthDate = new DateTime(1990, 5, 15);
    /// var age = Age.FromBirthDate(birthDate);
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Age FromBirthDate(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
        {
            age--;
        }
        return new(age);
    }

    /// <summary>
    /// Validates whether the age is within the valid range.
    /// </summary>
    /// <returns><c>true</c> if the age is within valid range; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidRange()
        => Value >= MinValue && Value <= MaxValue;

    /// <summary>
    /// Determines whether the age represents an adult (18 years or older by default).
    /// </summary>
    /// <param name="adultAge">The age threshold for adulthood. Defaults to 18.</param>
    /// <returns><c>true</c> if the age is adult; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var age = new Age(21);
    /// bool isAdult = age.IsAdult(); // true
    /// bool isAdultIn21 = age.IsAdult(21); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsAdult(int adultAge = AdultAge)
        => Value >= adultAge;

    /// <summary>
    /// Determines whether the age represents a minor (under 18 years by default).
    /// </summary>
    /// <param name="adultAge">The age threshold for adulthood. Defaults to 18.</param>
    /// <returns><c>true</c> if the age is a minor; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsMinor(int adultAge = AdultAge)
        => Value < adultAge;
}
