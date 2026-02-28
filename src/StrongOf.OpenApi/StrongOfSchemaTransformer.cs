// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace StrongOf.OpenApi;

/// <summary>
/// An OpenAPI schema transformer that maps <see cref="StrongOf{TTarget,TStrong}"/> types
/// to their underlying primitive schemas, ensuring correct API documentation.
/// </summary>
/// <remarks>
/// <para>
/// Without this transformer, strong types appear as complex objects in OpenAPI specs.
/// With this transformer, they are correctly documented as their underlying primitive types.
/// </para>
/// <para>
/// Register this transformer in your ASP.NET Core application:
/// <code>
/// builder.Services.AddOpenApi(options =>
/// {
///     options.AddSchemaTransformer&lt;StrongOfSchemaTransformer&gt;();
/// });
/// </code>
/// </para>
/// </remarks>
public sealed class StrongOfSchemaTransformer : IOpenApiSchemaTransformer
{
    private static readonly Dictionary<Type, (string Type, string? Format)> s_typeMap = new()
    {
        [typeof(IStrongGuid)] = ("string", "uuid"),
        [typeof(IStrongString)] = ("string", null),
        [typeof(IStrongInt32)] = ("integer", "int32"),
        [typeof(IStrongInt64)] = ("integer", "int64"),
        [typeof(IStrongDecimal)] = ("number", "double"),
        [typeof(IStrongChar)] = ("string", null),
        [typeof(IStrongDateTime)] = ("string", "date-time"),
        [typeof(IStrongDateTimeOffset)] = ("string", "date-time"),
    };

    /// <inheritdoc />
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        Type type = context.JsonTypeInfo.Type;

        foreach (KeyValuePair<Type, (string Type, string? Format)> entry in s_typeMap)
        {
            if (entry.Key.IsAssignableFrom(type))
            {
                schema.Type = entry.Value.Type;
                schema.Format = entry.Value.Format;
                schema.Properties.Clear();
                break;
            }
        }

        return Task.CompletedTask;
    }
}
