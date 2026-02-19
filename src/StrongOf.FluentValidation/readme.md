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

### Common (all strong types)

| Method | Applies To | Description |
|--------|-----------|-------------|
| `HasValue()` | All strong types | Not null and, for string types, not empty/whitespace |
| `IsEqualTo(expr)` | All strong types | Must equal another property of the same type |
| `IsNotEqualTo(expr)` | All strong types | Must not equal another property of the same type |

### StrongString

| Method | Description |
|--------|-------------|
| `HasMinimumLength(int)` | Minimum character length |
| `HasMaximumLength(int)` | Maximum character length |
| `IsRegexMatch(Regex)` | Must match a regular expression |
| `AllowedChars(ICollection<char>, string)` | Must only contain characters from the allowed set |

### StrongInt32 / StrongInt64

| Method | Description |
|--------|-------------|
| `HasMinimum(int/long)` | Value must be ≥ min |
| `HasMaximum(int/long)` | Value must be ≤ max |
| `HasRange(int/long, int/long)` | Value must be within [min, max] |

### StrongDateTime / StrongDateTimeOffset

| Method | Description |
|--------|-------------|
| `HasMinimum(DateTime/DateTimeOffset)` | Value must be ≥ min |
| `HasMaximum(DateTime/DateTimeOffset)` | Value must be ≤ max |
| `HasRange(DateTime/DateTimeOffset, ...)` | Value must be within [min, max] |

## Installation

```bash
dotnet add package StrongOf.FluentValidation
```

## GitHub

See https://github.com/BenjaminAbt/StrongOf