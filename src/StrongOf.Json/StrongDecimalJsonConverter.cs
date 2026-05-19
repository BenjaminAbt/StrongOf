// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// System.Text.Json converter for <see cref="StrongDecimal{TStrong}"/> values.
/// Uses invariant string representation for stable wire format across cultures.
/// </summary>
/// <typeparam name="TStrong">Concrete strong decimal type.</typeparam>
public class StrongDecimalJsonConverter<TStrong> : JsonConverter<TStrong>
    where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
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
        // Parse with invariant culture so decimal separators are deterministic regardless
        // of server locale.
        if (string.IsNullOrEmpty(value) is false && StrongDecimal<TStrong>.TryParse(value, CultureInfo.InvariantCulture.NumberFormat, out TStrong? strong))
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
        // Keep decimal payloads as strings to avoid culture issues and numeric round-trip
        // surprises in systems that coerce JSON numbers to floating-point.
        => writer.WriteStringValue(strong.Value.ToString(CultureInfo.InvariantCulture.NumberFormat));
}
