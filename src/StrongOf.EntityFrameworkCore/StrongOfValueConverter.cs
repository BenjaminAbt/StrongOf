// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// A generic EF Core value converter for any <see cref="StrongOf{TTarget,TStrong}"/> type.
/// Converts between the strong type and its underlying primitive for database storage.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type.</typeparam>
/// <typeparam name="TTarget">The underlying primitive type (e.g., <see cref="Guid"/>, <see cref="string"/>, <see cref="int"/>).</typeparam>
/// <example>
/// <code>
/// // In OnModelCreating:
/// modelBuilder.Entity&lt;User&gt;()
///     .Property(e => e.Id)
///     .HasConversion(new StrongOfValueConverter&lt;UserId, Guid&gt;());
/// </code>
/// </example>
public sealed class StrongOfValueConverter<TStrong, TTarget>(ConverterMappingHints? mappingHints = null)
    : ValueConverter<TStrong, TTarget>(
        strong => strong.Value,
        value => StrongOf<TTarget, TStrong>.From(value),
        mappingHints)
    where TStrong : StrongOf<TTarget, TStrong>
    where TTarget : notnull;
