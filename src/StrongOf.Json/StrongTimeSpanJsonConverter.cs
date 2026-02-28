// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// A JSON converter for <see cref="StrongTimeSpan{TStrong}"/> types.
/// Reads and writes TimeSpan values as ISO 8601 duration strings.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongTimeSpan.</typeparam>
public class StrongTimeSpanJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongTimeSpan<TStrong>
{
    /// <summary>
    /// Reads and converts the JSON to type TStrong.
    /// </summary>
    /// <param name="reader">The Utf8JsonReader to read from.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">Options to control the serializer behavior during reading.</param>
    /// <returns>A value of type TStrong.</returns>
    public override TStrong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value) is false && TimeSpan.TryParse(value, out TimeSpan ts))
        {
            return StrongOf<TimeSpan, TStrong>.From(ts);
        }

        return null;
    }

    /// <summary>
    /// Writes a TStrong value to a Utf8JsonWriter.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteStringValue(strong.Value.ToString("c"));
}
