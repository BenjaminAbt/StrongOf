using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongGuidJsonConverterTests
{
    private sealed class TestGuidOf(Guid value) : StrongGuid<TestGuidOf>(value) { }

    private readonly StrongGuidJsonConverter<TestGuidOf> _converter;
    private readonly JsonSerializerOptions _options;

    public StrongGuidJsonConverterTests()
    {
        _converter = new StrongGuidJsonConverter<TestGuidOf>();
        _options = new JsonSerializerOptions();
    }

    [Fact]
    public void Read_ValidJson_ReturnsStrongGuid()
    {
        // Arrange
        string json = "{\"Id\": \"d3dd268c-7d12-4e2a-89b9-5368f0b2f38a\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestGuidOf? result = _converter.Read(ref reader, typeof(TestGuidOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("d3dd268c-7d12-4e2a-89b9-5368f0b2f38a", result.Value.ToString());
    }

    [Fact]
    public void Write_ValidStrongGuid_WritesJson()
    {
        // Arrange
        TestGuidOf strongGuid = new(Guid.Parse("d3dd268c-7d12-4e2a-89b9-5368f0b2f38a"));
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strongGuid, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("\"d3dd268c-7d12-4e2a-89b9-5368f0b2f38a\"", json);
    }
}
