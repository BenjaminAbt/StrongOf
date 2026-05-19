// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongDateTimeOffset{TStrong}"/> values.
/// Uses ISO 8601 string representation to preserve UTC offset information.
/// </summary>
/// <typeparam name="TStrong">Concrete strong date-time-offset type.</typeparam>
public class StrongDateTimeOffsetJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
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
        // Parse through the StrongOf ISO helper so offset semantics remain consistent
        // with the domain type's own parsing behavior.
        if (string.IsNullOrEmpty(value) is false && StrongDateTimeOffset<TStrong>.TryParseIso8601(value, out TStrong? strong))
        {
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> as an ISO 8601 JSON string.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        => writer.WriteStringValue(strong.ToStringIso8601());
}
