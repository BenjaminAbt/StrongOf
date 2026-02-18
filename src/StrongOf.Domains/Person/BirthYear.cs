// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Person;

/// <summary>
/// Represents a strongly-typed year of birth.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(BirthYearTypeConverter))]
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

/// <summary>
/// Type converter for <see cref="BirthYear"/>.
/// </summary>
public sealed class BirthYearTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        return value switch
        {
            int i => new BirthYear(i),
            string s when int.TryParse(s, System.Globalization.NumberStyles.Integer, culture, out int parsed) => new BirthYear(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}
