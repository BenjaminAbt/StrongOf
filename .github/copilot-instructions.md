# StrongOf - GitHub Copilot Instructions

## Project Overview

**StrongOf** is a .NET library that provides strongly-typed primitives for domain-driven design. It enables developers to wrap primitive types (string, Guid, int, etc.) in domain-specific types to improve type safety, code clarity, and prevent parameter order mistakes at compile-time.

## Architecture & Key Concepts

### Core Type Hierarchy

- `StrongOf<TTarget, TStrong>` - Base abstract class for all strong types
- `StrongString<TStrong>` - Strong type wrapper for `string`
- `StrongGuid<TStrong>` - Strong type wrapper for `Guid`
- `StrongInt32<TStrong>` - Strong type wrapper for `int`
- `StrongInt64<TStrong>` - Strong type wrapper for `long`
- `StrongDecimal<TStrong>` - Strong type wrapper for `decimal`
- `StrongChar<TStrong>` - Strong type wrapper for `char`
- `StrongDateTime<TStrong>` - Strong type wrapper for `DateTime`
- `StrongDateTimeOffset<TStrong>` - Strong type wrapper for `DateTimeOffset`

### Package Structure

| Package | Purpose |
|---------|---------|
| `StrongOf` | Core library with all strong type base classes (ships the source generator inside the NuGet) |
| `StrongOf.SourceGenerators` | Roslyn incremental source generator – emits constructor, base-type, and AOT-safe `Create` member |
| `StrongOf.Domains` | Ready-to-use concrete domain types (EmailAddress, Url, EntityId, etc.) |
| `StrongOf.Json` | System.Text.Json converters for serialization |
| `StrongOf.AspNetCore` | Model binders for ASP.NET Core |
| `StrongOf.FluentValidation` | Validation extensions for FluentValidation |

## Code Style & Conventions

- Respect the `.editorconfig` settings for consistent formatting
- Use explicit access modifiers (e.g., `public`, `private`) for all members
- Never use `var` for strong type declarations; always use explicit types for clarity

### Creating Strong Types

**Preferred: Source Generator (AOT safe, zero boilerplate)**

Add the attribute and declare the class as `partial`. The Roslyn generator emits the primary constructor, base-type inheritance, and `Create()` automatically:

```csharp
using StrongOf.SourceGeneration;

[StrongGuid]   
public partial class UserId;

[StrongString]
public partial class Email;

[StrongDecimal]
public partial class Amount;

// Generic syntax (equivalent):
[Strong<Guid>]
public partial class OrderId;

[Strong<string>]
public partial class Slug;
```

**Manual: Hand-written types**

When you need custom logic in the class body, implement the CRTP pattern manually. The class **must** also implement `IStrongOf<TTarget, TSelf>` and provide `Create()` – required by the base-class constraint so `From()` dispatches without reflection:

```csharp
// Correct: sealed class with primary constructor + IStrongOf<,> + Create
public sealed class UserId(Guid value) 
    : StrongGuid<UserId>(value), IStrongOf<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);
}

public sealed class Email(string value) 
    : StrongString<Email>(value), IStrongOf<string, Email>
{
    public static Email Create(string value) => new(value);
}
```

**CRITICAL: No implicit conversion.** Never add `implicit operator` from the underlying type to a strong type. All conversions must remain explicit to prevent parameter-order bugs at runtime:

```csharp
// ✅ Correct - explicit, compile-time safe
Email email = new Email("user@example.com");
Email? email = Email.From("user@example.com");

// ❌ Wrong - never add this
public static implicit operator Email(string value) => new(value);
```

### Instantiation Patterns

```csharp
// Prefer: Direct instantiation with new (most performant)
UserId userId = new UserId(Guid.NewGuid());

// Alternative: Static From method (uses static abstract IStrongOf<,>.Create - AOT safe)
UserId userId = UserId.From(Guid.NewGuid());

// From nullable values
UserId? userId = UserId.FromGuid(nullableGuid);
Email? email = Email.FromNullable(nullableString);

// From strings (for Guid types)
UserId? userId = UserId.FromString("550e8400-e29b-41d4-a716-446655440000");

// TryParse (all types except StrongChar)
if (UserId.TryParse("550e8400-e29b-41d4-a716-446655440000", out UserId? userId))
{
    // Use userId
}
```

### Accessing Values

```csharp
// Use the Value property
Guid rawGuid = userId.Value;
string rawEmail = email.Value;

// Or use typed accessor methods
Guid rawGuid = userId.AsGuid();
string rawEmail = email.AsString();
```

### Collection Conversion

```csharp
// Efficiently convert a collection of raw values to strong types
Guid[] guids = [Guid.NewGuid(), Guid.NewGuid()];
List<UserId>? userIds = UserId.From(guids); // pre-sized internally

// Also supports IEnumerable<T>, List<T>, arrays; null-safe (returns null for null input)
List<UserId>? nullResult = UserId.From((IEnumerable<Guid>?)null); // null
```

## Performance Guidelines

