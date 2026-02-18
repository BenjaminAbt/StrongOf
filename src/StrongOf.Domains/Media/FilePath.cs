// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed file path.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(FilePathTypeConverter))]
public sealed class FilePath(string value) : StrongString<FilePath>(value)
{
    /// <summary>
    /// Determines whether the file path is valid.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsValidPath()
    {
        if (string.IsNullOrWhiteSpace(Value))
        {
            return false;
        }

        return Value.IndexOfAny(Path.GetInvalidPathChars()) < 0;
    }

    /// <summary>
    /// Gets the file extension of the path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetExtension()
        => Path.GetExtension(Value);

    /// <summary>
    /// Gets the file name of the path.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetFileName()
        => Path.GetFileName(Value);
}

/// <summary>
/// Type converter for <see cref="FilePath"/>.
/// </summary>
public sealed class FilePathTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new FilePath(stringValue) : base.ConvertFrom(context, culture, value);
}
