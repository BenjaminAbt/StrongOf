// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using StrongOf.AspNetCore.OpenApi;
using Xunit;

namespace StrongOf.AspNetCore.UnitTests.OpenApi;

public class StrongOfSchemaTransformerTests
{
    private sealed class TestId(Guid value) : StrongGuid<TestId>(value) { }
    private sealed class TestName(string value) : StrongString<TestName>(value) { }
    private sealed class TestCount(int value) : StrongInt32<TestCount>(value) { }
    private sealed class TestAmount(long value) : StrongInt64<TestAmount>(value) { }
    private sealed class TestPrice(decimal value) : StrongDecimal<TestPrice>(value) { }
    private sealed class TestFlag(bool value) : StrongBoolean<TestFlag>(value) { }
    private sealed class TestInitial(char value) : StrongChar<TestInitial>(value) { }
    private sealed class TestDate(DateTime value) : StrongDateTime<TestDate>(value) { }
    private sealed class TestTimestamp(DateTimeOffset value) : StrongDateTimeOffset<TestTimestamp>(value) { }

    [Fact]
    public async Task TransformAsync_StrongGuid_MapsToStringUuid()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestId));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("string", schema.Type);
        Assert.Equal("uuid", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongString_MapsToString()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestName));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("string", schema.Type);
        Assert.Null(schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongInt32_MapsToIntegerInt32()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestCount));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("integer", schema.Type);
        Assert.Equal("int32", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongInt64_MapsToIntegerInt64()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestAmount));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("integer", schema.Type);
        Assert.Equal("int64", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongDecimal_MapsToNumberDouble()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestPrice));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("number", schema.Type);
        Assert.Equal("double", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongBoolean_MapsToBoolean()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestFlag));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("boolean", schema.Type);
        Assert.Null(schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongChar_MapsToString()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestInitial));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("string", schema.Type);
        Assert.Null(schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongDateTime_MapsToStringDateTime()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestDate));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("string", schema.Type);
        Assert.Equal("date-time", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_StrongDateTimeOffset_MapsToStringDateTime()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new() { Properties = { ["value"] = new OpenApiSchema() } };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestTimestamp));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("string", schema.Type);
        Assert.Equal("date-time", schema.Format);
        Assert.Empty(schema.Properties);
    }

    [Fact]
    public async Task TransformAsync_PreservesExistingDescription()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new()
        {
            Description = "Custom description",
            Properties = { ["value"] = new OpenApiSchema() }
        };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestId));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("Custom description", schema.Description);
    }

    [Fact]
    public async Task TransformAsync_SetsDefaultDescription_WhenNoneProvided()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new()
        {
            Properties = { ["value"] = new OpenApiSchema() }
        };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestId));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("A strongly-typed GUID value.", schema.Description);
    }

    [Fact]
    public async Task TransformAsync_UnknownType_LeavesSchemaUnchanged()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = new()
        {
            Type = "object",
            Properties = { ["value"] = new OpenApiSchema() }
        };
        OpenApiSchemaTransformerContext context = CreateContext(typeof(string));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        Assert.Equal("object", schema.Type);
        Assert.Single(schema.Properties);
    }

    private static OpenApiSchemaTransformerContext CreateContext(Type type)
    {
        JsonSerializerOptions options = new()
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        };
        JsonTypeInfo jsonTypeInfo = options.GetTypeInfo(type);

        // OpenApiSchemaTransformerContext has an internal constructor;
        // create via reflection and set the required JsonTypeInfo property.
#pragma warning disable SYSLIB0050 // FormatterServices.GetUninitializedObject is obsolete
        OpenApiSchemaTransformerContext context = (OpenApiSchemaTransformerContext)
            System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(OpenApiSchemaTransformerContext));
#pragma warning restore SYSLIB0050

        typeof(OpenApiSchemaTransformerContext)
            .GetProperty(nameof(OpenApiSchemaTransformerContext.JsonTypeInfo))!
            .SetValue(context, jsonTypeInfo);

        return context;
    }
}
