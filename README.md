# StrongOf <a href="https://www.buymeacoffee.com/benjaminabt" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" height="30" ></a>

StrongOf helps to implement primitives as a strong type that represents a domain object (e.g. UserId, EmailAddress, etc.). It is a simple class that wraps a value and provides a few helper methods to make it easier to work with.

In contrast to other approaches, StrongOf is above all simple and performant - and not over-engineered.

## Why? 

Wrapping in a class is necessary because C# unfortunately still does not support [type abbreviations](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/type-abbreviations).

See GitHub proposal: [Proposal: Type aliases / abbreviations / newtype](https://github.com/dotnet/csharplang/issues/410)

## Usage

The clearest distinction to other approaches is that all `StrongOf` types inherit from `StrongOf<T>` in order to be able to implement generic approaches. Furthermore, it is possible to extend the class, e.g. to implement validations.

```csharp
private sealed class UserId(Guid value) : StrongGuid<UserId>(value) { }
```

This has the enormous advantage that `new` can still be used for instantiation in order to utilize the performance advantages over `Activator.CreateInstance` or `Expression.New`.

```shell
BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3803/22H2/2022Update)
AMD Ryzen 9 5950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.14 (7.0.1423.51910), X64 RyuJIT AVX2


| Method      | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|------------ |----------:|----------:|----------:|----------:|-------:|----------:|
| Guid_New    |  3.366 ns | 0.0998 ns | 0.0934 ns |  3.379 ns | 0.0019 |      32 B |
| Guid_From   | 12.301 ns | 1.0492 ns | 3.0936 ns | 14.184 ns | 0.0019 |      32 B |
|             |           |           |           |           |        |           |
| Int32_New   |  2.996 ns | 0.1013 ns | 0.1167 ns |  2.954 ns | 0.0014 |      24 B |
| Int32_From  |  9.829 ns | 0.6536 ns | 1.9273 ns | 10.486 ns | 0.0014 |      24 B |
|             |           |           |           |           |        |           |
| Int64_New   |  2.703 ns | 0.0867 ns | 0.0724 ns |  2.671 ns | 0.0014 |      24 B |
| Int64_From  | 11.706 ns | 0.4101 ns | 1.2093 ns | 11.976 ns | 0.0014 |      24 B |
|             |           |           |           |           |        |           |
| String_New  |  3.807 ns | 0.1205 ns | 0.1127 ns |  3.837 ns | 0.0014 |      24 B |
| String_From | 11.339 ns | 0.4997 ns | 1.4735 ns | 10.969 ns | 0.0014 |      24 B |
```

## Usage with Json

You can just use [StrongOf.Json](https://www.nuget.org/packages/StrongOf.Json) and use one of the pre-defined converters

```csharp
public class MyClass
{
    [JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
    public UserId Id { get; set; }
}
```

or use the `JsonSerializerOptions` 

```csharp
JsonSerializerOptions serializeOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Converters =
    {
        new StrongGuidJsonConverter<UserId>()
    }
};

string jsonString = JsonSerializer.Serialize(myObject, serializeOptions);
```

## Usage with ASP.NET Core

You can just use [StrongOf.AspNetCore](https://www.nuget.org/packages/StrongOf.AspNetCore) and use one of the pre-defined binders

```csharp
public class MyBinderProvider : IModelBinderProvider
{
    private static readonly IReadOnlyDictionary<Type, Type> s_binders = new Dictionary<Type, Type>
    {
        {typeof(UserId), typeof(StrongGuidBinder<UserId>)}
        // ... more here ...
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

You can also create a customized binder

```csharp
public class MyCustonStrongGuidBinder<TStrong> : StrongOfBinder
    where TStrong : StrongGuid<TStrong>
{
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        // do something here
        
        if (StrongGuid<TStrong>.TryParse(value, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
```


## Usage with Entity Framework

Unfortunately, the Entity Framework does not love generic [Value Converter](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?WT.mc_id=DT-MVP-5001507), which is why you have to write it yourself.

```csharp
public class UserIdValueConverter : ValueConverter<UserId, Guid>
{
    public UserIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new(value), mappingHints) { }
}
```

## Usage with ASP.NET Core

Unfortunately, the Entity Framework does not love generic [Value Converter](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?WT.mc_id=DT-MVP-5001507), which is why you have to write it yourself.

```csharp
public class UserIdValueConverter : ValueConverter<UserId, Guid>
{
    public UserIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new(value), mappingHints) { }
}
```

## Installation

[![StrongOf](https://img.shields.io/nuget/v/StrongOf.svg?logo=nuget&label=StrongOf)](https://www.nuget.org/packages/StrongOf)\
[![StrongOf.AspNetCore](https://img.shields.io/nuget/v/StrongOf.AspNetCore.svg?logo=nuget&label=StrongOf.AspNetCore)](https://www.nuget.org/packages/StrongOf.AspNetCore)
[![StrongOf.Json](https://img.shields.io/nuget/v/StrongOf.Json.svg?logo=nuget&label=StrongOf.Json)](https://www.nuget.org/packages/StrongOf.Json)

See [StrongOf on NuGet.org](https://www.nuget.org/packages/StrongOf)

## FAQ

__Why no records?__

Records (currently) have a few disadvantages, which is why they are not suitable for this type of class. For example, it is currently not possible to validly inherit `GetHashCode`. `sealed` on `GetHashCode` is only available if the record itself is `sealed`, which does not make sense here.

---

[MIT LICENSE](./LICENSE)