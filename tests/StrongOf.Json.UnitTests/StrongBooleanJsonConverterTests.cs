// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongBooleanJsonConverterTests
{
    private sealed class TestBooleanOf(bool value) : StrongBoolean<TestBooleanOf>(value), IStrongOf<bool, TestBooleanOf>
    {
        public static TestBooleanOf Create(bool value) => new(value);
    }

    private readonly StrongBooleanJsonConverter<TestBooleanOf> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJsonBoolean_ReturnsStrongBoolean()
    {
        // Arrange
        const string json = "{\"IsEnabled\": true}";
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.True)
            {
                break;
            }
        }

        // Act
        TestBooleanOf? result = _converter.Read(ref reader, typeof(TestBooleanOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Value);
    }

    [Fact]
    public void Write_ValidStrongBoolean_WritesJson()
    {
        // Arrange
        TestBooleanOf strong = new(true);
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("true", json);
    }
}
