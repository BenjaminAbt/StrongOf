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

## GitHub

See https://github.com/BenjaminAbt/StrongOf