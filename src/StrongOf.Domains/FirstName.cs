// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

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
[TypeConverter(typeof(FirstNameTypeConverter))]
public sealed class FirstName(string value) : StrongString<FirstName>(value)
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
}

/// <summary>
/// Type converter for <see cref="FirstName"/>.
/// </summary>
public sealed class FirstNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new FirstName(stringValue) : base.ConvertFrom(context, culture, value);
}
