// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongDouble{TStrong}"/> values.
/// Serializes to JSON numbers and accepts both numeric and string tokens when reading.
/// </summary>
/// <typeparam name="TStrong">Concrete strong double type.</typeparam>
public class StrongDoubleJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongDouble<TStrong>, IStrongOf<double, TStrong>
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
        if (reader.TokenType == JsonTokenType.Number && reader.TryGetDouble(out double number))
        {
            // Fast path for canonical JSON number tokens.
            return StrongOf<double, TStrong>.From(number);
        }

        // Compatibility path for payloads that encode numbers as strings.
        string? value = reader.GetString();
        if (string.IsNullOrEmpty(value) is false && StrongDouble<TStrong>.TryParse(value, CultureInfo.InvariantCulture, out TStrong? strong))
        {
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> as a JSON number token.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteNumberValue(strong.Value);
}