1. **Prefer `new()` over `From()`** - Direct instantiation is faster as it avoids static-abstract interface dispatch overhead
2. **Use `From()` for generic scenarios** - When you need to create instances in generic code
3. **Avoid boxing** - Strong types are reference types; use generics to avoid boxing operations
4. **Pre-size collections** - When converting lists, the library automatically pre-sizes based on source collection
5. **Use `Equals(TStrong?)` for comparisons** - Prefer the typed method over `Equals(object?)` for better performance
6. **Use `CompareTo(TStrong?)` for sorting** - Enables efficient sorting without boxing

## Comparison & Equality

All strong types support comparison and equality:

```csharp
// Equality (IEquatable<T>)
UserId id1 = new UserId(guid);
UserId id2 = new UserId(guid);
bool areEqual = id1.Equals(id2);  // true
bool useOperator = id1 == id2;    // true

// Comparison (IComparable<T>)
Priority low = new Priority(1);
Priority high = new Priority(10);
int compare = low.CompareTo(high); // negative value
bool isLess = low < high;          // true

// Sorting
List<UserId> ids = new List<UserId> { userId3, userId1, userId2 };
ids.Sort(); // Works because of IComparable<T>
```

## Testing Conventions

- Tests use **xUnit v3** framework
- Test files follow naming pattern: `{TypeName}Tests.cs` or `{TypeName}_{Feature}_Tests.cs`
- Use descriptive test method names that explain the scenario
- Separate test files for operators, methods, and properties

Example test structure:
```csharp
public class StrongGuidTests
{
    private sealed class TestId(Guid value) : StrongGuid<TestId>(value), IStrongOf<Guid, TestId>
    {
        public static TestId Create(Guid value) => new(value);
    }

    [Fact]
    public void From_WithValidGuid_ReturnsStrongType()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        TestId? result = TestId.From(guid);

        // Assert
        Assert.Equal(guid, result.Value);
    }
}
```

## Strong Utility Class

`Strong` provides static helper methods for null/empty checks:

```csharp
Strong.IsNull(strong)              // true if strong is null
Strong.IsNotNull(strong)           // true if strong is not null
Strong.IsNullOrEmpty(strong)       // true if null or empty string
Strong.HasValue(strong)            // true if not null and not empty string
Strong.IsNotNullOrEmpty(strong)    // true if not null and not empty string
```

## Domain Types (StrongOf.Domains)

When creating concrete domain types in `StrongOf.Domains`, follow this pattern:

```csharp
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(UserIdTypeConverter))]
public sealed class UserId(Guid value) : StrongGuid<UserId>(value), IStrongOf<Guid, UserId>
{
    public static UserId Create(Guid value) => new(value);

    // Domain-specific methods only - no redundant overrides
    public bool HasValue() => Value != Guid.Empty;
}

public sealed class UserIdTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(Guid) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            Guid g => new UserId(g),
            string s when Guid.TryParse(s, out Guid parsed) => new UserId(parsed),
            _ => base.ConvertFrom(context, culture, value)
        };
}
```

Always include:
- `[DebuggerDisplay("{Value}")]` - for debugger readability
- A `TypeConverter` nested class - for MVC model binding, designer tools, and configuration
- Validation methods (`IsValidFormat()`, `HasValue()`) where applicable
- Regex via `[GeneratedRegex(...)]` for format validation (avoids runtime compilation)

## JSON Serialization

Use the appropriate converter for each strong type:

```csharp
// Attribute-based
[JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
public UserId Id { get; set; }

// Options-based
var options = new JsonSerializerOptions
{
    Converters = { new StrongGuidJsonConverter<UserId>() }
};
```

Available converters:
- `StrongGuidJsonConverter<T>`
- `StrongStringJsonConverter<T>`
- `StrongInt32JsonConverter<T>`
- `StrongInt64JsonConverter<T>`
- `StrongDecimalJsonConverter<T>`
- `StrongCharJsonConverter<T>`
- `StrongDateTimeJsonConverter<T>`
- `StrongDateTimeOffsetJsonConverter<T>`

## ASP.NET Core Integration

### MVC Model Binders

Register the built-in StrongOf model binder provider via `AddStrongOfModelBinderProvider()`.
Pass types explicitly (simplest for most apps):

```csharp
using StrongOf.AspNetCore.Mvc;

// Program.cs
builder.Services.AddControllers(options =>
    options.AddStrongOfModelBinderProvider(
        typeof(UserId),
        typeof(EmailAddress)));
```

Or scan an entire assembly automatically:

```csharp
using StrongOf.AspNetCore.Mvc;

builder.Services.AddControllers(options =>
    options.AddStrongOfModelBinderProviderFromAssemblies(typeof(UserId).Assembly));
```

Available binders in `StrongOf.AspNetCore.Mvc`: `StrongGuidBinder<T>`, `StrongStringBinder<T>`, `StrongInt32Binder<T>`, `StrongInt64Binder<T>`, `StrongDecimalBinder<T>`, `StrongDoubleBinder<T>`, `StrongCharBinder<T>`, `StrongBooleanBinder<T>`, `StrongDateTimeBinder<T>`, `StrongDateTimeOffsetBinder<T>`, `StrongTimeSpanBinder<T>`.

