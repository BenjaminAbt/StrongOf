# StrongOf.FluentValidation

FluentValidation extensions for [StrongOf](https://www.nuget.org/packages/StrongOf) types.

## Why Not `NotNull()`?

For strong types, `NotNull()` only checks the object reference. StrongOf provides `HasValue()` which covers both nullability and empty values:

```csharp
public sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.UserId)
            .HasValue()            // not NotNull()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Email)
            .HasValue()
            .WithMessage("Email is required.");
    }
}
```

## Available Extensions

| Method | Applies To | Description |
|--------|-----------|-------------|
| `HasValue()` | All strong types | Not null and, for string types, not empty/whitespace |

## Installation

```bash
dotnet add package StrongOf.FluentValidation
```

## GitHub

See https://github.com/BenjaminAbt/StrongOf