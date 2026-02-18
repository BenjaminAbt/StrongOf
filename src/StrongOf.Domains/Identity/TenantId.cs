// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Identity;

/// <summary>
/// Represents a strongly-typed tenant identifier for multi-tenant systems.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to isolate data and requests per tenant in multi-tenant architectures.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var tenantId = new TenantId(Guid.Parse("..."));
/// var orders = await orderRepo.GetForTenantAsync(tenantId);
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongGuidTypeConverter<TenantId>))]
public sealed class TenantId(Guid value) : StrongGuid<TenantId>(value)
{
    /// <summary>
    /// Determines whether the tenant ID has a non-empty value.
    /// </summary>
    /// <returns><see langword="true"/> if the underlying GUID is not <see cref="Guid.Empty"/>; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => Value != Guid.Empty;
}
