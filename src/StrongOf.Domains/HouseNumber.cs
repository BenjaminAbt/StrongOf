// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed house number.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(HouseNumberTypeConverter))]
public sealed partial class HouseNumber(string value) : StrongString<HouseNumber>(value)
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
}

/// <summary>
/// Type converter for <see cref="HouseNumber"/>.
/// </summary>
public sealed class HouseNumberTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new HouseNumber(stringValue) : base.ConvertFrom(context, culture, value);
}
