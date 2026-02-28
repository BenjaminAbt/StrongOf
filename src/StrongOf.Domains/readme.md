# StrongOf.Domains

Predefined strongly-typed domain models based on [StrongOf](https://www.nuget.org/packages/StrongOf) for common use cases.

The namespace structure is deliberately designed so that namespace names do **not** collide with common class names you might use in your own codebase (e.g. `Address`, `Person`, `Network`).

## Available Domain Types

| Namespace | Types |
|-----------|-------|
| `StrongOf.Domains.Identity` | `CorrelationId`, `EntityId`, `TenantId`, `Token`, `Username` |
| `StrongOf.Domains.People` | `Age`, `BirthYear`, `DateOfBirth`, `FirstName`, `FullName`, `LastName`, `MiddleName`, `PhoneNumber` |
| `StrongOf.Domains.Postal` | `City`, `CountryCode`, `CountryName`, `HouseNumber`, `Street`, `ZipCode` |
| `StrongOf.Domains.Geography` | `GeoCoordinate`, `Latitude`, `Longitude` |
| `StrongOf.Domains.Networking` | `EmailAddress`, `HostName`, `HttpMethod`, `IpAddress`, `MacAddress`, `Port`, `Url` |
| `StrongOf.Domains.Finance` | `CurrencyCode`, `Iban`, `Percentage` |
| `StrongOf.Domains.Commerce` | `Priority`, `Quantity`, `Sku` |
| `StrongOf.Domains.Localization` | `LanguageCode`, `Locale`, `TimeZoneId` |
| `StrongOf.Domains.Measurement` | `HeightCm`, `TemperatureCelsius`, `WeightKg` |
| `StrongOf.Domains.Media` | `ColorHex`, `FileExtension`, `FilePath`, `Isbn`, `MimeType`, `Slug` |
| `StrongOf.Domains.Software` | `SemVer` |

## Recommended: Global Usings

Add a single `GlobalUsings.cs` to your project to avoid per-file using directives:

```csharp
// GlobalUsings.cs
global using StrongOf.Domains.Identity;
global using StrongOf.Domains.People;
global using StrongOf.Domains.Postal;
global using StrongOf.Domains.Geography;
global using StrongOf.Domains.Networking;
global using StrongOf.Domains.Finance;
global using StrongOf.Domains.Commerce;
global using StrongOf.Domains.Localization;
global using StrongOf.Domains.Measurement;
global using StrongOf.Domains.Media;
global using StrongOf.Domains.Software;
```

## Key Features

- **Format validation** via `IsValidFormat()` for types with format constraints (e.g. `EmailAddress`, `CountryCode`, `Iban`)
- **Safe factory** via `TryCreate(string?, out T?)` - returns `false` instead of creating an invalid instance
- **Range validation** via `IsValidRange()` for numeric and geographic types
- **Conversion helpers** e.g. `ToMeters()`, `ToFahrenheit()`, `ToKelvin()`, `ToCultureInfo()`, `TryGetTimeZone()`
- **Case-insensitive equality** for `CountryCode`, `CurrencyCode`, `LanguageCode`, `Locale`, `FileExtension`, `MimeType`, `HostName`, `MacAddress`

## Usage

```csharp
// Create domain objects
EmailAddress email = new("user@example.com");
bool isValidEmail = email.IsValidFormat();

// Safe factory - does not create an invalid instance
bool created = EmailAddress.TryCreate("bad-input", out EmailAddress? result); // false

CountryCode country = new("US");
CurrencyCode currency = new("USD");
GeoCoordinate berlin = GeoCoordinate.From(52.52m, 13.405m);
bool inRange = berlin.IsValidRange();

// Temperature conversion
TemperatureCelsius temp = new(100m);
decimal fahrenheit = temp.ToFahrenheit(); // 212
```

## Installation

```bash
dotnet add package StrongOf.Domains
```
