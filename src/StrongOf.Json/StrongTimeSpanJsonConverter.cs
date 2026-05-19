// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongTimeSpan{TStrong}"/> values.
/// Reads and writes <see cref="TimeSpan"/> values as string tokens.
/// </summary>
/// <typeparam name="TStrong">Concrete strong time-span type.</typeparam>
public class StrongTimeSpanJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
{
    /// <summary>
    /// Reads JSON and converts it to <typeparamref name="TStrong"/>.
    /// </summary>
    /// <param name="reader">The Utf8JsonReader to read from.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">Options to control the serializer behavior during reading.</param>
    /// <returns>
    /// Parsed <typeparamref name="TStrong"/> instance, or <see langword="null"/>
    /// when the token is empty or cannot be parsed.
    /// </returns>
    public override TStrong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        // TryParse intentionally accepts both canonical "c" payloads and common legacy
        // formats so existing integrations keep working.
        if (string.IsNullOrEmpty(value) is false && TimeSpan.TryParse(value, out TimeSpan ts))
        {
            return StrongOf<TimeSpan, TStrong>.From(ts);
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> using the round-trippable "c" format.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteStringValue(strong.Value.ToString("c"));
}
