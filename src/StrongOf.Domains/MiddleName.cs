// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed middle name.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(MiddleNameTypeConverter))]
public sealed class MiddleName(string value) : StrongString<MiddleName>(value)
{
    /// <summary>
    /// Minimum length for a valid middle name.
    /// </summary>
    public const int MinLength = 1;

    /// <summary>
    /// Maximum length for a valid middle name.
    /// </summary>
    public const int MaxLength = 50;

    /// <summary>
    /// Determines whether the middle name length is valid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidLength()
        => !string.IsNullOrWhiteSpace(Value) && Value.Length >= MinLength && Value.Length <= MaxLength;

    /// <summary>
    /// Returns the trimmed middle name.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Trimmed()
        => Value.Trim();
}

/// <summary>
/// Type converter for <see cref="MiddleName"/>.
/// </summary>
public sealed class MiddleNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new MiddleName(stringValue) : base.ConvertFrom(context, culture, value);
}
