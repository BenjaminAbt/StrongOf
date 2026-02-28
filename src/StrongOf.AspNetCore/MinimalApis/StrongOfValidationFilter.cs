// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Http;

namespace StrongOf.AspNetCore.MinimalApis;

/// <summary>
/// An endpoint filter that validates <see cref="IValidatable"/> strong type parameters
/// before the endpoint handler is invoked.
/// </summary>
/// <remarks>
/// <para>
/// When added to a Minimal API endpoint, this filter inspects all arguments for
/// <see cref="IValidatable"/> implementations and returns a <c>400 Bad Request</c>
/// if any fail validation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// app.MapPost("/users", (EmailAddress email) => Results.Ok(email))
///    .AddEndpointFilter&lt;StrongOfValidationFilter&gt;();
/// </code>
/// </example>
public sealed class StrongOfValidationFilter : IEndpointFilter
{
    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (object? argument in context.Arguments)
        {
            if (argument is IValidatable validatable && !validatable.IsValidFormat())
            {
                return Results.BadRequest($"Invalid value for {argument.GetType().Name}.");
            }
        }

        return await next(context).ConfigureAwait(false);
    }
}
