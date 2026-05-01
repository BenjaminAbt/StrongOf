// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// A generic EF Core value converter for any <see cref="StrongOf{TTarget,TStrong}"/> type.
/// Converts between the strong type and its underlying primitive for database storage.
/// </summary>
/// <remarks>
/// <para>
/// This converter supports both explicit property mapping in <c>OnModelCreating</c>
/// and global registration in <c>ConfigureConventions</c>.
/// </para>
/// <para>
/// When used from <c>ConfigureConventions</c> with <c>HaveConversion&lt;TConversion&gt;()</c>,
/// EF Core expects a public parameterless constructor. This converter provides one.
/// </para>
/// </remarks>
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
public sealed class StrongOfValueConverter<TStrong, TTarget>
    : ValueConverter<TStrong, TTarget>
    where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
    where TTarget : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrongOfValueConverter{TStrong,TTarget}"/> class.
    /// </summary>
    public StrongOfValueConverter()
        : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StrongOfValueConverter{TStrong,TTarget}"/> class
    /// with optional mapping hints.
    /// </summary>
    /// <param name="mappingHints">Optional EF Core mapping hints.</param>
    public StrongOfValueConverter(ConverterMappingHints? mappingHints)
        : base(
            strong => strong.Value,
            value => StrongOf<TTarget, TStrong>.From(value),
            mappingHints)
    {
    }
}
