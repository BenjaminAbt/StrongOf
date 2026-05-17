// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongBoolean{TStrong}"/> values.
/// Serializes to JSON booleans and accepts both boolean and string tokens when reading.
/// </summary>
/// <typeparam name="TStrong">Concrete strong boolean type.</typeparam>
public class StrongBooleanJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongBoolean<TStrong>, IStrongOf<bool, TStrong>
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
        if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
        {
            // Fast path for canonical JSON boolean tokens.
            return StrongOf<bool, TStrong>.From(reader.GetBoolean());
        }

        // Compatibility path for payloads that carry boolean values as strings.
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value) is false && StrongBoolean<TStrong>.TryParse(value, null, out TStrong? strong))
        {
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> as a JSON boolean token.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteBooleanValue(strong.Value);
}
