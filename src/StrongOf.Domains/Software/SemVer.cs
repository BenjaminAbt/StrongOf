// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Software;

/// <summary>
/// Represents a strongly-typed semantic version string following SemVer 2.0.0.
/// </summary>
/// <remarks>
/// <para>
/// A valid SemVer string has the form <c>MAJOR.MINOR.PATCH[-pre][+build]</c>.
/// Use <see cref="IsValidFormat"/> to validate and <see cref="TryGetMajor"/> to extract the major version.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// SemVer version = new("2.1.0-beta.1");
/// bool valid = version.IsValidFormat(); // true
/// if (version.TryGetMajor(out int major))
/// {
///     // major == 2
/// }
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<SemVer>))]
public sealed partial class SemVer(string value) : StrongString<SemVer>(value), IValidatable
{
    [GeneratedRegex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?(?:\+[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex SemVerRegex();

    /// <summary>
    /// Determines whether the semantic version has a valid format.
    /// </summary>
    /// <returns><see langword="true"/> if the value matches the SemVer 2.0.0 pattern; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && SemVerRegex().IsMatch(Value);

    /// <summary>
    /// Tries to read the major version component from the version string.
    /// </summary>
    /// <param name="major">When this method returns <see langword="true"/>, contains the major version number; otherwise, 0.</param>
    /// <returns><see langword="true"/> if the major component could be parsed; otherwise, <see langword="false"/>.</returns>
    public bool TryGetMajor(out int major)
    {
        major = 0;
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        string[] parts = Value.Split('.', 3, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 1 && int.TryParse(parts[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out major);
    }
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
    public static bool TryCreate(string? value, [NotNullWhen(true)] out SemVer? result)
    {
        if (value is not null)
        {
            SemVer candidate = new(value);
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
