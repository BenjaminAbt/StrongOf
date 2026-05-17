// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongString{TStrong}"/> values.
/// Serializes and deserializes using JSON string tokens.
/// </summary>
/// <typeparam name="TStrong">Concrete strong string type.</typeparam>
public class StrongStringJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
{
    /// <summary>
    /// Reads JSON and converts it to <typeparamref name="TStrong"/>.
    /// </summary>
    /// <param name="reader">The Utf8JsonReader to read from.</param>
    /// <param name="typeToConvert">The type of object to convert.</param>
    /// <param name="options">Options to control the serializer behavior during reading.</param>
    /// <returns>
    /// Parsed <typeparamref name="TStrong"/> instance, or <see langword="null"/>
    /// when the token is empty.
    /// </returns>
    public override TStrong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value) is false)
        {
            // Keep null/empty handling consistent with converter behavior across StrongOf.Json.
            return StrongString<TStrong>.From(value);
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> as a JSON string token.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteStringValue(strong.Value);
}
