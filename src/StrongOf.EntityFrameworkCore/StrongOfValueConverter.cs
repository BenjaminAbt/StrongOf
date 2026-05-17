// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// Generic EF Core value converter for <see cref="StrongOf{TTarget,TStrong}"/> model properties.
/// Converts strong types to their primitive database representation and back.
/// </summary>
/// <remarks>
/// <para>
/// The converter is usable in both explicit property mapping (<c>OnModelCreating</c>)
/// and convention-based registration (<c>ConfigureConventions</c>).
/// </para>
/// <para>
/// When used from <c>ConfigureConventions</c> with <c>HaveConversion&lt;TConversion&gt;()</c>,
/// EF Core expects a public parameterless constructor. This converter provides one.
/// </para>
/// <para>
/// Conversion uses expression trees so EF Core can inline the mapping into query translation,
/// change tracking and materialization pipelines.
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
    /// <remarks>
    /// This constructor is intentionally parameterless for compatibility with
    /// <c>HaveConversion&lt;TConversion&gt;()</c> convention registration.
    /// </remarks>
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
            // Persist only the primitive value so schema types remain native EF primitives.
            strong => strong.Value,
            // Rehydrate through StrongOf.From to support both source-generated and manual types.
            value => StrongOf<TTarget, TStrong>.From(value),
            mappingHints)
    {
    }
}
