// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Http;
using NSubstitute;
using StrongOf.AspNetCore.MinimalApis;
using Xunit;

namespace StrongOf.AspNetCore.UnitTests.MinimalApis;

public class StrongOfValidationFilterTests
{
    private sealed class ValidEmail(string value) : StrongString<ValidEmail>(value), IValidatable
    {
        public bool IsValidFormat() => Value.Contains('@');
    }

    private sealed class InvalidEmail(string value) : StrongString<InvalidEmail>(value), IValidatable
    {
        public bool IsValidFormat() => false;
    }

    [Fact]
    public async Task InvokeAsync_WithValidArgument_CallsNext()
    {
        // Arrange
        StrongOfValidationFilter filter = new();
        ValidEmail validEmail = new("user@example.com");

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?> { validEmail });

        object? expectedResult = Results.Ok();
        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();
        next.Invoke(context).Returns(new ValueTask<object?>(expectedResult));

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.Same(expectedResult, result);
        await next.Received(1).Invoke(context);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidArgument_ReturnsBadRequest()
    {
        // Arrange
        StrongOfValidationFilter filter = new();
        InvalidEmail invalidEmail = new("not-an-email");

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?> { invalidEmail });

        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.IsAssignableFrom<IResult>(result);
        await next.DidNotReceive().Invoke(Arg.Any<EndpointFilterInvocationContext>());
    }

    [Fact]
    public async Task InvokeAsync_WithNonValidatableArgument_CallsNext()
    {
        // Arrange
        StrongOfValidationFilter filter = new();
        string nonValidatable = "just a string";

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?> { nonValidatable });

        object? expectedResult = Results.Ok();
        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();
        next.Invoke(context).Returns(new ValueTask<object?>(expectedResult));

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.Same(expectedResult, result);
        await next.Received(1).Invoke(context);
    }

    [Fact]
    public async Task InvokeAsync_WithNullArgument_CallsNext()
    {
        // Arrange
        StrongOfValidationFilter filter = new();

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?> { null });

        object? expectedResult = Results.Ok();
        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();
        next.Invoke(context).Returns(new ValueTask<object?>(expectedResult));

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.Same(expectedResult, result);
        await next.Received(1).Invoke(context);
    }

    [Fact]
    public async Task InvokeAsync_WithMixedArguments_ReturnsBadRequestOnFirstInvalid()
    {
        // Arrange
        StrongOfValidationFilter filter = new();
        ValidEmail validEmail = new("user@example.com");
        InvalidEmail invalidEmail = new("bad");

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?> { validEmail, invalidEmail });

        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.IsAssignableFrom<IResult>(result);
        await next.DidNotReceive().Invoke(Arg.Any<EndpointFilterInvocationContext>());
    }

    [Fact]
    public async Task InvokeAsync_WithEmptyArguments_CallsNext()
    {
        // Arrange
        StrongOfValidationFilter filter = new();

        EndpointFilterInvocationContext context = Substitute.For<EndpointFilterInvocationContext>();
        context.Arguments.Returns(new List<object?>());

        object? expectedResult = Results.Ok();
        EndpointFilterDelegate next = Substitute.For<EndpointFilterDelegate>();
        next.Invoke(context).Returns(new ValueTask<object?>(expectedResult));

        // Act
        object? result = await filter.InvokeAsync(context, next);

        // Assert
        Assert.Same(expectedResult, result);
        await next.Received(1).Invoke(context);
    }
}
