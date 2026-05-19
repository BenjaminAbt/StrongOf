// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongDoubleJsonConverterTests
{
    private sealed class TestDoubleOf(double value) : StrongDouble<TestDoubleOf>(value), IStrongOf<double, TestDoubleOf>
    {
        public static TestDoubleOf Create(double value) => new(value);
    }

    private readonly StrongDoubleJsonConverter<TestDoubleOf> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJsonNumber_ReturnsStrongDouble()
    {
        // Arrange
        const string json = "{\"Distance\": 123.45}";
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                break;
            }
        }

        // Act
        TestDoubleOf? result = _converter.Read(ref reader, typeof(TestDoubleOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123.45d, result.Value);
    }

    [Fact]
    public void Write_ValidStrongDouble_WritesJson()
    {
        // Arrange
        TestDoubleOf strong = new(123.45d);
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("123.45", json);
    }
}
