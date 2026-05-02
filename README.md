# StrongOf

<p align="center">
    <img src="res/benjamin-abt-strongof.png" alt="StrongOf" width="160" />
</p>

<p align="center">
    <a href="https://NuBrowse.com/packages/StrongOf"><img src="https://img.shields.io/nuget/v/StrongOf?label=StrongOf&logo=nuget&color=0f766e" alt="StrongOf" /></a>
    <a href="https://NuBrowse.com/packages/StrongOf.Domains"><img src="https://img.shields.io/nuget/v/StrongOf.Domains?label=StrongOf.Domains&logo=nuget&color=0f766e" alt="StrongOf.Domains" /></a>
    <a href="https://NuBrowse.com/packages/StrongOf.Json"><img src="https://img.shields.io/nuget/v/StrongOf.Json?label=StrongOf.Json&logo=nuget&color=0f766e" alt="StrongOf.Json" /></a>
</p>

<p align="center">
    <a href="https://NuBrowse.com/packages/StrongOf.AspNetCore"><img src="https://img.shields.io/nuget/v/StrongOf.AspNetCore?label=StrongOf.AspNetCore&logo=nuget&color=0369a1" alt="StrongOf.AspNetCore" /></a>
    <a href="https://NuBrowse.com/packages/StrongOf.EntityFrameworkCore"><img src="https://img.shields.io/nuget/v/StrongOf.EntityFrameworkCore?label=StrongOf.EntityFrameworkCore&logo=nuget&color=0369a1" alt="StrongOf.EntityFrameworkCore" /></a>
    <a href="https://NuBrowse.com/packages/StrongOf.FluentValidation"><img src="https://img.shields.io/nuget/v/StrongOf.FluentValidation?label=StrongOf.FluentValidation&logo=nuget&color=0369a1" alt="StrongOf.FluentValidation" /></a>
</p>

<p align="center">
    <a href="LICENSE"><img src="https://img.shields.io/badge/License-MIT-f4b400.svg" alt="License: MIT" /></a>
</p>

<p align="center">
    <strong>Strongly typed primitives for .NET 8, .NET 9, .NET 10, and .NET 11.</strong>
    <br />
    Core library, domain types, JSON, ASP.NET Core, EF Core, and FluentValidation integrations.
</p>

---

__StrongOf__ helps to implement primitives as a strong type that represents a domain object (e.g. UserId, EmailAddress, etc.). It is a simple class that wraps a value and provides a few helper methods to make it easier to work with.

In contrast to other approaches, __StrongOf__ is above all simple and performant - and not over-engineered.

## Why? 

This library was created because C# still does not support [type abbreviations](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/type-abbreviations).

