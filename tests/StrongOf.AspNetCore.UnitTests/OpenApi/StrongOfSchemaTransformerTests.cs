// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.AspNetCore.OpenApi;
#if NET9_0
using OpenApiSchema = Microsoft.OpenApi.Models.OpenApiSchema;
#else
using Microsoft.OpenApi;
using OpenApiSchema = Microsoft.OpenApi.OpenApiSchema;
#endif
using StrongOf.AspNetCore.OpenApi;
using Xunit;

namespace StrongOf.AspNetCore.UnitTests.OpenApi;

public class StrongOfSchemaTransformerTests
{
    private sealed class TestId(Guid value) : StrongGuid<TestId>(value), IStrongOf<Guid, TestId>
    {
        public static TestId Create(Guid value) => new(value);
    }

    private sealed class TestName(string value) : StrongString<TestName>(value), IStrongOf<string, TestName>
    {
        public static TestName Create(string value) => new(value);
    }

    private sealed class TestCount(int value) : StrongInt32<TestCount>(value), IStrongOf<int, TestCount>
    {
        public static TestCount Create(int value) => new(value);
    }

    private sealed class TestAmount(long value) : StrongInt64<TestAmount>(value), IStrongOf<long, TestAmount>
    {
        public static TestAmount Create(long value) => new(value);
    }

    private sealed class TestPrice(decimal value) : StrongDecimal<TestPrice>(value), IStrongOf<decimal, TestPrice>
    {
        public static TestPrice Create(decimal value) => new(value);
    }

    private sealed class TestFlag(bool value) : StrongBoolean<TestFlag>(value), IStrongOf<bool, TestFlag>
    {
        public static TestFlag Create(bool value) => new(value);
    }

    private sealed class TestInitial(char value) : StrongChar<TestInitial>(value), IStrongOf<char, TestInitial>
    {
        public static TestInitial Create(char value) => new(value);
    }

    private sealed class TestDate(DateTime value) : StrongDateTime<TestDate>(value), IStrongOf<DateTime, TestDate>
    {
        public static TestDate Create(DateTime value) => new(value);
    }

    private sealed class TestTimestamp(DateTimeOffset value) : StrongDateTimeOffset<TestTimestamp>(value), IStrongOf<DateTimeOffset, TestTimestamp>
    {
        public static TestTimestamp Create(DateTimeOffset value) => new(value);
    }

    private sealed class TestRate(double value) : StrongDouble<TestRate>(value), IStrongOf<double, TestRate>
    {
        public static TestRate Create(double value) => new(value);
    }

    private sealed class TestDuration(TimeSpan value) : StrongTimeSpan<TestDuration>(value), IStrongOf<TimeSpan, TestDuration>
    {
        public static TestDuration Create(TimeSpan value) => new(value);
    }

    [Fact]
    public async Task TransformAsync_StrongGuid_MapsToStringUuid()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestId));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Equal("uuid", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongString_MapsToString()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestName));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Null(schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongInt32_MapsToIntegerInt32()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestCount));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "integer");
        Assert.Equal("int32", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongInt64_MapsToIntegerInt64()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestAmount));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "integer");
        Assert.Equal("int64", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongDecimal_MapsToNumberDouble()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestPrice));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "number");
        Assert.Equal("double", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongBoolean_MapsToBoolean()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestFlag));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "boolean");
        Assert.Null(schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongChar_MapsToString()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestInitial));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Null(schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongDateTime_MapsToStringDateTime()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestDate));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Equal("date-time", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongDateTimeOffset_MapsToStringDateTime()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestTimestamp));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Equal("date-time", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongDouble_MapsToNumberDouble()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestRate));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "number");
        Assert.Equal("double", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_StrongTimeSpan_MapsToStringDuration()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        OpenApiSchemaTransformerContext context = CreateContext(typeof(TestDuration));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "string");
        Assert.Equal("duration", schema.Format);
        AssertNoProperties(schema);
    }

    [Fact]
    public async Task TransformAsync_PreservesExistingDescription()
    {
        // Arrange
        StrongOfSchemaTransformer transformer = new();
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        schema.Description = "Custom description";
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
        OpenApiSchema schema = CreateSchemaWithValueProperty();
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
        OpenApiSchema schema = CreateSchemaWithValueProperty();
        SetSchemaType(schema, "object");
        OpenApiSchemaTransformerContext context = CreateContext(typeof(string));

        // Act
        await transformer.TransformAsync(schema, context, CancellationToken.None);

        // Assert
        AssertSchemaType(schema, "object");
        AssertSingleValueProperty(schema);
    }

    private static OpenApiSchema CreateSchemaWithValueProperty()
    {
        OpenApiSchema schema = new();
#if NET9_0
        schema.Properties ??= new Dictionary<string, OpenApiSchema>(StringComparer.Ordinal);
#else
        schema.Properties ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSchema>(StringComparer.Ordinal);
#endif
        schema.Properties["value"] = new OpenApiSchema();
        return schema;
    }

    private static void AssertNoProperties(OpenApiSchema schema)
        => Assert.True((schema.Properties?.Count ?? 0) == 0);

    private static void AssertSingleValueProperty(OpenApiSchema schema)
        => Assert.True((schema.Properties?.Count ?? 0) == 1);

    private static void SetSchemaType(OpenApiSchema schema, string value)
    {
#if NET9_0
        schema.Type = value;
#else
        schema.Type = value switch
        {
            "string" => JsonSchemaType.String,
            "integer" => JsonSchemaType.Integer,
            "number" => JsonSchemaType.Number,
            "boolean" => JsonSchemaType.Boolean,
            "object" => JsonSchemaType.Object,
            _ => throw new InvalidOperationException($"Unsupported schema type: {value}")
        };
#endif
    }

    private static void AssertSchemaType(OpenApiSchema schema, string expected)
    {
#if NET9_0
        Assert.Equal(expected, schema.Type);
#else
        JsonSchemaType? expectedType = expected switch
        {
            "string" => JsonSchemaType.String,
            "integer" => JsonSchemaType.Integer,
            "number" => JsonSchemaType.Number,
            "boolean" => JsonSchemaType.Boolean,
            "object" => JsonSchemaType.Object,
            _ => throw new InvalidOperationException($"Unsupported schema type: {expected}")
        };
        Assert.Equal(expectedType, schema.Type);
#endif
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
        OpenApiSchemaTransformerContext context = (OpenApiSchemaTransformerContext)
            System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(OpenApiSchemaTransformerContext));

        typeof(OpenApiSchemaTransformerContext)
            .GetProperty(nameof(OpenApiSchemaTransformerContext.JsonTypeInfo))!
            .SetValue(context, jsonTypeInfo);

        return context;
    }
}
