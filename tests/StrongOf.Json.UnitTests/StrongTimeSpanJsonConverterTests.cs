// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Text;
using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongTimeSpanJsonConverterTests
{
    private sealed class TestTimeSpanOf(TimeSpan value) : StrongTimeSpan<TestTimeSpanOf>(value) { }

    private readonly StrongTimeSpanJsonConverter<TestTimeSpanOf> _converter = new();
    private readonly JsonSerializerOptions _options = new();

    [Fact]
    public void Read_ValidJson_ReturnsStrongTimeSpan()
    {
        // Arrange
        const string json = "{\"Duration\": \"02:03:04\"}";
        Utf8JsonReader reader = new(Encoding.UTF8.GetBytes(json));

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                break;
            }
        }

        // Act
        TestTimeSpanOf? result = _converter.Read(ref reader, typeof(TestTimeSpanOf), _options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new TimeSpan(2, 3, 4), result.Value);
    }

    [Fact]
    public void Write_ValidStrongTimeSpan_WritesJson()
    {
        // Arrange
        TestTimeSpanOf strong = new(new TimeSpan(2, 3, 4));
        MemoryStream stream = new();
        using Utf8JsonWriter writer = new(stream);

        // Act
        _converter.Write(writer, strong, _options);
        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());
        string? jsonValue = JsonSerializer.Deserialize<string>(json);

        // Assert
        Assert.Equal("02:03:04", jsonValue);
    }
}
