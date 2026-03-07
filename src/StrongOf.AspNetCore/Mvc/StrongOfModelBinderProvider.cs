// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// An <see cref="IModelBinderProvider"/> that automatically resolves the correct
/// <see cref="StrongOfBinder"/> for any registered strong type, eliminating the need
/// for manual per-type binder registration.
/// </summary>
/// <remarks>
/// <para>
/// Register this provider in your ASP.NET Core application:
/// <code>
/// builder.Services.AddControllers(options =>
/// {
///     options.ModelBinderProviders.Insert(0, new StrongOfModelBinderProvider(new Dictionary&lt;Type, Type&gt;
///     {
///         { typeof(UserId), typeof(StrongGuidBinder&lt;UserId&gt;) },
///         { typeof(Email), typeof(StrongStringBinder&lt;Email&gt;) },
///     }));
/// });
/// </code>
/// </para>
/// </remarks>
public sealed class StrongOfModelBinderProvider : IModelBinderProvider
{
    private static readonly IReadOnlyDictionary<Type, Type> s_binderDefinitions = new Dictionary<Type, Type>
    {
        { typeof(StrongBoolean<>), typeof(StrongBooleanBinder<>) },
        { typeof(StrongChar<>), typeof(StrongCharBinder<>) },
        { typeof(StrongDateTime<>), typeof(StrongDateTimeBinder<>) },
        { typeof(StrongDateTimeOffset<>), typeof(StrongDateTimeOffsetBinder<>) },
        { typeof(StrongDecimal<>), typeof(StrongDecimalBinder<>) },
        { typeof(StrongDouble<>), typeof(StrongDoubleBinder<>) },
        { typeof(StrongGuid<>), typeof(StrongGuidBinder<>) },
        { typeof(StrongInt32<>), typeof(StrongInt32Binder<>) },
        { typeof(StrongInt64<>), typeof(StrongInt64Binder<>) },
        { typeof(StrongString<>), typeof(StrongStringBinder<>) },
        { typeof(StrongTimeSpan<>), typeof(StrongTimeSpanBinder<>) },
    };

    private readonly Dictionary<Type, Type> _binderMap;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrongOfModelBinderProvider"/> class with explicit binder mappings.
    /// </summary>
    /// <param name="binderMap">A dictionary mapping strong types to their corresponding binder types.</param>
    public StrongOfModelBinderProvider(Dictionary<Type, Type> binderMap)
    {
        ArgumentNullException.ThrowIfNull(binderMap);
        _binderMap = binderMap;
    }

    /// <inheritdoc />
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (_binderMap.TryGetValue(context.Metadata.ModelType, out Type? binderType))
        {
            return new BinderTypeModelBinder(binderType);
        }

        return null;
    }

    internal static Dictionary<Type, Type> CreateBinderMap(IEnumerable<Type> strongTypes)
    {
        ArgumentNullException.ThrowIfNull(strongTypes);

        Dictionary<Type, Type> binderMap = [];

        foreach (Type strongType in strongTypes)
        {
            ArgumentNullException.ThrowIfNull(strongType);

            if (binderMap.ContainsKey(strongType))
            {
                continue;
            }

            if (!TryResolveBinderType(strongType, out Type? binderType))
            {
                throw new NotSupportedException($"No StrongOf ASP.NET Core binder is available for type '{strongType}'.");
            }

            binderMap.Add(strongType, binderType);
        }

        if (binderMap.Count == 0)
        {
            throw new ArgumentException("At least one supported StrongOf type must be provided.", nameof(strongTypes));
        }

        return binderMap;
    }

    internal static Dictionary<Type, Type> CreateBinderMapFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        HashSet<Type> discoveredStrongTypes = [];

        foreach (Assembly assembly in assemblies)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && !type.IsGenericTypeDefinition && TryResolveBinderType(type, out _))
                {
                    discoveredStrongTypes.Add(type);
                }
            }
        }

        return CreateBinderMap(discoveredStrongTypes);
    }

    internal static bool TryResolveBinderType(Type strongType, [NotNullWhen(true)] out Type? binderType)
    {
        ArgumentNullException.ThrowIfNull(strongType);

        if (strongType.IsAbstract || strongType.IsGenericTypeDefinition)
        {
            binderType = null;
            return false;
        }

        Type? currentType = strongType;

        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && s_binderDefinitions.TryGetValue(currentType.GetGenericTypeDefinition(), out Type? binderDefinition))
            {
                binderType = binderDefinition.MakeGenericType(strongType);
                return true;
            }

            currentType = currentType.BaseType;
        }

        binderType = null;
        return false;
    }
}

/// <summary>
/// Extension methods for configuring <see cref="StrongOfModelBinderProvider"/> in ASP.NET Core MVC.
/// </summary>
public static class StrongOfMvcOptionsExtensions
{
    /// <summary>
    /// Adds a <see cref="StrongOfModelBinderProvider"/> to the MVC options with the specified binder mappings.
    /// </summary>
    /// <param name="options">The MVC options.</param>
    /// <param name="binderMap">A dictionary mapping strong types to their corresponding binder types.</param>
    /// <returns>The MVC options for further chaining.</returns>
    /// <example>
    /// <code>
    /// builder.Services.AddControllers(options =>
    /// {
    ///     options.AddStrongOfModelBinderProvider(new Dictionary&lt;Type, Type&gt;
    ///     {
    ///         { typeof(UserId), typeof(StrongGuidBinder&lt;UserId&gt;) },
    ///         { typeof(Email), typeof(StrongStringBinder&lt;Email&gt;) },
    ///     });
    /// });
    /// </code>
    /// </example>
    public static MvcOptions AddStrongOfModelBinderProvider(this MvcOptions options, Dictionary<Type, Type> binderMap)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(binderMap);

        options.ModelBinderProviders.Insert(0, new StrongOfModelBinderProvider(binderMap));
        return options;
    }

    /// <summary>
    /// Adds a <see cref="StrongOfModelBinderProvider"/> to the MVC options and automatically
    /// resolves the required binder types for the provided StrongOf model types.
    /// </summary>
    /// <param name="options">The MVC options.</param>
    /// <param name="strongTypes">The concrete StrongOf model types to register.</param>
    /// <returns>The MVC options for further chaining.</returns>
    public static MvcOptions AddStrongOfModelBinderProvider(this MvcOptions options, params Type[] strongTypes)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(strongTypes);

        return options.AddStrongOfModelBinderProvider(StrongOfModelBinderProvider.CreateBinderMap(strongTypes));
    }

    /// <summary>
    /// Adds a <see cref="StrongOfModelBinderProvider"/> to the MVC options and automatically
    /// resolves binders for all supported StrongOf model types discovered in the provided assemblies.
    /// </summary>
    /// <param name="options">The MVC options.</param>
    /// <param name="assemblies">Assemblies to scan for concrete StrongOf types.</param>
    /// <returns>The MVC options for further chaining.</returns>
    public static MvcOptions AddStrongOfModelBinderProviderFromAssemblies(this MvcOptions options, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(assemblies);

        return options.AddStrongOfModelBinderProvider(StrongOfModelBinderProvider.CreateBinderMapFromAssemblies(assemblies));
    }
}
