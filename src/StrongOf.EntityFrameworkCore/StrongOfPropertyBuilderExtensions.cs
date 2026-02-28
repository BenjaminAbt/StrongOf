// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// Extension methods for configuring EF Core properties to use strong type value converters.
/// </summary>
public static class StrongOfPropertyBuilderExtensions
{
    /// <summary>
    /// Configures the property to use a <see cref="StrongOfValueConverter{TStrong,TTarget}"/>
    /// for converting between the strong type and its underlying primitive.
    /// </summary>
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
        where TStrong : StrongOf<TTarget, TStrong>
        where TTarget : notnull
    {
        return builder.HasConversion(new StrongOfValueConverter<TStrong, TTarget>());
    }
}
