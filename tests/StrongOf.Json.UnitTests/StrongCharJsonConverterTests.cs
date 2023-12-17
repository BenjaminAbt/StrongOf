using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongCharJsonConverterTests
{
    private sealed class TestCharOf(char value) : StrongChar<TestCharOf>(value) { }

    private readonly StrongCharJsonConverter<TestCharOf> _converter = new ();
    private readonly JsonSerializerOptions _options = new ();

    [Fact]
    public void Read_ValidJson_ReturnsStrongChar()
    {
        // Arrange
        string json = "{\"Id\": \"a\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestCharOf? result = _converter.Read(ref reader, typeof(TestCharOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal('a', result.Value);
    }

    [Fact]
    public void Write_ValidStrongChar_WritesJson()
    {
        // Arrange
        TestCharOf strong = new('a');
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("\"a\"", json);
    }
}
