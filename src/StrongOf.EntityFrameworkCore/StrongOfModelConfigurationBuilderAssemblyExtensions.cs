// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace StrongOf.EntityFrameworkCore;

/// <summary>
/// Convenience extensions that scan an assembly for concrete <see cref="StrongOf{TTarget,TStrong}"/>
/// derivatives and register a <see cref="StrongOfValueConverter{TStrong,TTarget}"/> for each one.
/// </summary>
/// <remarks>
/// <para>
/// Use these methods when you have many strong types and do not want to enumerate every type
/// manually in <c>ConfigureConventions</c>. Register once per assembly that defines strong types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
/// {
///     configurationBuilder.RegisterStrongOfFromAssembly(typeof(UserId).Assembly);
/// }
/// </code>
/// </example>
public static class StrongOfModelConfigurationBuilderAssemblyExtensions
{
    /// <summary>
    /// Scans the supplied assembly for sealed concrete classes derived from
    /// <see cref="StrongOf{TTarget,TStrong}"/> and registers a value-converter convention for each.
    /// </summary>
    /// <param name="builder">The model configuration builder.</param>
    /// <param name="assembly">The assembly to scan.</param>
    /// <returns>The same <paramref name="builder"/> for chaining.</returns>
    [RequiresUnreferencedCode("Reflects on all public types in the supplied assembly to discover strong types.")]
    public static ModelConfigurationBuilder RegisterStrongOfFromAssembly(
        this ModelConfigurationBuilder builder,
        Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assembly);

        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsGenericTypeDefinition)
            {
                continue;
            }

            if (!TryGetStrongOfTargetType(type, out Type? targetType))
            {
                continue;
            }

            Type converterType = typeof(StrongOfValueConverter<,>).MakeGenericType(type, targetType);

            // builder.Properties<TStrong>().HaveConversion<TConverter>()
            MethodInfo propertiesMethod = typeof(ModelConfigurationBuilder)
                .GetMethod(nameof(ModelConfigurationBuilder.Properties), [])!
                .MakeGenericMethod(type);

            object propertiesBuilder = propertiesMethod.Invoke(builder, null)!;

            MethodInfo haveConversion = propertiesBuilder.GetType()
                .GetMethods()
                .First(m => string.Equals(m.Name, "HaveConversion", StringComparison.Ordinal)
                            && m.IsGenericMethodDefinition
                            && m.GetGenericArguments().Length == 1
                            && m.GetParameters().Length == 0)
                .MakeGenericMethod(converterType);

            haveConversion.Invoke(propertiesBuilder, null);
        }

        return builder;
    }

    private static bool TryGetStrongOfTargetType(Type type, [NotNullWhen(true)] out Type? targetType)
    {
        Type? current = type.BaseType;
        while (current is not null)
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == typeof(StrongOf<,>))
            {
                targetType = current.GetGenericArguments()[0];
                return true;
            }
            current = current.BaseType;
        }

        targetType = null;
        return false;
    }
}
