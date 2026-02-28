# StrongOf <a href="https://www.buymeacoffee.com/benjaminabt" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" height="30" ></a>

[![Build](https://github.com/benjaminabt/StrongOf/actions/workflows/ci.yml/badge.svg)](https://github.com/benjaminabt/StrongOf/actions/workflows/ci.yml)

||StrongOf|StrongOf.AspNetCore|StrongOf.Json|StrongOf.FluentValidation|StrongOf.EntityFrameworkCore|
|-|-|-|-|-|-|
|*NuGet*|[![NuGet](https://img.shields.io/nuget/v/StrongOf.svg?logo=nuget&label=StrongOf)](https://www.nuget.org/packages/StrongOf/)|[![NuGet](https://img.shields.io/nuget/v/StrongOf.AspNetCore.svg?logo=nuget&label=StrongOf.AspNetCore)](https://www.nuget.org/packages/StrongOf.AspNetCore)|[![NuGet](https://img.shields.io/nuget/v/StrongOf.Json.svg?logo=nuget&label=StrongOf.Json)](https://www.nuget.org/packages/StrongOf.Json)|[![NuGet](https://img.shields.io/nuget/v/StrongOf.FluentValidation.svg?logo=nuget&label=StrongOf.FluentValidation)](https://www.nuget.org/packages/StrongOf.FluentValidation)|[![NuGet](https://img.shields.io/nuget/v/StrongOf.EntityFrameworkCore.svg?logo=nuget&label=StrongOf.EntityFrameworkCore)](https://www.nuget.org/packages/StrongOf.EntityFrameworkCore)|

All [StrongOf Packages](https://www.nuget.org/packages/StrongOf)  are available for .NET 8, .NET 9, .NET 10, and .NET 11.

---

__StrongOf__ helps to implement primitives as a strong type that represents a domain object (e.g. UserId, EmailAddress, etc.). It is a simple class that wraps a value and provides a few helper methods to make it easier to work with.

In contrast to other approaches, __StrongOf__ is above all simple and performant - and not over-engineered.

## Why? 

This library was developed because C# did not support [type abbreviations](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/type-abbreviations) up to and including version 12. Originally announced for C#13, we should finally receive [Extension Types](https://devblogs.microsoft.com/dotnet/dotnet-build-2024-announcements) with C# 14.

See GitHub proposal: [Proposal: Type aliases / abbreviations / newtype](https://github.com/dotnet/csharplang/issues/410)

## The idea

The frequent problem in code implementation is that values are not given any meaning and many methods are simply a technical string of values or data classes are just a list of types.

```csharp
public class User
{    
    public  Guid    TenantId { get; set; }
    public  Guid    UserId { get; set; }
    public  string  FirstName { get; set; }
    public  string  LastName { get; set; }
    public  string  Email { get; set; }
}
```

A consequential problem is that there is also no compiler support if parameters are swapped. This can only be covered by complex unit tests.

```csharp
// no compiler warning if you mess up the order here
public User AddUser(Guid tenantId, Guid userId, string firstName, string lastName, string email)
```

The idea is to use a domain-driven design approach to give specific values a meaning through their own types.

```csharp
private sealed class TenantId(Guid value)    : StrongGuid<TenantId>(value) { }
private sealed class UserId(Guid value)      : StrongGuid<UserId>(value) { }
private sealed class FirstName(string value) : StrongString<FirstName>(value) { }
private sealed class LastName(string value)  : StrongString<LastName>(value) { }
private sealed class Email(string value)     : StrongString<Email>(value) { }

public class User
{    
    public  TenantId   TenantId { get; set; }
    public  UserId     UserId { get; set; }
    public  FirstName  FirstName { get; set; }
    public  LastName   LastName { get; set; }
    public  Email      Email { get; set; }
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

## Usage with StrongOf.Domains

`StrongOf.Domains` ships a collection of ready-to-use strong types organized into namespaces by domain area:

| Namespace | Types |
|-----------|-------|
| `StrongOf.Domains.Identity` | `CorrelationId`, `EntityId`, `TenantId`, `Token`, `Username` |
| `StrongOf.Domains.People` | `Age`, `BirthYear`, `DateOfBirth`, `FirstName`, `FullName`, `LastName`, `MiddleName`, `PhoneNumber` |
| `StrongOf.Domains.Postal` | `City`, `CountryCode`, `CountryName`, `HouseNumber`, `Street`, `ZipCode` |
| `StrongOf.Domains.Geography` | `GeoCoordinate`, `Latitude`, `Longitude` |
| `StrongOf.Domains.Networking` | `EmailAddress`, `HostName`, `HttpMethod`, `IpAddress`, `MacAddress`, `Port`, `Url` |
| `StrongOf.Domains.Finance` | `CurrencyCode`, `Iban`, `Percentage` |
| `StrongOf.Domains.Commerce` | `Priority`, `Quantity`, `Sku` |
| `StrongOf.Domains.Localization` | `LanguageCode`, `Locale`, `TimeZoneId` |
| `StrongOf.Domains.Measurement` | `HeightCm`, `TemperatureCelsius`, `WeightKg` |
| `StrongOf.Domains.Media` | `ColorHex`, `FileExtension`, `FilePath`, `Isbn`, `MimeType`, `Slug` |
| `StrongOf.Domains.Software` | `SemVer` |

### Global Usings - avoid repetitive `using` directives

Because domain types are spread across multiple namespaces, we strongly recommend adding a single `GlobalUsings.cs` file at the root of your project. This is a standard C# feature that applies `using` directives project-wide, so you never have to repeat them in individual files.

Create `GlobalUsings.cs` in your project root and include only the namespaces you actually use:

```csharp
// GlobalUsings.cs
global using StrongOf.Domains.Identity;
global using StrongOf.Domains.People;
global using StrongOf.Domains.Postal;
global using StrongOf.Domains.Geography;
global using StrongOf.Domains.Networking;
global using StrongOf.Domains.Finance;
global using StrongOf.Domains.Commerce;
global using StrongOf.Domains.Localization;
global using StrongOf.Domains.Measurement;
global using StrongOf.Domains.Media;
global using StrongOf.Domains.Software;
```

After this one-time setup, all domain types are available everywhere in your project without any further imports:

```csharp
// No using directives needed - GlobalUsings.cs already covers them
public class User
{
    public TenantId   TenantId   { get; set; }
    public UserId     UserId     { get; set; }
    public FirstName  FirstName  { get; set; }
    public LastName   LastName   { get; set; }
    public EmailAddress Email    { get; set; }
    public CountryCode Country   { get; set; }
}
```

> **Tip:** Global usings are evaluated at compile-time and have zero runtime overhead. They were introduced in C# 10 / .NET 6 and are a first-class language feature - not a workaround.

### Namespace naming rationale

The namespace names are deliberately chosen to **not conflict** with common domain class names (`Address`, `Person`, `Network`, `Location` etc. are all typical class names in business applications). Using plural or domain-specific nouns (`Postal`, `People`, `Networking`) means you can safely have both a `namespace StrongOf.Domains.Postal` import and a `class Address` in the same file without any ambiguity.

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
public class MyCustomStrongGuidBinder<TStrong> : StrongOfBinder
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

### Usage with Minimal APIs

All strong types implement `IParsable<TSelf>` and work as route/query parameters in Minimal APIs out of the box.
Use the validation filter from `StrongOf.AspNetCore.MinimalApis` to automatically validate `IValidatable` parameters:

```csharp
using StrongOf.AspNetCore.MinimalApis;

// Strong types work as route parameters out of the box
app.MapGet("/users/{id}", (UserId id) => Results.Ok(id));

// Register the endpoint filter for automatic validation of IValidatable types
app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
   .WithStrongOfValidation();
```

### Usage with OpenApi (.NET 9+)

Use the schema transformer from `StrongOf.AspNetCore.OpenApi` to correctly map strong types in OpenAPI specs:

```csharp
using StrongOf.AspNetCore.OpenApi;

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<StrongOfSchemaTransformer>();
});
```

This maps strong types to their underlying primitives (e.g. `UserId` becomes `string (uuid)` instead of a complex object).

## Usage with Entity Framework Core

Install [StrongOf.EntityFrameworkCore](https://www.nuget.org/packages/StrongOf.EntityFrameworkCore) for first-class EF Core integration with a generic value converter that works for all strong types - no per-type converter classes needed.

```bash
dotnet add package StrongOf.EntityFrameworkCore
```

### Register Converters Once with `ConfigureConventions` (Recommended)

Register each strong type once in `ConfigureConventions` and EF Core applies the converter globally across all entities and `DbSet`s - no per-property configuration required:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Register once - applies everywhere these types appear
        configurationBuilder.Properties<UserId>()
            .HaveConversion<StrongOfValueConverter<UserId, Guid>>();

        configurationBuilder.Properties<Email>()
            .HaveConversion<StrongOfValueConverter<Email, string>>();

        configurationBuilder.Properties<Amount>()
            .HaveConversion<StrongOfValueConverter<Amount, decimal>>();
    }
}
```

### Per-Property Configuration

For explicit control over individual properties (e.g. column constraints):

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>(entity =>
    {
        entity.Property(e => e.Id)
              .HasStrongOfConversion<UserId, Guid>();

        entity.Property(e => e.Email)
              .HasStrongOfConversion<Email, string>()
              .HasMaxLength(256);
    });
}
```

### LINQ Limitations

EF Core value converters work transparently for CRUD, equality comparisons (`==`, `!=`), and ordering. However, be aware of these limitations:

- **`.Value` in LINQ** - not translatable to SQL. Compare strong types against strong types instead.
- **`Contains` with mixed types** - the list must be the strong type (`UserId`), not the primitive (`Guid`). Use `UserId.From(guids)` to convert.
- **`ToString()` in LINQ** - not translatable. Format client-side after materializing.
- **Aggregations** - may require casting to the nullable primitive type (e.g. `(decimal?)order.Total`).

See the [StrongOf.EntityFrameworkCore readme](src/StrongOf.EntityFrameworkCore/readme.md) for detailed examples and a full compatibility table.

### Why No Source Generator for EF Core?

The generic `StrongOfValueConverter<TStrong, TTarget>` already eliminates all per-type boilerplate. Combined with `ConfigureConventions`, registration is a single line per type. A source generator would add Roslyn coupling complexity and contradicts the project's design philosophy (see FAQ below) - for zero practical benefit.

## Usage with FluentValidation

FluentValidation is a great library for validating models; especially popular in the ASP.NET Core world.\
Therefore, separate validations are available for `StrongOf` models, which are constantly being expanded.

In order not to forget the namespace, separate methods are available that differ from the default ValidationContext.

```csharp
public class MySubmitModel
{
    // Mandatory properties should be 
    //  marked as not null, but can still be null at 
    //  runtime if no value has been passed.
    public MyStrongString MyUserName { get; set; } = null!;
}

public class MySubmitModelValidator : AbstractValidator<MySubmitModel>
{
    public MySubmitModelValidator()
    {
        RuleFor(x => x.MyUserName)
            .HasValue() // not NotNull
            .WithMessage("No user name passed.");

        // more validations...
```


## Performance matters

Since the strong types created here can still be instantiated with `new()`, this also means an enormous performance advantage over libraries that have to work with `Activator.CreateInstance` or `Expression.New`.

```shell
BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6216/22H2/2022Update)
AMD Ryzen 9 9950X 4.30GHz, 1 CPU, 32 logical and 16 physical cores
.NET SDK 10.0.100-preview.7.25380.108
  [Host]    : .NET 10.0.0 (10.0.25.38108), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 10.0 : .NET 10.0.0 (10.0.25.38108), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0  : .NET 8.0.19 (8.0.1925.36514), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0  : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method      | Runtime   | Categories   | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated |
|------------ |---------- |------------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|
| Guid_New    | .NET 8.0  | StrongGuid   | 1.738 ns | 0.0645 ns | 0.0571 ns |  1.00 |    0.03 | 0.0019 |      32 B |
| Guid_New    | .NET 9.0  | StrongGuid   | 1.701 ns | 0.0288 ns | 0.0255 ns |  0.98 |    0.02 | 0.0019 |      32 B |
| Guid_New    | .NET 10.0 | StrongGuid   | 1.732 ns | 0.0249 ns | 0.0208 ns |  1.00 |    0.02 | 0.0019 |      32 B |
|             |           |              |          |           |           |       |         |        |           |
| Guid_From   | .NET 8.0  | StrongGuid   | 3.347 ns | 0.0263 ns | 0.0246 ns |  1.33 |    0.02 | 0.0019 |      32 B |
| Guid_From   | .NET 9.0  | StrongGuid   | 2.525 ns | 0.0243 ns | 0.0203 ns |  1.00 |    0.02 | 0.0019 |      32 B |
| Guid_From   | .NET 10.0 | StrongGuid   | 2.523 ns | 0.0472 ns | 0.0394 ns |  1.00 |    0.02 | 0.0019 |      32 B |
|             |           |              |          |           |           |       |         |        |           |
| Int32_New   | .NET 8.0  | StrongInt32  | 1.677 ns | 0.0315 ns | 0.0263 ns |  0.97 |    0.04 | 0.0014 |      24 B |
| Int32_New   | .NET 9.0  | StrongInt32  | 1.737 ns | 0.0645 ns | 0.0690 ns |  1.01 |    0.05 | 0.0014 |      24 B |
| Int32_New   | .NET 10.0 | StrongInt32  | 1.728 ns | 0.0663 ns | 0.0588 ns |  1.00 |    0.05 | 0.0014 |      24 B |
|             |           |              |          |           |           |       |         |        |           |
| Int32_From  | .NET 8.0  | StrongInt32  | 2.895 ns | 0.0911 ns | 0.0852 ns |  1.55 |    0.05 | 0.0014 |      24 B |
| Int32_From  | .NET 9.0  | StrongInt32  | 1.928 ns | 0.0325 ns | 0.0304 ns |  1.03 |    0.02 | 0.0014 |      24 B |
| Int32_From  | .NET 10.0 | StrongInt32  | 1.872 ns | 0.0182 ns | 0.0161 ns |  1.00 |    0.01 | 0.0014 |      24 B |
|             |           |              |          |           |           |       |         |        |           |
| Int64_New   | .NET 8.0  | StrongInt64  | 1.684 ns | 0.0255 ns | 0.0213 ns |  0.95 |    0.06 | 0.0014 |      24 B |
| Int64_New   | .NET 9.0  | StrongInt64  | 1.746 ns | 0.0530 ns | 0.0496 ns |  0.99 |    0.06 | 0.0014 |      24 B |
| Int64_New   | .NET 10.0 | StrongInt64  | 1.776 ns | 0.0700 ns | 0.1089 ns |  1.00 |    0.09 | 0.0014 |      24 B |
|             |           |              |          |           |           |       |         |        |           |
| Int64_From  | .NET 8.0  | StrongInt64  | 2.764 ns | 0.0379 ns | 0.0354 ns |  1.42 |    0.03 | 0.0014 |      24 B |
| Int64_From  | .NET 9.0  | StrongInt64  | 1.978 ns | 0.0289 ns | 0.0256 ns |  1.02 |    0.02 | 0.0014 |      24 B |
| Int64_From  | .NET 10.0 | StrongInt64  | 1.943 ns | 0.0369 ns | 0.0345 ns |  1.00 |    0.02 | 0.0014 |      24 B |
|             |           |              |          |           |           |       |         |        |           |
| String_New  | .NET 8.0  | StrongString | 1.667 ns | 0.0367 ns | 0.0326 ns |  0.97 |    0.02 | 0.0014 |      24 B |
| String_New  | .NET 9.0  | StrongString | 1.646 ns | 0.0292 ns | 0.0273 ns |  0.96 |    0.02 | 0.0014 |      24 B |
| String_New  | .NET 10.0 | StrongString | 1.710 ns | 0.0237 ns | 0.0198 ns |  1.00 |    0.02 | 0.0014 |      24 B |
|             |           |              |          |           |           |       |         |        |           |
| String_From | .NET 8.0  | StrongString | 3.638 ns | 0.0637 ns | 0.0596 ns |  1.26 |    0.03 | 0.0014 |      24 B |
| String_From | .NET 9.0  | StrongString | 2.803 ns | 0.0401 ns | 0.0376 ns |  0.97 |    0.02 | 0.0014 |      24 B |
| String_From | .NET 10.0 | StrongString | 2.882 ns | 0.0695 ns | 0.0650 ns |  1.00 |    0.03 | 0.0014 |      24 B |
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
