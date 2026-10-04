# StrongOf

StrongOf helps to implement primitives as a strong type that represents a domain object (e.g. `UserId`, `EmailAddress`, etc.). It is a simple class that wraps a value and provides a few helper methods, preventing parameter-order bugs at compile time.

## NativeAOT

The core package supports NativeAOT and trimming on .NET 9 through .NET 11. Both hand-written
`IStrongOf<TValue, TSelf>.Create` implementations and source-generated factories use static dispatch.
The source generator executes at build time and is not a runtime dependency.

```csharp
using StrongOf.SourceGeneration;

[StrongGuid]
public sealed partial class UserId;
```

Source-generated classes must be top-level, non-generic and partial. `UserId<T>` is rejected with
`STRONG005`; the generic marker syntax `[Strong<Guid>]` remains supported.

Set `<PublishAot>true</PublishAot>` in the consuming application and publish for a specific RID.
The core library sets `IsAotCompatible=true`; its .NET 10+ builds additionally verify referenced AOT
metadata with `VerifyReferenceAotCompatibility=true`. This opt-in check is separate from the code analyzers.

The [native example](https://github.com/BenjaminAbt/StrongOf/tree/main/samples/StrongOf.NativeAot)
is published and executed in CI for .NET 9/10/11 on Windows x64 and Linux x64. Framework integration
packages do not inherit the core package's compatibility guarantee. For JSON, use the
[generated-context example](https://github.com/BenjaminAbt/StrongOf/blob/main/src/StrongOf.Json/readme.md).

## Available Base Types

| Base Class | Wraps |
|------------|-------|
| `StrongString<T>` | `string` |
| `StrongGuid<T>` | `Guid` |
| `StrongInt32<T>` | `int` |
| `StrongInt64<T>` | `long` |
| `StrongDecimal<T>` | `decimal` |
| `StrongDouble<T>` | `double` |
| `StrongChar<T>` | `char` |
| `StrongBoolean<T>` | `bool` |
| `StrongDateTime<T>` | `DateTime` |
| `StrongDateTimeOffset<T>` | `DateTimeOffset` |
| `StrongTimeSpan<T>` | `TimeSpan` |

## Quick Start

```csharp
// Define your types using the CRTP pattern
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId> { public static UserId Create(Guid value) => new(value); }
public sealed class Email(string value) : StrongString<Email>(value), IStrongOf<string, Email> { public static Email Create(string value) => new(value); }

// Instantiation
UserId userId = new(Guid.NewGuid());  // preferred - fastest
UserId userId2 = UserId.From(Guid.NewGuid()); // via static Create for generic scenarios

// Accessing the value
Guid rawId = userId.Value;

// Nullable factory
UserId? optional = UserId.From(nullableGuid);

// TryParse
bool parsed = UserId.TryParse("550e8400-...", out UserId? id);

// Strongly-typed comparison and sorting
bool equal = userId == userId2;
ids.Sort(); // works via IComparable<T>
```

## Generic TypeConverters

The `StrongOf` package ships generic `TypeConverter` implementations for each base type, usable directly in domain types via the `[TypeConverter]` attribute:

```csharp
[TypeConverter(typeof(StrongGuidTypeConverter<UserId>))]
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId> { public static UserId Create(Guid value) => new(value); }
```

Available converters: `StrongStringTypeConverter<T>`, `StrongGuidTypeConverter<T>`, `StrongInt32TypeConverter<T>`, `StrongInt64TypeConverter<T>`, `StrongDecimalTypeConverter<T>`, `StrongDoubleTypeConverter<T>`, `StrongCharTypeConverter<T>`, `StrongBooleanTypeConverter<T>`, `StrongDateTimeTypeConverter<T>`, `StrongDateTimeOffsetTypeConverter<T>`, `StrongTimeSpanTypeConverter<T>`.

The converters themselves can be instantiated directly in native applications. Reflection-based discovery
through `TypeDescriptor` has its own trimming requirements; a converter attribute does not make an arbitrary
consumer framework NativeAOT-compatible.

## DateTimeOffset ISO parsing

`FromIso8601` and `TryParseIso8601` accept timestamps with seconds, zero to seven fractional digits,
and `Z` or an explicit offset. The original offset is preserved. Output uses the round-trip `o` format.
This changes the previous implicit UTC normalization; call `Value.ToUniversalTime()` explicitly when needed.

## Interfaces

- `IStrongOf` - base marker interface
- `IStrongString`, `IStrongGuid`, `IStrongInt32`, etc. - for generic constraints
- `IValidatable` - for types that validate their own format via `IsValidFormat()`

## GitHub

See https://github.com/BenjaminAbt/StrongOf
