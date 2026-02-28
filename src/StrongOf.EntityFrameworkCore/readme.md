# StrongOf.EntityFrameworkCore

Entity Framework Core value converters for [StrongOf](https://www.nuget.org/packages/StrongOf/) strongly-typed primitives.

## Installation

```bash
dotnet add package StrongOf.EntityFrameworkCore
```

## Usage

### Value Converters

Apply value converters per property in your `DbContext.OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>(entity =>
    {
        entity.Property(e => e.Id)
              .HasStrongOfConversion<UserId, Guid>();

        entity.Property(e => e.Email)
              .HasStrongOfConversion<Email, string>();
    });
}
```

### Standalone Value Converters

You can also create value converters directly:

```csharp
StrongOfValueConverter<UserId, Guid> converter = new();
```
