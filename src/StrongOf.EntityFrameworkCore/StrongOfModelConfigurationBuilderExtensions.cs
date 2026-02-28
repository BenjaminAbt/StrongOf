// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.EntityFrameworkCore;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// Extension methods for <see cref="ModelConfigurationBuilder"/> to register
/// <see cref="StrongOf{TTarget,TStrong}"/> value converters as conventions.
/// </summary>
/// <remarks>
/// <para>
/// Use these methods in <c>ConfigureConventions</c> to avoid configuring each
/// strong type property individually.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
/// {
///     configurationBuilder.RegisterStrongOf&lt;UserId, Guid&gt;();
///     configurationBuilder.RegisterStrongOf&lt;Email, string&gt;();
/// }
/// </code>
/// </example>
public static class StrongOfModelConfigurationBuilderExtensions
{
    /// <summary>
    /// Registers a <see cref="StrongOfValueConverter{TStrong,TTarget}"/> convention so that
    /// every property of type <typeparamref name="TStrong"/> is automatically converted.
    /// </summary>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="builder">The model configuration builder.</param>
    /// <returns>The builder for further chaining.</returns>
    public static ModelConfigurationBuilder RegisterStrongOf<TStrong, TTarget>(this ModelConfigurationBuilder builder)
        where TStrong : StrongOf<TTarget, TStrong>
        where TTarget : notnull
    {
        builder.Properties<TStrong>()
               .HaveConversion<StrongOfValueConverter<TStrong, TTarget>>();

        return builder;
    }
}
