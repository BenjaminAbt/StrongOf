// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed file extension.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(FileExtensionTypeConverter))]
public sealed partial class FileExtension(string value) : StrongString<FileExtension>(value)
{
    [GeneratedRegex(@"^\.[A-Za-z0-9]{1,10}$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture, matchTimeoutMilliseconds: 1000)]
    private static partial Regex FileExtensionRegex();

    /// <summary>
    /// Determines whether the file extension has a valid format.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidFormat()
        => !string.IsNullOrWhiteSpace(Value) && FileExtensionRegex().IsMatch(Value);

    /// <summary>
    /// Returns the extension without the leading dot.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string WithoutDot()
        => Value.StartsWith(".", StringComparison.Ordinal) ? Value[1..] : Value;
}

/// <summary>
/// Type converter for <see cref="FileExtension"/>.
/// </summary>
public sealed class FileExtensionTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new FileExtension(stringValue) : base.ConvertFrom(context, culture, value);
}
