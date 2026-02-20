// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Media;

/// <summary>
/// Represents a strongly-typed file system path.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="IsValidPath"/> to verify the value contains no invalid path characters.
/// Helper methods <see cref="GetExtension"/> and <see cref="GetFileName"/> delegate to <see cref="Path"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// FilePath path = new("/var/data/file.json");
/// bool valid = path.IsValidPath(); // true
/// string ext  = path.GetExtension(); // ".json"
/// string name = path.GetFileName();  // "file.json"
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongStringTypeConverter<FilePath>))]
public sealed class FilePath(string value) : StrongString<FilePath>(value)
{
    /// <summary>
    /// Determines whether the file path is valid (contains no invalid path characters).
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the value is non-empty and contains no characters from
    /// <see cref="Path.GetInvalidPathChars"/>; otherwise, <see langword="false"/>.
    /// </returns>
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
    /// <returns>The extension including the leading dot (e.g. <c>".txt"</c>), or an empty string if there is no extension.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetExtension()
        => Path.GetExtension(Value);

    /// <summary>
    /// Gets the file name of the path.
    /// </summary>
    /// <returns>The file name including extension (e.g. <c>"file.txt"</c>).</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string GetFileName()
        => Path.GetFileName(Value);
}
