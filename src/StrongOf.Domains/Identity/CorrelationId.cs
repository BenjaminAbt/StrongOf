// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Identity;

/// <summary>
/// Represents a strongly-typed correlation identifier for distributed tracing and request tracking.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to correlate log entries, events, and requests across services in distributed systems.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var correlationId = CorrelationId.New();
/// logger.LogInformation("Processing request {CorrelationId}", correlationId);
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(CorrelationIdTypeConverter))]
public sealed class CorrelationId(Guid value) : StrongGuid<CorrelationId>(value)
{
    /// <summary>
    /// Creates a new <see cref="CorrelationId"/> with a randomly generated GUID.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static new CorrelationId New()
        => new(Guid.NewGuid());

    /// <summary>
    /// Determines whether the correlation ID has a non-empty value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => Value != Guid.Empty;
}

/// <summary>
/// Type converter for <see cref="CorrelationId"/>.
/// </summary>
public sealed class CorrelationIdTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value switch
        {
            Guid g => new CorrelationId(g),
            string s when Guid.TryParse(s, out Guid parsed) => new CorrelationId(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
