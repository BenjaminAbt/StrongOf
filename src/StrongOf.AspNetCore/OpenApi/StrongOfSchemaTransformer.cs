// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.OpenApi;

#if NET9_0
using OpenApiSchema = Microsoft.OpenApi.Models.OpenApiSchema;
#else
using OpenApiSchema = Microsoft.OpenApi.OpenApiSchema;
#endif

namespace StrongOf.AspNetCore.OpenApi;

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
    /// <summary>
    /// Canonical mapping from StrongOf marker interfaces to OpenAPI primitive schema metadata.
    /// </summary>
    /// <remarks>
    /// We map by interface (instead of concrete type) so all user-defined StrongOf classes
    /// are automatically covered without additional registration.
    /// </remarks>
#if NET9_0
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
#else
    private static readonly Dictionary<Type, (Microsoft.OpenApi.JsonSchemaType Type, string? Format, string Description)> s_typeMap = new()
    {
        [typeof(IStrongGuid)] = (Microsoft.OpenApi.JsonSchemaType.String, "uuid", "A strongly-typed GUID value."),
        [typeof(IStrongString)] = (Microsoft.OpenApi.JsonSchemaType.String, null, "A strongly-typed string value."),
        [typeof(IStrongInt32)] = (Microsoft.OpenApi.JsonSchemaType.Integer, "int32", "A strongly-typed 32-bit integer value."),
        [typeof(IStrongInt64)] = (Microsoft.OpenApi.JsonSchemaType.Integer, "int64", "A strongly-typed 64-bit integer value."),
        [typeof(IStrongDecimal)] = (Microsoft.OpenApi.JsonSchemaType.Number, "double", "A strongly-typed decimal value."),
        [typeof(IStrongDouble)] = (Microsoft.OpenApi.JsonSchemaType.Number, "double", "A strongly-typed double-precision floating-point value."),
        [typeof(IStrongBoolean)] = (Microsoft.OpenApi.JsonSchemaType.Boolean, null, "A strongly-typed boolean value."),
        [typeof(IStrongChar)] = (Microsoft.OpenApi.JsonSchemaType.String, null, "A strongly-typed single character."),
        [typeof(IStrongDateTime)] = (Microsoft.OpenApi.JsonSchemaType.String, "date-time", "A strongly-typed date and time value."),
        [typeof(IStrongDateTimeOffset)] = (Microsoft.OpenApi.JsonSchemaType.String, "date-time", "A strongly-typed date and time value with UTC offset."),
        [typeof(IStrongTimeSpan)] = (Microsoft.OpenApi.JsonSchemaType.String, "duration", "A strongly-typed time interval."),
    };
#endif

    /// <inheritdoc />
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        Type type = context.JsonTypeInfo.Type;

        foreach (var entry in s_typeMap)
        {
            if (entry.Key.IsAssignableFrom(type))
            {
                // Replace the default object schema with the primitive wire representation,
                // otherwise OpenAPI would describe StrongOf types as nested JSON objects.
                schema.Type = entry.Value.Type;
                schema.Format = entry.Value.Format;
                schema.Description ??= entry.Value.Description;
                schema.Properties?.Clear();
                break;
            }
        }

        return Task.CompletedTask;
    }
}
