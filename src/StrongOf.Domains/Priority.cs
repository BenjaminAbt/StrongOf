// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains;

/// <summary>
/// Represents a strongly-typed priority level.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to represent priority levels in tasks, queues, or workflows.
/// Lower numeric values typically indicate higher priority (e.g., 1 = critical, 10 = low).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var priority = new Priority(1);
/// var tasks = queue.OrderBy(t => t.Priority);
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(PriorityTypeConverter))]
public sealed class Priority(int value) : StrongInt32<Priority>(value)
{
    /// <summary>
    /// Determines whether this priority is higher than another (lower numeric value = higher priority).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsHigherThan(Priority other)
        => Value < other.Value;

    /// <summary>
    /// Determines whether this priority is lower than another (higher numeric value = lower priority).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsLowerThan(Priority other)
        => Value > other.Value;
}

/// <summary>
/// Type converter for <see cref="Priority"/>.
/// </summary>
public sealed class PriorityTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value switch
        {
            int i => new Priority(i),
            string s when int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int parsed) => new Priority(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
