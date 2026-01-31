// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

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
[TypeConverter(typeof(CityTypeConverter))]
public sealed class City(string value) : StrongString<City>(value)
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
}

/// <summary>
/// Type converter for <see cref="City"/>.
/// </summary>
public sealed class CityTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new City(stringValue) : base.ConvertFrom(context, culture, value);
}
