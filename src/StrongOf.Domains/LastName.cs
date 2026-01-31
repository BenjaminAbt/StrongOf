// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed last name.
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a string value representing a person's last name (surname).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var lastName = new LastName("Smith");
/// string value = lastName.Value;
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(LastNameTypeConverter))]
public sealed class LastName(string value) : StrongString<LastName>(value)
{
    /// <summary>
    /// Validates whether the last name has a valid format (non-empty and contains only letters, spaces, hyphens, or apostrophes).
    /// </summary>
    /// <returns><c>true</c> if the last name format is valid; otherwise, <c>false</c>.</returns>
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
    /// Gets the last name with proper title case formatting.
    /// </summary>
    /// <returns>The last name formatted with title case.</returns>
    /// <example>
    /// <code>
    /// var name = new LastName("smith");
    /// string formatted = name.ToTitleCase(); // "Smith"
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
    /// Gets the last name in uppercase format.
    /// </summary>
    /// <returns>The last name in uppercase.</returns>
    /// <example>
    /// <code>
    /// var name = new LastName("Smith");
    /// string upper = name.ToUpperCase(); // "SMITH"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();
}

/// <summary>
/// Type converter for <see cref="LastName"/>.
/// </summary>
public sealed class LastNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new LastName(stringValue) : base.ConvertFrom(context, culture, value);
}
