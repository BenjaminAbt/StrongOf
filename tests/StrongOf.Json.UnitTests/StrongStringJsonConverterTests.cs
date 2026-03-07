// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongStringJsonConverterTests
{
    private sealed class TestStringOf(string value) : StrongString<TestStringOf>(value) { }

    private readonly StrongStringJsonConverter<TestStringOf> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJson_ReturnsStrongString()
    {
        // Arrange
        const string json = "{\"Id\": \"hello-world\"}";
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                break;
            }
        }

        // Act
        TestStringOf? result = _converter.Read(ref reader, typeof(TestStringOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("hello-world", result.Value);
    }

    [Fact]
    public void Write_ValidStrongString_WritesJson()
    {
        // Arrange
        TestStringOf strong = new("hello-world");
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());
        string? jsonValue = JsonSerializer.Deserialize<string>(json);

        // Assert
        Assert.Equal("hello-world", jsonValue);
    }
}
