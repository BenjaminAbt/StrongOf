// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

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
[TypeConverter(typeof(FullNameTypeConverter))]
public sealed class FullName(string value) : StrongString<FullName>(value)
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
}

/// <summary>
/// Type converter for <see cref="FullName"/>.
/// </summary>
public sealed class FullNameTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new FullName(stringValue) : base.ConvertFrom(context, culture, value);
}
