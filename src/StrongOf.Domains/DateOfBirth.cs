// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed date of birth.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a <see cref="DateTime"/> value representing a date of birth.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var dob = new DateOfBirth(new DateTime(1990, 5, 12));
/// bool isPast = dob.IsInPast();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(DateOfBirthTypeConverter))]
public sealed class DateOfBirth(DateTime value) : StrongDateTime<DateOfBirth>(value)
{
    /// <summary>
    /// Determines whether the date of birth is in the past (UTC).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsInPast()
        => Value < DateTime.UtcNow;

    /// <summary>
    /// Determines whether the date of birth is in the future (UTC).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsInFuture()
        => Value > DateTime.UtcNow;

    /// <summary>
    /// Checks whether the date of birth is within a reasonable age range.
    /// </summary>
    /// <param name="maxYears">Maximum age in years.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsReasonableAge(int maxYears = 130)
        => Value >= DateTime.UtcNow.AddYears(-maxYears) && Value <= DateTime.UtcNow;
}

/// <summary>
/// Type converter for <see cref="DateOfBirth"/>.
/// </summary>
public sealed class DateOfBirthTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(DateTime) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        return value switch
        {
            DateTime dt => new DateOfBirth(dt),
            string s when DateTime.TryParse(s, culture, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime parsed) => new DateOfBirth(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
