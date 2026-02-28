# StrongOf.MinimalApis

Minimal API integration for the StrongOf strongly-typed primitives library.

All strong types implement `IParsable<TSelf>` and are automatically supported as route/query parameters in ASP.NET Core Minimal APIs.

## Usage

```csharp
// Strong types work as route parameters out of the box
app.MapGet("/users/{id}", (UserId id) => Results.Ok(id));

// Register the endpoint filter for automatic validation of IValidatable types
app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
   .AddEndpointFilter<StrongOfValidationFilter>();
```
