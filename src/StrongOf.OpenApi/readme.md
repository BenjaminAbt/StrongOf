# StrongOf.OpenApi

OpenAPI schema transformer for [StrongOf](https://www.nuget.org/packages/StrongOf/) strongly-typed primitives.

Maps strong types to their underlying primitive types in OpenAPI documentation,
so API schemas show `string (uuid)` instead of a complex object for `UserId`.

## Requirements

- .NET 9.0 or later (uses the built-in ASP.NET Core OpenAPI support)

## Installation

```bash
dotnet add package StrongOf.OpenApi
```

## Usage

Register the schema transformer in your ASP.NET Core application:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<StrongOfSchemaTransformer>();
});
```

## Type Mappings

| Strong Type | OpenAPI Type | OpenAPI Format |
|-------------|-------------|----------------|
| `StrongGuid<T>` | `string` | `uuid` |
| `StrongString<T>` | `string` | - |
| `StrongInt32<T>` | `integer` | `int32` |
| `StrongInt64<T>` | `integer` | `int64` |
| `StrongDecimal<T>` | `number` | `double` |
| `StrongChar<T>` | `string` | - |
| `StrongDateTime<T>` | `string` | `date-time` |
| `StrongDateTimeOffset<T>` | `string` | `date-time` |
