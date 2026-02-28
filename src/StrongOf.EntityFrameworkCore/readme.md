# StrongOf.EntityFrameworkCore

Entity Framework Core value converters for [StrongOf](https://www.nuget.org/packages/StrongOf/) strongly-typed primitives.
Seamlessly persist strong types like `UserId`, `Email`, or `Amount` to and from the database - without losing type safety.

## Installation

```bash
dotnet add package StrongOf.EntityFrameworkCore
```

## Quick Start

```csharp
// Your domain types
public sealed class UserId(Guid value) : StrongGuid<UserId>(value);
public sealed class Email(string value) : StrongString<Email>(value);
public sealed class Amount(decimal value) : StrongDecimal<Amount>(value);

// Your entity
public class Order
{
    public UserId Id { get; set; } = null!;
    public Email CustomerEmail { get; set; } = null!;
    public Amount Total { get; set; } = null!;
}
```

## Usage

### Option 1: Register Converters Once with `ConfigureConventions` (Recommended)

The best approach is to register converters **once** in `ConfigureConventions`, so every property of that type is automatically converted - no per-property configuration needed:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Register once - applies to ALL properties of these types across ALL entities
        configurationBuilder.Properties<UserId>()
            .HaveConversion<StrongOfValueConverter<UserId, Guid>>();

        configurationBuilder.Properties<Email>()
            .HaveConversion<StrongOfValueConverter<Email, string>>();

        configurationBuilder.Properties<Amount>()
            .HaveConversion<StrongOfValueConverter<Amount, decimal>>();
    }
}
```

> **This is the recommended approach.** You register each strong type exactly once, and EF Core applies the converter everywhere that type appears - in any entity, in any `DbSet`. No per-property calls to `HasConversion` or `HasStrongOfConversion` are needed.

### Option 2: Per-Property Configuration with `HasStrongOfConversion`

If you prefer explicit per-property control (e.g., only certain properties should be converted, or different column types):

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Order>(entity =>
    {
        entity.Property(e => e.Id)
              .HasStrongOfConversion<UserId, Guid>();

        entity.Property(e => e.CustomerEmail)
              .HasStrongOfConversion<Email, string>()
              .HasMaxLength(256);

        entity.Property(e => e.Total)
              .HasStrongOfConversion<Amount, decimal>()
              .HasPrecision(18, 2);
    });
}
```

### Option 3: Direct Converter Instance

You can also instantiate a converter directly, e.g. for use in custom model building logic:

```csharp
StrongOfValueConverter<UserId, Guid> converter = new();

// Use with HasConversion directly
modelBuilder.Entity<Order>()
    .Property(e => e.Id)
    .HasConversion(converter);
```

## Supported Types

| Strong Type Base | Database Type | Converter |
|---|---|---|
| `StrongGuid<T>` | `uniqueidentifier` / `uuid` | `StrongOfValueConverter<T, Guid>` |
| `StrongString<T>` | `nvarchar` / `text` | `StrongOfValueConverter<T, string>` |
| `StrongInt32<T>` | `int` | `StrongOfValueConverter<T, int>` |
| `StrongInt64<T>` | `bigint` | `StrongOfValueConverter<T, long>` |
| `StrongDecimal<T>` | `decimal` | `StrongOfValueConverter<T, decimal>` |
| `StrongChar<T>` | `nchar(1)` / `char(1)` | `StrongOfValueConverter<T, char>` |
| `StrongDateTime<T>` | `datetime2` / `timestamp` | `StrongOfValueConverter<T, DateTime>` |
| `StrongDateTimeOffset<T>` | `datetimeoffset` / `timestamptz` | `StrongOfValueConverter<T, DateTimeOffset>` |

## ⚠️ EF Core LINQ Limitations

EF Core value converters work transparently for basic CRUD operations (insert, update, delete, read).
However, there are important **LINQ query limitations** you should be aware of.

