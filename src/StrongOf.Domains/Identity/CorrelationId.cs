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
[TypeConverter(typeof(StrongGuidTypeConverter<CorrelationId>))]
public sealed class CorrelationId(Guid value) : StrongGuid<CorrelationId>(value)
{
    /// <summary>
    /// Determines whether the correlation ID has a non-empty value.
    /// </summary>
    /// <returns><see langword="true"/> if the underlying GUID is not <see cref="Guid.Empty"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => Value != Guid.Empty;
}