It is currently unclear whether this feature will ever become part of the language:  
[Proposal: Type aliases / abbreviations / newtype](https://github.com/dotnet/csharplang/issues/410)

## Table of Contents

- [The idea](#the-idea)
- [Usage](#usage)
    - [Usage with source generators](#usage-with-source-generators)
    - [Usage without source generators](#usage-without-source-generators)
    - [Bulk conversion helpers](#bulk-conversion-helpers)
    - [Validation helpers for custom domain types](#validation-helpers-for-custom-domain-types)
    - [Available marker attributes](#available-marker-attributes)
- [StrongOf.Domains](#usage-with-strongofdomains)
    - [Global Usings - avoid repetitive `using` directives](#global-usings---avoid-repetitive-using-directives)
    - [Namespace naming rationale](#namespace-naming-rationale)
- [Json](#usage-with-json)
- [ASP.NET Core](#usage-with-aspnet-core)
    - [Usage with Minimal APIs](#usage-with-minimal-apis)
    - [Usage with OpenApi (.NET 9+)](#usage-with-openapi-net-9)
- [Entity Framework Core](#usage-with-entity-framework-core)
    - [Register Converters Once with `ConfigureConventions` (Recommended)](#register-converters-once-with-configureconventions-recommended)
    - [Configure Strong Types in `OnModelCreating`](#configure-strong-types-in-onmodelcreating)
    - [LINQ Limitations](#linq-limitations)
    - [Why No Source Generator for EF Core?](#why-no-source-generator-for-ef-core)
- [FluentValidation](#usage-with-fluentvalidation)
- [Migrations](#migrations)
    - [Migration guide: StrongOf v2 -> v3 (Source Generators)](#migration-guide-strongof-v2---v3-source-generators)
    - [Interop metadata for other source generators](#interop-metadata-for-other-source-generators)
- [Performance matters](#performance-matters)
- [FAQ](#faq)


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
private sealed class TenantId(Guid value)    : StrongGuid<TenantId>(value);
private sealed class UserId(Guid value)      : StrongGuid<UserId>(value);
private sealed class FirstName(string value) : StrongString<FirstName>(value);
private sealed class LastName(string value)  : StrongString<LastName>(value);
private sealed class Email(string value)     : StrongString<Email>(value);

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

The recommended approach is to define strong types with the built-in source generator. If you prefer the classic hand-written form, that is still fully supported and shown afterwards.

### Usage with source generators

> For v2 users: see migration guide to v3 at the end of this readme.

The shipped Roslyn source generator turns a single attribute-marked partial class into a fully implemented strong type, with **zero** Expression-based factory dependency. The result is fully Native AOT and trim safe.

Recommended style is the generic form `Strong<TTarget>`:

- Preferred: `[Strong<Guid>]`
- Also supported: `[StrongGuid]` and `[Strong(typeof(Guid))]`
- Exactly one marker is required per type declaration (do not combine multiple marker forms on the same class).

```csharp
using StrongOf.SourceGeneration;

[Strong<Guid>]
public partial class UserId;

[StrongGuid] 
public partial class LegacyUserId;

[StrongString]
public partial class Email;
[Strong<string>]
public partial class Alias;

[StrongInt32] 
public partial class Quantity;

[StrongDecimal]
public partial class Amount;
```

The generator emits the primary constructor, the base type (`StrongGuid<UserId>`,
`StrongString<Email>`, ...) and an AOT-friendly static `Create` method that the generic `From(...)`
dispatch resolves directly to `new TStrong(value)`. No reflection, no `Expression.Compile`.

You can still extend the type with your own partial - the generator only contributes the
constructor / base type / `Create` member, never the body:

```csharp
[StrongString]
public partial class Email
{
    public bool LooksLikeEmail() => Value.Contains('@');
}
```


### Usage without source generators

The classic hand-written form keeps the implementation explicit. All strong types inherit from `StrongOf<TTarget, TStrong>`, and hand-written types must implement `IStrongOf<TTarget, TStrong>` so generic `From(...)` calls can dispatch through a trim-safe static `Create` method.

```csharp
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);
}
```

Prefer direct instantiation with `new` in hot paths:

```csharp
UserId userId1 = new(Guid.NewGuid());
UserId userId2 = UserId.From(Guid.NewGuid());
```

`From(...)` is intended for generic scenarios where the concrete strong type is not known statically.

### Bulk conversion helpers

For collections of primitive values, StrongOf provides dedicated conversion helpers:

```csharp
Guid[] ids = [Guid.NewGuid(), Guid.NewGuid()];

List<UserId>? list = UserId.From(ids);
UserId[]? array = UserId.FromArray(ids);
UserId[] spanArray = UserId.FromSpan(ids);
```

Use `FromArray(...)` or `FromSpan(...)` when you want a compact array result with a single allocation.

### Validation helpers for custom domain types

If you build your own validated domain types, prefer explicit factory methods over implicit conversions:

With source generators:

```csharp
using StrongOf.SourceGeneration;

[StrongString]
public partial class Email
{
    public static bool TryCreate(string? value, out Email? result)
        => StrongValidation.TryCreate(value, IsValid, Email.Create, out result);

    private static bool IsValid(string value) => value.Contains('@');
}
```

Without source generators:

```csharp
public sealed class Email(string value) : StrongString<Email>(value), IStrongOf<string, Email>
{
    public static Email Create(string value) => new(value);

    public static bool TryCreate(string? value, out Email? result)
        => StrongValidation.TryCreate(value, IsValid, Create, out result);

    private static bool IsValid(string value) => value.Contains('@');
}
```

This keeps construction explicit and allows validation without moving logic into the primary constructor.

### Available marker attributes

The following marker attributes are available in `StrongOf.SourceGeneration`:

| Attribute | Wraps |
|-----------|-------|
| `[Strong<TTarget>]` | Generic form (same supported primitive set as below) |
| `[StrongBoolean]` | `bool` |
| `[StrongChar]` | `char` |
| `[StrongDateTime]` | `DateTime` |
| `[StrongDateTimeOffset]` | `DateTimeOffset` |
| `[StrongDecimal]` | `decimal` |
| `[StrongDouble]` | `double` |
| `[StrongGuid]` | `Guid` |
| `[StrongInt32]` | `int` |
| `[StrongInt64]` | `long` |
| `[StrongString]` | `string` |
| `[StrongTimeSpan]` | `TimeSpan` |

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

You can just use [StrongOf.Json](https://NuBrowse.com/packages/StrongOf.Json) and use one of the pre-defined converters.

**Recommended: Options-based (explicit registration):**

```csharp
JsonSerializerOptions serializeOptions = new()
{
    WriteIndented = true,
    Converters =
    {
        new StrongGuidJsonConverter<UserId>(),
        new StrongStringJsonConverter<EmailAddress>(),
    }
};

string jsonString = JsonSerializer.Serialize(myObject, serializeOptions);
```

**Attribute-based:**

```csharp
public class MyClass
{
    [JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
    public UserId Id { get; set; }
}
```

## Usage with ASP.NET Core

You can just use [StrongOf.AspNetCore](https://NuBrowse.com/packages/StrongOf.AspNetCore) and use one of the pre-defined binders from the `StrongOf.AspNetCore.Mvc` namespace:

```csharp
using StrongOf.AspNetCore.Mvc;

builder.Services.AddControllers(options =>
    options.AddStrongOfModelBinderProvider(
        typeof(UserId),
        typeof(EmailAddress)));
```

If your strong types live in one or more assemblies, you can also register them by assembly scan:

```csharp
using StrongOf.AspNetCore.Mvc;

builder.Services.AddControllers(options =>
    options.AddStrongOfModelBinderProviderFromAssemblies(typeof(UserId).Assembly));
```

You can also create a customized binder

```csharp
using StrongOf.AspNetCore.Mvc;

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

Install [StrongOf.EntityFrameworkCore](https://NuBrowse.com/packages/StrongOf.EntityFrameworkCore) for first-class EF Core integration with a generic value converter that works for all strong types - no per-type converter classes needed.

```bash
dotnet add package StrongOf.EntityFrameworkCore
```

### Register Converters Once with `ConfigureConventions` (Recommended)

For most applications, register each strong type once in `ConfigureConventions`. EF Core then applies the converter globally across all entities and `DbSet`s with no per-property configuration required:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.RegisterStrongOf<UserId, Guid>();
        configurationBuilder.RegisterStrongOf<Email, string>();
        configurationBuilder.RegisterStrongOf<Amount, decimal>();
    }
}
```

`ConfigureConventions` uses EF Core's pre-convention activation path. `StrongOfValueConverter<TStrong, TTarget>` supports this by exposing the public parameterless constructor EF Core expects for `HaveConversion<TConversion>()`.

### Configure Strong Types in `OnModelCreating`

If you want explicit control over individual properties or want the conversion right next to length, precision, or column-type settings, configure it in `OnModelCreating`:

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

    modelBuilder.Entity<Order>(entity =>
    {
        entity.Property(e => e.Total)
              .HasStrongOfConversion<Amount, decimal>()
              .HasPrecision(18, 2);
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

The generic `StrongOfValueConverter<TStrong, TTarget>` already eliminates all per-type boilerplate. Combined with `ConfigureConventions`, registration is a single line per type, while `OnModelCreating` remains available when you want more explicit per-property control.

## Usage with FluentValidation

FluentValidation is a great library for validating models; especially popular in the ASP.NET Core world.
Therefore, separate validations are available for `StrongOf` models, which are constantly being expanded.

In order not to forget the namespace, separate methods are available that differ from the default ValidationContext.

```csharp
public class MySubmitModel
{
    // Mandatory properties should be 
    //  marked as not null, but can still be null at 
    //  runtime if no value has been passed.
    public MyStrongString MyUserName { get; set; } = null!;

    public UserId UserId { get; set; } = null!;
}

public class MySubmitModelValidator : AbstractValidator<MySubmitModel>
{
    public MySubmitModelValidator()
    {
        RuleFor(x => x.MyUserName)
            .HasValue() // not NotNull
            .WithMessage("No user name passed.");

        RuleFor(x => x.MyUserName)
            .IsNotNull();

        RuleFor(x => x.UserId)
            .HasNonDefaultValue<MySubmitModel, UserId, Guid>();

        RuleFor(x => x.UserId)
            .ValueMust<MySubmitModel, UserId, Guid>(value => value != Guid.Empty);
    }
}
```

There are also generic rules for all StrongOf types:

```csharp
RuleFor(x => x.SomeStrongValue)
    .IsNotNull();

RuleFor(x => x.SomeStrongValue)
    .HasNonDefaultValue<MyModel, SomeStrongValue, Guid>();

RuleFor(x => x.SomeStrongValue)
    .ValueMust<MyModel, SomeStrongValue, Guid>(value => value != Guid.Empty);
```

These are useful for non-string strong types where `HasValue()` is not expressive enough.

## Migrations

### Migration guide: StrongOf v2 -> v3 (Source Generators)

This section shows how to migrate existing hand-written strong types to the v3 generator-based style.

1. Update NuGet package(s) to v3 (`StrongOf` and optional integration packages).
2. Add `using StrongOf.SourceGeneration;` in files where you declare strong types.
3. Replace hand-written inheritance with attribute + `partial class`.
4. Keep domain-specific behavior in additional partial class bodies.

#### 1) Basic type migration

Before (v2 / hand-written):

```csharp
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);
}
```

After (v3 / recommended):

```csharp
using StrongOf.SourceGeneration;

[Strong<Guid>]
public partial class UserId;
```

Also valid:

```csharp
using StrongOf.SourceGeneration;

[StrongGuid]
public partial class UserId;
```

#### 2) Keep custom domain methods/validation

If your old type had custom members, keep them in a separate partial body:

```csharp
using StrongOf.SourceGeneration;

[Strong<string>]
public partial class Email;

public partial class Email
{
    public bool LooksLikeEmail() => Value.Contains('@');
}
```

#### 3) Interop for other source generators

If other generators in your solution need explicit primitive metadata, add:

```csharp
using StrongOf.SourceGeneration;

[Strong(typeof(Guid))]
public partial class UserId;
```

`[Strong(typeof(...))]` is supported by StrongOf itself and is also useful as stable metadata for external generators.

#### 4) Quick migration checklist

- Replace `public sealed class X(T value) : StrongY<X>(value)` with `[Strong<T>] public partial class X;`
- Remove hand-written static `Create` methods (generator emits them)
- Keep only domain logic in partial class bodies
- Build and run tests to confirm behavior parity

### Interop metadata for other source generators

If you use additional source generators and want to expose the primitive target type as explicit
metadata, you can use `Strong(typeof(...))` directly as your single marker:

```csharp
using StrongOf.SourceGeneration;

[Strong(typeof(Guid))]
public partial class UserId;
```

`StrongAttribute` (non-generic form) is supported by StrongOf's generator and also works well as explicit primitive metadata for external generators.

If you want to keep the classic hand-written form, you still can - but you must declare the explicit
`IStrongOf<TTarget, TStrong>` contract yourself:

```csharp
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);
}
```

The generator does exactly this for you.

## Performance matters

Since the strong types created here can still be instantiated with `new()`, this also means an enormous performance advantage over libraries that have to work with `Activator.CreateInstance` or `Expression.New`.

```shell
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.7184/22H2/2022Update)
AMD Ryzen 9 9950X 4.30GHz, 1 CPU, 32 logical and 16 physical cores
.NET SDK 11.0.100-preview.2.26159.112
  [Host]    : .NET 10.0.7 (10.0.7, 10.0.726.21808), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.7 (10.0.7, 10.0.726.21808), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.26 (8.0.26, 8.0.2626.16921), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.15 (9.0.15, 9.0.1526.17522), X64 RyuJIT x86-64-v4


| Method      | Runtime   | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated |
|------------ |---------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|
| Guid_New    | .NET 10.0 | 1.714 ns | 0.0301 ns | 0.0281 ns |  1.00 |    0.02 | 0.0019 |      32 B |
| Guid_New    | .NET 8.0  | 1.764 ns | 0.0344 ns | 0.0287 ns |  1.03 |    0.02 | 0.0019 |      32 B |
| Guid_New    | .NET 9.0  | 1.714 ns | 0.0221 ns | 0.0207 ns |  1.00 |    0.02 | 0.0019 |      32 B |
|             |           |          |           |           |       |         |        |           |
| Guid_From   | .NET 10.0 | 2.534 ns | 0.0322 ns | 0.0285 ns |  1.00 |    0.02 | 0.0019 |      32 B |
| Guid_From   | .NET 8.0  | 3.411 ns | 0.0985 ns | 0.0922 ns |  1.35 |    0.04 | 0.0019 |      32 B |
| Guid_From   | .NET 9.0  | 2.462 ns | 0.0267 ns | 0.0237 ns |  0.97 |    0.01 | 0.0019 |      32 B |
|             |           |          |           |           |       |         |        |           |
| Int32_New   | .NET 10.0 | 1.688 ns | 0.0301 ns | 0.0267 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int32_New   | .NET 8.0  | 1.687 ns | 0.0214 ns | 0.0190 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int32_New   | .NET 9.0  | 1.648 ns | 0.0363 ns | 0.0339 ns |  0.98 |    0.02 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int32_From  | .NET 10.0 | 2.079 ns | 0.0274 ns | 0.0243 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int32_From  | .NET 8.0  | 2.876 ns | 0.0865 ns | 0.1093 ns |  1.38 |    0.05 | 0.0014 |      24 B |
| Int32_From  | .NET 9.0  | 1.966 ns | 0.0527 ns | 0.0493 ns |  0.95 |    0.03 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int64_New   | .NET 10.0 | 1.598 ns | 0.0229 ns | 0.0191 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int64_New   | .NET 8.0  | 1.599 ns | 0.0170 ns | 0.0159 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int64_New   | .NET 9.0  | 1.630 ns | 0.0180 ns | 0.0151 ns |  1.02 |    0.01 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int64_From  | .NET 10.0 | 1.883 ns | 0.0248 ns | 0.0207 ns |  1.00 |    0.01 | 0.0014 |      24 B |
| Int64_From  | .NET 8.0  | 2.634 ns | 0.0414 ns | 0.0387 ns |  1.40 |    0.02 | 0.0014 |      24 B |
| Int64_From  | .NET 9.0  | 1.922 ns | 0.0266 ns | 0.0223 ns |  1.02 |    0.02 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| String_New  | .NET 10.0 | 1.617 ns | 0.0136 ns | 0.0128 ns |  1.00 |    0.01 | 0.0014 |      24 B |
| String_New  | .NET 8.0  | 1.726 ns | 0.0626 ns | 0.0586 ns |  1.07 |    0.04 | 0.0014 |      24 B |
| String_New  | .NET 9.0  | 1.614 ns | 0.0317 ns | 0.0281 ns |  1.00 |    0.02 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| String_From | .NET 10.0 | 2.759 ns | 0.0330 ns | 0.0309 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| String_From | .NET 8.0  | 3.530 ns | 0.0540 ns | 0.0505 ns |  1.28 |    0.02 | 0.0014 |      24 B |
| String_From | .NET 9.0  | 2.764 ns | 0.0416 ns | 0.0369 ns |  1.00 |    0.02 | 0.0014 |      24 B |

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.7184/22H2/2022Update)
AMD Ryzen 9 9950X 4.30GHz, 1 CPU, 32 logical and 16 physical cores
.NET SDK 11.0.100-preview.2.26159.112
  [Host]    : .NET 10.0.7 (10.0.7, 10.0.726.21808), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.7 (10.0.7, 10.0.726.21808), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.26 (8.0.26, 8.0.2626.16921), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.15 (9.0.15, 9.0.1526.17522), X64 RyuJIT x86-64-v4


| Method      | Runtime   | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated |
|------------ |---------- |---------:|----------:|----------:|------:|--------:|-------:|----------:|
| Guid_New    | .NET 10.0 | 1.720 ns | 0.0203 ns | 0.0180 ns |  1.00 |    0.01 | 0.0019 |      32 B |
| Guid_New    | .NET 8.0  | 1.782 ns | 0.0311 ns | 0.0276 ns |  1.04 |    0.02 | 0.0019 |      32 B |
| Guid_New    | .NET 9.0  | 1.714 ns | 0.0168 ns | 0.0157 ns |  1.00 |    0.01 | 0.0019 |      32 B |
|             |           |          |           |           |       |         |        |           |
| Guid_From   | .NET 10.0 | 1.756 ns | 0.0304 ns | 0.0269 ns |  1.00 |    0.02 | 0.0019 |      32 B |
| Guid_From   | .NET 8.0  | 1.745 ns | 0.0239 ns | 0.0224 ns |  0.99 |    0.02 | 0.0019 |      32 B |
| Guid_From   | .NET 9.0  | 1.818 ns | 0.0246 ns | 0.0230 ns |  1.04 |    0.02 | 0.0019 |      32 B |
|             |           |          |           |           |       |         |        |           |
| Int32_New   | .NET 10.0 | 1.754 ns | 0.0333 ns | 0.0278 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int32_New   | .NET 8.0  | 1.717 ns | 0.0401 ns | 0.0375 ns |  0.98 |    0.03 | 0.0014 |      24 B |
| Int32_New   | .NET 9.0  | 1.714 ns | 0.0372 ns | 0.0348 ns |  0.98 |    0.02 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int32_From  | .NET 10.0 | 1.797 ns | 0.0678 ns | 0.0928 ns |  1.00 |    0.07 | 0.0014 |      24 B |
| Int32_From  | .NET 8.0  | 1.682 ns | 0.0359 ns | 0.0336 ns |  0.94 |    0.05 | 0.0014 |      24 B |
| Int32_From  | .NET 9.0  | 1.848 ns | 0.0680 ns | 0.0976 ns |  1.03 |    0.07 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int64_New   | .NET 10.0 | 1.715 ns | 0.0334 ns | 0.0296 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int64_New   | .NET 8.0  | 1.708 ns | 0.0261 ns | 0.0231 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| Int64_New   | .NET 9.0  | 1.789 ns | 0.0602 ns | 0.0534 ns |  1.04 |    0.03 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| Int64_From  | .NET 10.0 | 1.717 ns | 0.0557 ns | 0.0596 ns |  1.00 |    0.05 | 0.0014 |      24 B |
| Int64_From  | .NET 8.0  | 1.748 ns | 0.0471 ns | 0.0441 ns |  1.02 |    0.04 | 0.0014 |      24 B |
| Int64_From  | .NET 9.0  | 1.776 ns | 0.0696 ns | 0.0715 ns |  1.04 |    0.05 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| String_New  | .NET 10.0 | 1.711 ns | 0.0339 ns | 0.0301 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| String_New  | .NET 8.0  | 1.682 ns | 0.0229 ns | 0.0203 ns |  0.98 |    0.02 | 0.0014 |      24 B |
| String_New  | .NET 9.0  | 1.693 ns | 0.0350 ns | 0.0310 ns |  0.99 |    0.02 | 0.0014 |      24 B |
|             |           |          |           |           |       |         |        |           |
| String_From | .NET 10.0 | 1.637 ns | 0.0276 ns | 0.0258 ns |  1.00 |    0.02 | 0.0014 |      24 B |
| String_From | .NET 8.0  | 1.680 ns | 0.0306 ns | 0.0272 ns |  1.03 |    0.02 | 0.0014 |      24 B |
| String_From | .NET 9.0  | 1.720 ns | 0.0443 ns | 0.0415 ns |  1.05 |    0.03 | 0.0014 |      24 B |
```

## FAQ

__Why no records?__

Records (currently) have a few disadvantages, which is why they are not suitable for this type of class. For example, it is currently not possible to validly inherit `GetHashCode`. `sealed` on `GetHashCode` is only available if the record itself is `sealed`, which does not make sense here.

__Why no structs?__

Structs cannot be used in all scenarios, e.g. with ASP.NET Core Action parameters.

---

[MIT LICENSE](./LICENSE)
