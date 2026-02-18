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
[TypeConverter(typeof(TenantIdTypeConverter))]
public sealed class TenantId(Guid value) : StrongGuid<TenantId>(value)
{
    /// <summary>
    /// Creates a new <see cref="TenantId"/> with a randomly generated GUID.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static new TenantId New()
        => new(Guid.NewGuid());

    /// <summary>
    /// Determines whether the tenant ID has a non-empty value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => Value != Guid.Empty;
}

/// <summary>
/// Type converter for <see cref="TenantId"/>.
/// </summary>
public sealed class TenantIdTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value switch
        {
            Guid g => new TenantId(g),
            string s when Guid.TryParse(s, out Guid parsed) => new TenantId(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
