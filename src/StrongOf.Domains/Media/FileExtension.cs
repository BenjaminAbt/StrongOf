// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed file extension (e.g. <c>.txt</c>, <c>.json</c>).
/// </summary>
/// <remarks>
/// <para>
/// A valid extension starts with a dot followed by 1–10 alphanumeric characters.
/// Equality and hash code comparisons are case-insensitive.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// FileExtension ext = new(".json");
/// bool valid   = ext.IsValidFormat(); // true
/// string noDot = ext.WithoutDot();   // "json"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<FileExtension>))]
public sealed partial class FileExtension(string value) : StrongString<FileExtension>(value), IValidatable
{
    [GeneratedRegex(@"^\.[A-Za-z0-9]{1,10}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex FileExtensionRegex();

    /// <summary>
    /// Determines whether the file extension has a valid format.
    /// </summary>
    /// <returns><see langword="true"/> if the value matches <c>^\.[A-Za-z0-9]{1,10}$</c>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && FileExtensionRegex().IsMatch(Value);

    /// <summary>
    /// Returns the extension without the leading dot.
    /// </summary>
    /// <returns>The value without the leading <c>.</c>, or the original value if it does not start with a dot.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string WithoutDot()
        => Value.StartsWith(".", StringComparison.Ordinal) ? Value[1..] : Value;

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because FileExtension is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(FileExtension? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is FileExtension other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(FileExtension?)"/>.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
        => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out FileExtension? result)
    {
        if (value is not null)
        {
            FileExtension candidate = new(value);
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
