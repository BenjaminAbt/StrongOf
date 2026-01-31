# StrongOf.Domains

Predefined domain models based on StrongOf for common use cases.

## Available Domain Types

### Contact & Identity
- `EmailAddress` - Email address validation
- `PhoneNumber` - Phone number representation
- `FirstName` - Person's first name
- `LastName` - Person's last name
- `FullName` - Person's full name
- `Username` - User account name

### Location
- `CountryCode` - ISO 3166-1 alpha-2 country code (e.g., "US", "DE")
- `ZipCode` - Postal/ZIP code
- `City` - City name
- `Street` - Street address

### Finance
- `CurrencyCode` - ISO 4217 currency code (e.g., "USD", "EUR")
- `Percentage` - Percentage value (0-100)

### Network
- `Url` - URL/URI representation
- `IpAddress` - IP address (v4 or v6)
- `MacAddress` - MAC address
- `HostName` - Network host name
- `Port` - Network port number

### Identifiers
- `EntityId` - Generic entity identifier (Guid)

## Usage

```csharp
using StrongOf.Domains;

// Create domain objects
var email = new EmailAddress("user@example.com");
var phone = new PhoneNumber("+1-555-123-4567");
var country = new CountryCode("US");
var currency = new CurrencyCode("USD");

// Access values
string emailValue = email.Value;
bool isValid = email.IsValidFormat();
```

## Installation

```bash
dotnet add package StrongOf.Domains
```