### 1. No Translation of Custom Methods or Properties

EF Core cannot translate methods or properties on your strong types into SQL. Only the underlying primitive value is known to the database.

```csharp
// ❌ Will NOT work - EF Core cannot translate .Value into SQL
var users = await db.Users
    .Where(u => u.Id.Value == someGuid)
    .ToListAsync();

// ✅ Works - compare against another strong type instance
UserId targetId = new(someGuid);
var users = await db.Users
    .Where(u => u.Id == targetId)
    .ToListAsync();
```

### 2. Equality and Comparison

EF Core translates `==`, `!=`, and comparison operators correctly **when both sides are the same strong type** (the converter unwraps both to the primitive for SQL generation):

```csharp
UserId targetId = new(someGuid);

// ✅ Works - EF Core unwraps both sides
var user = await db.Users
    .FirstOrDefaultAsync(u => u.Id == targetId);

// ✅ Works - ordering is translated correctly
var sorted = await db.Users
    .OrderBy(u => u.Id)
    .ToListAsync();
```

### 3. No `Contains` with Mixed Types

When using `Contains` with a list of values, the list must be of the **strong type**, not the primitive:

```csharp
Guid[] rawGuids = [guid1, guid2, guid3];
List<UserId> userIds = UserId.From(rawGuids)!;

// ❌ Does NOT work - cannot mix Guid[] with UserId property
var users = await db.Users
    .Where(u => rawGuids.Contains(u.Id))
    .ToListAsync();

// ✅ Works - same types on both sides
var users = await db.Users
    .Where(u => userIds.Contains(u.Id))
    .ToListAsync();
```

### 4. No SQL Translation for `ToString()` or Formatting

Calls to `ToString()`, `IFormattable.ToString(format, provider)`, or any string formatting on strong types are **not translatable** to SQL:

```csharp
// ❌ Will throw or evaluate client-side
var emails = await db.Users
    .Select(u => u.Email.ToString())
    .ToListAsync();

// ✅ Select the strong type, then format in memory
var users = await db.Users.ToListAsync();
var emails = users.Select(u => u.Email.Value).ToList();
```

### 5. Aggregations Work on Numeric Types

EF Core correctly unwraps numeric strong types for aggregate functions:

```csharp
// ✅ Works - EF Core unwraps Amount to decimal for SUM
decimal? total = await db.Orders
    .SumAsync(o => (decimal?)o.Total);
```

> **Note:** You may need to cast to the nullable primitive type (as shown above) because EF Core aggregate methods expect the underlying type, not the strong type.

### 6. General Rule of Thumb

| Operation | Works? | Notes |
|---|---|---|
| Insert / Update / Delete | ✅ | Fully transparent |
| `Where` with `==` / `!=` | ✅ | Both sides must be the same strong type |
| `OrderBy` / `ThenBy` | ✅ | Translated to SQL |
| `Contains` (same type) | ✅ | List and property must be the same strong type |
| `Contains` (mixed types) | ❌ | Use `StrongType.From(...)` to convert the list |
| `.Value` in LINQ | ❌ | Not translatable - use strong type comparisons |
| `ToString()` in LINQ | ❌ | Not translatable - format client-side |
| Aggregations (`Sum`, `Avg`) | ⚠️ | Cast to nullable primitive may be required |
| Custom methods in LINQ | ❌ | No SQL translation for user-defined methods |

### Best Practices

1. **Use `ConfigureConventions`** to register converters once - avoid repetitive per-property configuration.
2. **Compare strong types against strong types** in LINQ, not against raw primitives.
3. **Convert lists before querying** - use `StrongType.From(rawValues)` to create a `List<TStrong>` for `Contains` queries.
4. **Format and transform client-side** - select entities first, then access `.Value` or call `.ToString()` in memory.
5. **Test your queries** - always verify that your LINQ queries translate to SQL correctly by checking the generated SQL (e.g., via logging or `ToQueryString()`).
