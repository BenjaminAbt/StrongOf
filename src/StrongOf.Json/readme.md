# StrongOf.Json

System.Text.Json converters for [StrongOf](https://www.nuget.org/packages/StrongOf) types.

## Available Converters

| Converter | For |
|-----------|-----|
| `StrongGuidJsonConverter<T>` | `StrongGuid<T>` types |
| `StrongStringJsonConverter<T>` | `StrongString<T>` types |
| `StrongInt32JsonConverter<T>` | `StrongInt32<T>` types |
| `StrongInt64JsonConverter<T>` | `StrongInt64<T>` types |
| `StrongDecimalJsonConverter<T>` | `StrongDecimal<T>` types |
| `StrongCharJsonConverter<T>` | `StrongChar<T>` types |
| `StrongDateTimeJsonConverter<T>` | `StrongDateTime<T>` types |
| `StrongDateTimeOffsetJsonConverter<T>` | `StrongDateTimeOffset<T>` types |

## Usage - Attribute

Decorate properties directly:

```csharp
public sealed class UserDto
{
    [JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
    public UserId Id { get; set; }

    [JsonConverter(typeof(StrongStringJsonConverter<EmailAddress>))]
    public EmailAddress Email { get; set; }
}
```

## Usage - JsonSerializerOptions

Or register converters globally:

```csharp
JsonSerializerOptions options = new()
{
    WriteIndented = true,
    Converters =
    {
        new StrongGuidJsonConverter<UserId>(),
        new StrongStringJsonConverter<EmailAddress>(),
    }
};

string json = JsonSerializer.Serialize(user, options);
UserDto dto = JsonSerializer.Deserialize<UserDto>(json, options)!;
```

## Installation

```bash
dotnet add package StrongOf.Json
```

## GitHub

See https://github.com/BenjaminAbt/StrongOf