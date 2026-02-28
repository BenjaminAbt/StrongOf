# StrongOf.AspNetCore

ASP.NET Core model binders for [StrongOf](https://www.nuget.org/packages/StrongOf) types. Enables route values, query strings, and form fields to be automatically parsed into strong types.

## Available Binders

| Binder | For |
|--------|-----|
| `StrongGuidBinder<T>` | `StrongGuid<T>` types |
| `StrongStringBinder<T>` | `StrongString<T>` types |
| `StrongInt32Binder<T>` | `StrongInt32<T>` types |
| `StrongInt64Binder<T>` | `StrongInt64<T>` types |
| `StrongDecimalBinder<T>` | `StrongDecimal<T>` types |

## Setup

Register a custom `IModelBinderProvider` in your ASP.NET Core startup:

```csharp
// Program.cs
builder.Services.AddControllers(options =>
    options.ModelBinderProviders.Insert(0, new MyBinderProvider()));

// MyBinderProvider.cs
public sealed class MyBinderProvider : IModelBinderProvider
{
    private static readonly IReadOnlyDictionary<Type, Type> s_binders =
        new Dictionary<Type, Type>
        {
            { typeof(UserId),       typeof(StrongGuidBinder<UserId>)   },
            { typeof(EmailAddress), typeof(StrongStringBinder<EmailAddress>) },
        };

    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (s_binders.TryGetValue(context.Metadata.ModelType, out Type? binderType))
        {
            return new BinderTypeModelBinder(binderType);
        }
        return null;
    }
}
```

## Custom Binders

Extend `StrongOfBinder` to add custom parsing logic:

```csharp
public sealed class MyUserIdBinder : StrongOfBinder
{
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongGuid<UserId>.TryParse(value, out UserId? id))
        {
            result = ModelBindingResult.Success(id);
            return true;
        }
        result = ModelBindingResult.Failed();
        return false;
    }
}
```

## Installation

```bash
dotnet add package StrongOf.AspNetCore
```

## Minimal APIs (`StrongOf.AspNetCore.MinimalApis`)

All strong types implement `IParsable<TSelf>` and work as route/query parameters in Minimal APIs out of the box.

Use the validation filter to automatically validate `IValidatable` parameters:

```csharp
using StrongOf.AspNetCore.MinimalApis;

// Strong types work as route parameters out of the box
app.MapGet("/users/{id}", (UserId id) => Results.Ok(id));

// Register the endpoint filter for automatic validation of IValidatable types
app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
   .WithStrongOfValidation();
```

## OpenAPI Schema Transformer (`StrongOf.AspNetCore.OpenApi`)

> Requires .NET 9.0 or later

Maps strong types to their underlying primitive types in OpenAPI documentation,
so API schemas show `string (uuid)` instead of a complex object for `UserId`.

```csharp
using StrongOf.AspNetCore.OpenApi;

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<StrongOfSchemaTransformer>();
});
```

### Type Mappings

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

## GitHub

See https://github.com/BenjaminAbt/StrongOf