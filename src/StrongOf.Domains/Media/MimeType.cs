// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed MIME type (e.g. <c>application/json</c>, <c>text/html</c>).
/// </summary>
/// <remarks>
/// <para>
/// A valid MIME type consists of a type and subtype separated by <c>/</c>.
/// Equality and hash code comparisons are case-insensitive per RFC 2045.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// MimeType mime = new("application/json");
/// bool valid   = mime.IsValidFormat(); // true
/// string type  = mime.GetTypePart();   // "application"
/// string sub   = mime.GetSubtypePart(); // "json"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<MimeType>))]
public sealed partial class MimeType(string value) : StrongString<MimeType>(value), IValidatable
{
    [GeneratedRegex(@"^[A-Za-z0-9!#$&^_.+-]+\/[A-Za-z0-9!#$&^_.+-]+$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex MimeTypeRegex();

    /// <summary>
    /// Determines whether the MIME type has a valid format.
    /// </summary>
    /// <returns><see langword="true"/> if the value matches the MIME type pattern (type/subtype); otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && MimeTypeRegex().IsMatch(Value);

    /// <summary>
    /// Gets the type part of the MIME type.
    /// </summary>
    /// <returns>The substring before the first <c>/</c>, or an empty string if there is no slash.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetTypePart()
    {
        int slashIndex = Value.IndexOf('/');
        return slashIndex >= 0 ? Value[..slashIndex] : string.Empty;
    }

    /// <summary>
    /// Gets the subtype part of the MIME type.
    /// </summary>
    /// <returns>The substring after the first <c>/</c>, or an empty string if there is no slash.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetSubtypePart()
    {
        int slashIndex = Value.IndexOf('/');
        return slashIndex >= 0 ? Value[(slashIndex + 1)..] : string.Empty;
    }

    /// <inheritdoc />
    /// <remarks>Comparison is case-insensitive because MimeType is defined as case-insensitive by its specification.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool Equals(MimeType? other)
        => other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is MimeType other && Equals(other);

    /// <inheritdoc />
    /// <remarks>Hash code is case-insensitive to match <see cref="Equals(MimeType?)"/>.</remarks>
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out MimeType? result)
    {
        if (value is not null)
        {
            MimeType candidate = new(value);
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
