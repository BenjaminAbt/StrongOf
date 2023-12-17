# StrongOf <a href="https://www.buymeacoffee.com/benjaminabt" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" height="30" ></a>

StrongOf helps to implement primitives as a strong type that represents a domain object (e.g. UserId, EmailAddress, etc.). It is a simple class that wraps a value and provides a few helper methods to make it easier to work with.

In contrast to other approaches, StrongOf is above all simple and performant - and not over-engineered.

## Why? 

Wrapping in a class is necessary because C# unfortunately still does not support [type abbreviations](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/type-abbreviations).

See GitHub proposal: [Proposal: Type aliases / abbreviations / newtype](https://github.com/dotnet/csharplang/issues/410)

## The idea

The frequent problem in code implementation is that values are not given any meaning and many methods are simply a technical string of values or data classes are just a list of types.

```csharp
public class User
{    
    public Tenant TenantId { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
```

A consequential problem is that there is also no compiler support if parameters are swapped. This can only be covered by complex unit tests.

```csharp
// no compiler warning if you mess up the order here
public User AddUser(Guid tenantId, Guid userId, string firstName, string lastName, string email)
```

The idea is to use a domain-driven design approach to give specific values a meaning through their own types.

```csharp
private sealed class TenantId(Guid value) : StrongGuid<TenantId>(value) { }
private sealed class UserId(Guid value) : StrongGuid<UserId>(value) { }
private sealed class FirstName(string value) : StrongString<FirstName>(value) { }
private sealed class LastName(string value) : StrongString<LastName>(value) { }
private sealed class Email(string value) : StrongString<Email>(value) { }

public class User
{    
    public TenantId TenantId { get; set; }
    public UserId UserId { get; set; }
    public FirstName FirstName { get; set; }
    public LastName LastName { get; set; }
    public Email Email { get; set; }
}

// with compiler warning if you mess up the order here
public User AddUser(TenantId tenantId, UserId userId, FirstName firstName, LastName lastName, Email email)
```

Now you are safe!

## Usage

The clearest distinction to other approaches is that all `StrongOf` types inherit from `StrongOf<T>` in order to be able to implement generic approaches. Furthermore, it is possible to extend the class, e.g. to implement validations.

```csharp
private sealed class UserId(Guid value) : StrongGuid<UserId>(value) { }
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

Unfortunately, Entity Framework does not love generic [Value Converters](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?WT.mc_id=DT-MVP-5001507), which is why you have to write it yourself.

```csharp
public class UserIdValueConverter : ValueConverter<UserId, Guid>
{
    public UserIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new(value), mappingHints) { }
}
```

There is no benefit in providing you a base class with an additional package and dependency.

## Installation

[![StrongOf](https://img.shields.io/nuget/v/StrongOf.svg?logo=nuget&label=StrongOf)](https://www.nuget.org/packages/StrongOf)\
[![StrongOf.AspNetCore](https://img.shields.io/nuget/v/StrongOf.AspNetCore.svg?logo=nuget&label=StrongOf.AspNetCore)](https://www.nuget.org/packages/StrongOf.AspNetCore)\
[![StrongOf.Json](https://img.shields.io/nuget/v/StrongOf.Json.svg?logo=nuget&label=StrongOf.Json)](https://www.nuget.org/packages/StrongOf.Json)

See [StrongOf on NuGet.org](https://www.nuget.org/packages/StrongOf)

## Performance matters

Since the strong types created here can still be instantiated with `new()`, this also means an enormous performance advantage over libraries that have to work with `Activator.CreateInstance` or `Expression.New`.

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

For certain scenarios, this library also has an `Expression.New` implementation (through a static From method); but not for general instantiation.

## FAQ

__Why no records?__

Records (currently) have a few disadvantages, which is why they are not suitable for this type of class. For example, it is currently not possible to validly inherit `GetHashCode`. `sealed` on `GetHashCode` is only available if the record itself is `sealed`, which does not make sense here.

__Why no Code Generator?__

Code generators are great, were my first idea too, but have proven to be a disadvantage in everyday life, e.g. when implementing generic extensions / implementations. This library is based on the experience of other libraries that have tended to be too large and their disadvantages.

__Why no structs?__

Structs cannot be used in all scenarios, e.g. with ASP.NET Core Action parameters.

---

[MIT LICENSE](./LICENSE)