// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongInt32{TStrong}"/> values.
/// Uses invariant string representation for numeric wire compatibility.
/// </summary>
/// <typeparam name="TStrong">Concrete strong 32-bit integer type.</typeparam>
public class StrongInt32JsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongInt32<TStrong>, IStrongOf<int, TStrong>
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
        // Keep deserialization aligned with the existing string-based wire format.
        if (string.IsNullOrEmpty(value) is false && StrongInt32<TStrong>.TryParse(value, out TStrong? strong))
        {
            return strong;
        }

        return null;
    }

    /// <summary>
    /// Writes <typeparamref name="TStrong"/> as an invariant-culture JSON string.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to write to.</param>
    /// <param name="strong">The value to write.</param>
    /// <param name="options">Options to control the serializer behavior during writing.</param>
    public override void Write(Utf8JsonWriter writer, TStrong strong, JsonSerializerOptions options)
        // Invariant culture avoids locale-specific digits or separators in persisted payloads.
        => writer.WriteStringValue(strong.Value.ToString(CultureInfo.InvariantCulture));
}
