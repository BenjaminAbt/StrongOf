// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Commerce;

/// <summary>
/// Represents a strongly-typed Stock Keeping Unit (SKU) / product code.
/// </summary>
/// <remarks>
/// <para>
/// A SKU is a unique identifier for a product variant used in inventory management.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var sku = new Sku("SHIRT-RED-L");
/// bool isValid = sku.IsValidFormat();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<Sku>))]
public sealed partial class Sku(string value) : StrongString<Sku>(value), IValidatable
{
    /// <summary>
    /// Regular expression pattern validating SKU format: alphanumeric characters and hyphens, 1–64 characters.
    /// </summary>
    [GeneratedRegex(@"^[A-Za-z0-9\-_]{1,64}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 500)]
    private static partial Regex SkuRegex();

    /// <summary>
    /// Validates whether the SKU has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && SkuRegex().IsMatch(Value);
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out Sku? result)
    {
        if (value is not null)
        {
            Sku candidate = new(value);
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
