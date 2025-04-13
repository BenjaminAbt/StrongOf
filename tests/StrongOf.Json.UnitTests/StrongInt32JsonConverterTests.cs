// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongInt32JsonConverterTests
{
    private sealed class TestInt32Of(int value) : StrongInt32<TestInt32Of>(value) { }

    private readonly StrongInt32JsonConverter<TestInt32Of> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJson_ReturnsStrongInt32()
    {
        // Arrange
        string json = "{\"Id\": \"123\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestInt32Of? result = _converter.Read(ref reader, typeof(TestInt32Of), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void Write_ValidStrongInt32_WritesJson()
    {
        // Arrange
        TestInt32Of strong = new(123);
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("\"123\"", json);
    }
}
