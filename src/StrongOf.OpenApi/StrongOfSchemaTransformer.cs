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
    private static readonly Dictionary<Type, (string Type, string? Format, string Description)> s_typeMap = new()
    {
        [typeof(IStrongGuid)] = ("string", "uuid", "A strongly-typed GUID value."),
        [typeof(IStrongString)] = ("string", null, "A strongly-typed string value."),
        [typeof(IStrongInt32)] = ("integer", "int32", "A strongly-typed 32-bit integer value."),
        [typeof(IStrongInt64)] = ("integer", "int64", "A strongly-typed 64-bit integer value."),
        [typeof(IStrongDecimal)] = ("number", "double", "A strongly-typed decimal value."),
        [typeof(IStrongDouble)] = ("number", "double", "A strongly-typed double-precision floating-point value."),
        [typeof(IStrongBoolean)] = ("boolean", null, "A strongly-typed boolean value."),
        [typeof(IStrongChar)] = ("string", null, "A strongly-typed single character."),
        [typeof(IStrongDateTime)] = ("string", "date-time", "A strongly-typed date and time value."),
        [typeof(IStrongDateTimeOffset)] = ("string", "date-time", "A strongly-typed date and time value with UTC offset."),
        [typeof(IStrongTimeSpan)] = ("string", "duration", "A strongly-typed time interval."),
    };

    /// <inheritdoc />
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        Type type = context.JsonTypeInfo.Type;

        foreach (KeyValuePair<Type, (string Type, string? Format, string Description)> entry in s_typeMap)
        {
            if (entry.Key.IsAssignableFrom(type))
            {
                schema.Type = entry.Value.Type;
                schema.Format = entry.Value.Format;
                schema.Description ??= entry.Value.Description;
                schema.Properties.Clear();
                break;
            }
        }

        return Task.CompletedTask;
    }
}
