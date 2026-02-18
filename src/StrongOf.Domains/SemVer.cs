// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed semantic version (SemVer 2.0.0).
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(SemVerTypeConverter))]
public sealed partial class SemVer(string value) : StrongString<SemVer>(value)
{
    [GeneratedRegex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?(?:\+[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex SemVerRegex();

    /// <summary>
    /// Determines whether the semantic version has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && SemVerRegex().IsMatch(Value);

    /// <summary>
    /// Tries to read the major version component.
    /// </summary>
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
}

/// <summary>
/// Type converter for <see cref="SemVer"/>.
/// </summary>
public sealed class SemVerTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new SemVer(stringValue) : base.ConvertFrom(context, culture, value);
}
