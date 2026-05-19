# StrongOf.Json

System.Text.Json converters for [StrongOf](https://NuBrowse.com/packages/StrongOf) types.

All converters are fully **Native AOT** and **trim-safe** — no reflection, no `Expression.Compile`, no factory auto-discovery.



## Recommended Usage — JsonSerializerOptions

Register converters explicitly when you prefer a central setup:

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
## Usage — Attribute

Decorate each property directly. This is AOT-safe and requires zero setup:

```csharp
public sealed class UserDto
{
    [JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
    public UserId Id { get; set; }

    [JsonConverter(typeof(StrongStringJsonConverter<EmailAddress>))]
    public EmailAddress Email { get; set; }
}
```

## Available Converters

| Converter | For |
|-----------|-----|
| `StrongBooleanJsonConverter<T>` | `StrongBoolean<T>` types |
| `StrongGuidJsonConverter<T>` | `StrongGuid<T>` types |
| `StrongStringJsonConverter<T>` | `StrongString<T>` types |
| `StrongInt32JsonConverter<T>` | `StrongInt32<T>` types |
| `StrongInt64JsonConverter<T>` | `StrongInt64<T>` types |
| `StrongDecimalJsonConverter<T>` | `StrongDecimal<T>` types |
| `StrongDoubleJsonConverter<T>` | `StrongDouble<T>` types |
| `StrongCharJsonConverter<T>` | `StrongChar<T>` types |
| `StrongDateTimeJsonConverter<T>` | `StrongDateTime<T>` types |
| `StrongDateTimeOffsetJsonConverter<T>` | `StrongDateTimeOffset<T>` types |
| `StrongTimeSpanJsonConverter<T>` | `StrongTimeSpan<T>` types |


## Installation

```bash
dotnet add package StrongOf.Json
```

## GitHub

See https://github.com/BenjaminAbt/StrongOf
