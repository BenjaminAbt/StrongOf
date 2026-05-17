// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// Extension methods for configuring EF Core properties that use StrongOf value objects.
/// </summary>
public static class StrongOfPropertyBuilderExtensions
{
    /// <summary>
    /// Configures the property to use <see cref="StrongOfValueConverter{TStrong,TTarget}"/>
    /// for conversion between the strong type and its primitive store type.
    /// </summary>
    /// <remarks>
    /// Use this method in <c>OnModelCreating</c> when you want explicit, per-property EF Core mapping
    /// in addition to or instead of model-wide configuration in <c>ConfigureConventions</c>.
    /// </remarks>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <returns>The property builder for further chaining.</returns>
    /// <example>
    /// <code>
    /// modelBuilder.Entity&lt;User&gt;(entity =>
    /// {
    ///     entity.Property(e => e.Id)
    ///           .HasStrongOfConversion&lt;UserId, Guid&gt;();
    /// });
    /// </code>
    /// </example>
    public static PropertyBuilder<TStrong> HasStrongOfConversion<TStrong, TTarget>(this PropertyBuilder<TStrong> builder)
        where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
        where TTarget : notnull
    {
        // Keep the converter explicit on this property when the model should not use
        // a global convention for all properties of the same strong type.
        return builder.HasConversion(new StrongOfValueConverter<TStrong, TTarget>());
    }
}
