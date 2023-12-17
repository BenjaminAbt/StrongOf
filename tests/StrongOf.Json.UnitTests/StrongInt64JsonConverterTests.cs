using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongInt64JsonConverterTests
{
    private sealed class TestInt64Of(long value) : StrongInt64<TestInt64Of>(value) { }

    private readonly StrongInt64JsonConverter<TestInt64Of> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJson_ReturnsStrongInt64()
    {
        // Arrange
        string json = "{\"Id\": \"123\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestInt64Of? result = _converter.Read(ref reader, typeof(TestInt64Of), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void Write_ValidStrongInt64_WritesJson()
    {
        // Arrange
        TestInt64Of strong = new(123);
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
