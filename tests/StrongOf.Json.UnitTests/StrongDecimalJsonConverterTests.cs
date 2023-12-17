using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongDecimalJsonConverterTests
{
    private sealed class TestDecimalOf(decimal value) : StrongDecimal<TestDecimalOf>(value) { }

    private readonly StrongDecimalJsonConverter<TestDecimalOf> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJson_ReturnsStrongDecimal()
    {
        // Arrange
        string json = "{\"Id\": \"123\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestDecimalOf? result = _converter.Read(ref reader, typeof(TestDecimalOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123m, result.Value);
    }

    [Fact]
    public void Write_ValidStrongDecimal_WritesJson()
    {
        // Arrange
        TestDecimalOf strong = new(123m);
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
