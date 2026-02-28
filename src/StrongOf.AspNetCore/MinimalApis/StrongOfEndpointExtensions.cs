// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace StrongOf.AspNetCore.MinimalApis;

/// <summary>
/// Provides extension methods for configuring StrongOf validation on Minimal API endpoints.
/// </summary>
public static class StrongOfEndpointExtensions
{
    /// <summary>
    /// Adds the <see cref="StrongOfValidationFilter"/> to the endpoint,
    /// which validates all <see cref="IValidatable"/> parameters before execution.
    /// </summary>
    /// <param name="builder">The route handler builder to configure.</param>
    /// <returns>The <paramref name="builder"/> for chaining.</returns>
    /// <example>
    /// <code>
    /// app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
    ///    .WithStrongOfValidation();
    /// </code>
    /// </example>
    public static RouteHandlerBuilder WithStrongOfValidation(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<StrongOfValidationFilter>();
    }
}
