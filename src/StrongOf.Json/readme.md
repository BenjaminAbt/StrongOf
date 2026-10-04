# StrongOf.Json

System.Text.Json converters for [StrongOf](https://NuBrowse.com/packages/StrongOf) types.
The converters support NativeAOT and trimming. Applications must supply generated JSON metadata as shown below.

## NativeAOT and source generation

Install `StrongOf.Json` and use a `JsonSerializerContext` for every serialized DTO. Put the converter attribute
on the strong type itself when StrongOf generates that type in the same project:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using StrongOf.Json;
using StrongOf.SourceGeneration;

UserDto user = new() { Id = UserId.From(Guid.NewGuid()) };
string json = JsonSerializer.Serialize(user, AppJsonContext.Default.UserDto);
UserDto restored = JsonSerializer.Deserialize(json, AppJsonContext.Default.UserDto)!;

[StrongGuid]
[JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
public sealed partial class UserId;

public sealed class UserDto
{
    public UserId Id { get; set; } = null!;
}

[JsonSerializable(typeof(UserDto))]
internal partial class AppJsonContext : JsonSerializerContext;
```

Set these properties in the application project:

```xml
<PropertyGroup>
  <PublishAot>true</PublishAot>
  <JsonSerializerIsReflectionEnabledByDefault>false</JsonSerializerIsReflectionEnabledByDefault>
</PropertyGroup>
```

The StrongOf and System.Text.Json generators cannot inspect each other's generated implementation in the same
compilation. A converter only on a DTO property is therefore insufficient for a newly generated strong type:
the JSON generator can otherwise emit a call to a nonexistent parameterless constructor (`CS7036`). The
explicit converter attribute on the strong type avoids this. Property-level converters remain useful for
hand-written types or types already compiled in another assembly, such as `StrongOf.Domains`.

## Options-based registration

Options still need a generated metadata resolver. For a type-level converter as above:

```csharp
JsonSerializerOptions options = new()
{
    WriteIndented = true,
    TypeInfoResolver = AppJsonContext.Default,
};
AppJsonContext context = new(options);
string json = JsonSerializer.Serialize(user, context.UserDto);
```

For hand-written or precompiled strong types, converters may also be added to `options.Converters`.
Registering converters alone does not supply metadata for containing DTOs. Avoid reflection-based serializer
overloads in NativeAOT applications; pass the generated `JsonTypeInfo<T>` or `JsonSerializerContext` instead.

## Available converters and wire format

| Converter | JSON representation |
|-----------|---------------------|
| `StrongBooleanJsonConverter<T>` | Boolean; also accepts strings when reading |
| `StrongGuidJsonConverter<T>` | GUID string |
| `StrongStringJsonConverter<T>` | String |
| `StrongInt32JsonConverter<T>` | Numeric string |
| `StrongInt64JsonConverter<T>` | Numeric string |
| `StrongDecimalJsonConverter<T>` | Numeric string |
| `StrongDoubleJsonConverter<T>` | Numeric string |
| `StrongCharJsonConverter<T>` | Single-character string |
| `StrongDateTimeJsonConverter<T>` | ISO date-time string |
| `StrongDateTimeOffsetJsonConverter<T>` | ISO date-time string, preserving the offset |
| `StrongTimeSpanJsonConverter<T>` | TimeSpan string in `c` format |

DateTimeOffset input accepts seconds with zero to seven fractional digits and either `Z` or an explicit
offset (`+02:00`, `-05:30`). Output uses the round-trip `o` format. An input without an offset is rejected.
The existing converters return `null` for unparseable string values; callers must validate required fields.

## Executable example

The [NativeAOT sample](https://github.com/BenjaminAbt/StrongOf/tree/main/samples/StrongOf.NativeAot)
publishes and runs round trips for all eleven converters with JSON reflection disabled.

## Installation

```bash
dotnet add package StrongOf.Json
```
