# StrongOf

StrongOf helps to implement primitives as a strong type that represents a domain object (e.g. `UserId`, `EmailAddress`, etc.). It is a simple class that wraps a value and provides a few helper methods, preventing parameter-order bugs at compile time.

## Available Base Types

| Base Class | Wraps |
|------------|-------|
| `StrongString<T>` | `string` |
| `StrongGuid<T>` | `Guid` |
| `StrongInt32<T>` | `int` |
| `StrongInt64<T>` | `long` |
| `StrongDecimal<T>` | `decimal` |
| `StrongChar<T>` | `char` |
| `StrongDateTime<T>` | `DateTime` |
| `StrongDateTimeOffset<T>` | `DateTimeOffset` |

## Quick Start

```csharp
// Define your types using the CRTP pattern
public sealed class UserId(Guid value) : StrongGuid<UserId>(value) { }
public sealed class Email(string value) : StrongString<Email>(value) { }

// Instantiation
UserId userId = new(Guid.NewGuid());  // preferred - fastest
UserId userId2 = UserId.From(Guid.NewGuid()); // via cached factory delegate

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
public sealed class UserId(Guid value) : StrongGuid<UserId>(value) { }
```

Available converters: `StrongStringTypeConverter<T>`, `StrongGuidTypeConverter<T>`, `StrongInt32TypeConverter<T>`, `StrongInt64TypeConverter<T>`, `StrongDecimalTypeConverter<T>`, `StrongDateTimeTypeConverter<T>`.

## Interfaces

- `IStrongOf` - base marker interface
- `IStrongString`, `IStrongGuid`, `IStrongInt32`, etc. - for generic constraints
- `IValidatable` - for types that validate their own format via `IsValidFormat()`

## GitHub

See https://github.com/BenjaminAbt/StrongOf