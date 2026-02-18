// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Identity;

/// <summary>
/// Represents a strongly-typed generic entity identifier (GUID-based).
/// </summary>
/// <remarks>
/// <para>
/// This type wraps a GUID value representing a unique entity identifier.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var entityId = new EntityId(Guid.NewGuid());
/// bool isEmpty = entityId.IsEmpty();
/// </code>
/// </example>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(StrongGuidTypeConverter<EntityId>))]
public sealed class EntityId(Guid value) : StrongGuid<EntityId>(value)
{
    /// <summary>
    /// Creates a new <see cref="EntityId"/> with a randomly generated GUID.
    /// </summary>
    /// <returns>A new <see cref="EntityId"/> with a new GUID.</returns>
    /// <example>
    /// <code>
    /// var entityId = EntityId.New();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static new EntityId New()
        => new(Guid.NewGuid());

    /// <summary>
    /// Gets an empty <see cref="EntityId"/>.
    /// </summary>
    public static new EntityId Empty => new(Guid.Empty);

    /// <summary>
    /// Determines whether the entity ID is empty (Guid.Empty).
    /// </summary>
    /// <returns><c>true</c> if the entity ID is empty; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var entityId = EntityId.Empty;
    /// bool isEmpty = entityId.IsEmpty(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public new bool IsEmpty()
        => Value == Guid.Empty;

    /// <summary>
    /// Determines whether the entity ID has a value (not empty).
    /// </summary>
    /// <returns><c>true</c> if the entity ID has a value; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => Value != Guid.Empty;

    /// <summary>
    /// Gets the entity ID as a short string representation (first 8 characters).
    /// </summary>
    /// <returns>The short string representation.</returns>
    /// <example>
    /// <code>
    /// var entityId = new EntityId(Guid.Parse("12345678-1234-1234-1234-123456789012"));
    /// string shortId = entityId.ToShortString(); // "12345678"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToShortString()
        => Value.ToString("N")[..8];
}
