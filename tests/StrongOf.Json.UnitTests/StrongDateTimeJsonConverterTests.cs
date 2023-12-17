using System.Globalization;
using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongDateTimeJsonConverterTests
{
    private sealed class TestDateTimeOffsetOf(DateTime value) : StrongDateTime<TestDateTimeOffsetOf>(value) { }

    private readonly StrongDateTimeJsonConverter<TestDateTimeOffsetOf> _converter = new ();
    private readonly JsonSerializerOptions _options = new ();

    [Fact]
    public void Read_ValidJson_ReturnsStrongDateTime()
    {
        // Arrange
        string json = "{\"Id\": \"2023-12-17T14:24:22.6412808+00:00\"}";

        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        // Positioning
        while (reader.Read()) { if (reader.TokenType == JsonTokenType.String) { break; } }

        // Act
        TestDateTimeOffsetOf? result = _converter.Read(ref reader, typeof(TestDateTimeOffsetOf), _options);
        DateTime expected = DateTime.ParseExact("2023-12-17T14:24:22.6412808+00:00", "o", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AdjustToUniversal);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Write_ValidStrongDateTime_WritesJson()
    {
        // Arrange
        TestDateTimeOffsetOf strong = TestDateTimeOffsetOf.FromIso8601("2023-12-17T14:24:22.6412808+00:00");
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());

        // Assert
        Assert.Equal("2023-12-17T14:24:22.6412808Z", strong.ToStringIso8601());
    }
}