### Minimal APIs

All strong types implement `IParsable<TSelf>` and work as route/query parameters out of the box:

```csharp
using StrongOf.AspNetCore.MinimalApis;

app.MapGet("/users/{id}", (UserId id) => Results.Ok(id));

// Automatic validation of IValidatable parameters
app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
   .WithStrongOfValidation();
```

### OpenAPI Schema Transformer

> Requires .NET 9.0 or later

Maps strong types to their underlying primitive types in OpenAPI docs:

```csharp
using StrongOf.AspNetCore.OpenApi;

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<StrongOfSchemaTransformer>();
});
```

## FluentValidation

Use `HasValue()` instead of `NotNull()` for strong types:

```csharp
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.UserId)
            .HasValue()
            .WithMessage("User ID is required.");
    }
}
```

## Entity Framework Core

Create explicit value converters (no generic base class provided):

```csharp
public class UserIdValueConverter : ValueConverter<UserId, Guid>
{
    public UserIdValueConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value, value => new UserId(value), mappingHints) { }
}
```

## Build & Development

### Prerequisites
- .NET SDK 9.0, 10.0, NET 11.0 (for source generators)
- Just command runner (optional, for convenience commands)

### Common Commands

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true

# Run benchmarks
dotnet run --project perf/StrongOf.Benchmarks/StrongOf.Benchmarks.csproj -c Release

# Using Just
just build
just test
just bench
just ci        # Full CI pipeline
```

### Target Frameworks
- .NET 9.0
- .NET 10.0
- .NET 11.0

## Important Design Decisions

1. **Classes over Records** - Records cannot properly inherit `GetHashCode` in unsealed scenarios
2. **Classes over Structs** - Structs require a default constructor, which breaks ASP.NET Core model binding (action parameters) and EF Core value converters. Always use sealed classes.
3. **Roslyn Source Generator** - Shipped inside the `StrongOf` NuGet; emits constructor, base-type, and `Create()` for zero-boilerplate AOT-safe types via `[StrongGuid] public partial class UserId;`
4. **Static Abstract Interface Dispatch** - `From()` calls `TStrong.Create(value)` via `IStrongOf<TTarget,TSelf>.Create` (C# 11 static abstract members) – no reflection, no delegate caching, fully AOT safe
5. **No Implicit Conversion** - Never add `implicit operator` from primitive to strong type; this is the core safety guarantee
6. **Nullable Annotations** - Full nullable reference type support with `[NotNullIfNotNull]` attributes
7. **Aggressive Optimization** - All methods use `[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]`
8. **IEquatable & IComparable** - All strong types implement `IEquatable<TStrong>` and `IComparable<TStrong>` for proper sorting and equality

## Interfaces

Each strong type implements corresponding interfaces for generic constraints:

- `IStrongOf` - Base interface for all strong types
- `IStrongString` - For string-based strong types
- `IStrongGuid` - For Guid-based strong types
- `IStrongInt32` - For int-based strong types
- `IStrongInt64` - For long-based strong types
- `IStrongDecimal` - For decimal-based strong types
- `IStrongChar` - For char-based strong types
- `IStrongDateTime` - For DateTime-based strong types
- `IStrongDateTimeOffset` - For DateTimeOffset-based strong types

Additionally, all strong types implement:
- `IComparable` - Non-generic interface for sorting
- `IComparable<TStrong>` - Generic interface for type-safe sorting
- `IEquatable<TStrong>` - Generic interface for type-safe equality

## File Organization

```
src/
├── StrongOf/                    # Core library
│   ├── Strong{Type}.cs          # Main implementation
│   ├── Strong{Type}.Operators.cs # Operator overloads
│   ├── Strong{Type}.Methods.cs   # Additional methods (if any)
│   └── IStrong{Type}.cs         # Interface definition
├── StrongOf.Domains/            # Concrete domain types
│   ├── Each concrete type is a single .cs file
│   └── Each file includes the type + its TypeConverter
├── StrongOf.Json/               # JSON converters
├── StrongOf.AspNetCore/         # ASP.NET Core integration
│   ├── Mvc/                     # MVC model binders
│   ├── MinimalApis/             # Minimal API extensions
│   └── OpenApi/                 # OpenAPI schema transformer
└── StrongOf.FluentValidation/   # Validation extensions

tests/
├── StrongOf.UnitTests/
├── StrongOf.Domains.UnitTests/
├── StrongOf.Json.UnitTests/
└── StrongOf.FluentValidation.UnitTests/

perf/
└── StrongOf.Benchmarks/         # Performance benchmarks
```

## Guidelines (MANDATORY)

1. Ensure all tests pass with `dotnet test`
2. Check code formatting with `dotnet format --verify-no-changes`
3. Add code documentation comments and add / update XML docs as needed
4. Add tests for new functionality
5. Update benchmarks if performance-critical code is changed
6. Maintain backward compatibility
7. Follow existing naming conventions and code style
8. Ensure no errors or warnings in the build
9. Verify all build and test commands succeed
10. Fix any new warnings or errors introduced by changes
