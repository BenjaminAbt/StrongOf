// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

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
        options.ModelBinderProviders.Insert(0, new StrongOfModelBinderProvider(binderMap));
        return options;
    }
}
