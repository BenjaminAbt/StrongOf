// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StrongOf.Json;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that resolves the correct StrongOf JSON converter
/// for supported strong types at runtime.
/// </summary>
/// <remarks>
/// <para>
/// Register this factory once in <see cref="JsonSerializerOptions.Converters"/> to enable
/// automatic serialization and deserialization for all supported StrongOf types without
/// adding per-type converters.
/// </para>
/// </remarks>
public sealed class StrongOfJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> s_converterCache = new();
    private static readonly IReadOnlyDictionary<Type, Type> s_converterMap = new Dictionary<Type, Type>
    {
        { typeof(StrongBoolean<>), typeof(StrongBooleanJsonConverter<>) },
        { typeof(StrongChar<>), typeof(StrongCharJsonConverter<>) },
        { typeof(StrongDateTime<>), typeof(StrongDateTimeJsonConverter<>) },
        { typeof(StrongDateTimeOffset<>), typeof(StrongDateTimeOffsetJsonConverter<>) },
        { typeof(StrongDecimal<>), typeof(StrongDecimalJsonConverter<>) },
        { typeof(StrongDouble<>), typeof(StrongDoubleJsonConverter<>) },
        { typeof(StrongGuid<>), typeof(StrongGuidJsonConverter<>) },
        { typeof(StrongInt32<>), typeof(StrongInt32JsonConverter<>) },
        { typeof(StrongInt64<>), typeof(StrongInt64JsonConverter<>) },
        { typeof(StrongString<>), typeof(StrongStringJsonConverter<>) },
        { typeof(StrongTimeSpan<>), typeof(StrongTimeSpanJsonConverter<>) },
    };

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        return TryGetConverterType(typeToConvert, out _);
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        return s_converterCache.GetOrAdd(typeToConvert, static strongType =>
        {
            if (!TryGetConverterType(strongType, out Type? converterType))
            {
                throw new NotSupportedException($"No StrongOf.Json converter is available for type '{strongType}'.");
            }

            Type closedConverterType = converterType.MakeGenericType(strongType);
            object? converter = Activator.CreateInstance(closedConverterType);

            return converter as JsonConverter
                   ?? throw new NotSupportedException($"Unable to create a JsonConverter for type '{strongType}'.");
        });
    }

    private static bool TryGetConverterType(Type typeToConvert, [NotNullWhen(true)] out Type? converterType)
    {
        if (typeToConvert.IsAbstract || typeToConvert.IsGenericTypeDefinition)
        {
            converterType = null;
            return false;
        }

        Type? currentType = typeToConvert;

        while (currentType is not null && currentType != typeof(object))
        {
            if (currentType.IsGenericType && s_converterMap.TryGetValue(currentType.GetGenericTypeDefinition(), out Type? resolvedConverterType))
            {
                converterType = resolvedConverterType;
                return true;
            }

            currentType = currentType.BaseType;
        }

        converterType = null;
        return false;
    }
}

/// <summary>
/// Extension methods for registering StrongOf JSON converters.
/// </summary>
public static class StrongOfJsonSerializerOptionsExtensions
{
    /// <summary>
    /// Registers the <see cref="StrongOfJsonConverterFactory"/> once so that all supported
    /// StrongOf types can be serialized and deserialized automatically.
    /// </summary>
    /// <param name="options">The serializer options to configure.</param>
    /// <returns>The same <see cref="JsonSerializerOptions"/> instance for chaining.</returns>
    /// <example>
    /// <code>
    /// JsonSerializerOptions options = new JsonSerializerOptions()
    ///     .AddStrongOfConverters();
    /// </code>
    /// </example>
    public static JsonSerializerOptions AddStrongOfConverters(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        foreach (JsonConverter converter in options.Converters)
        {
            if (converter is StrongOfJsonConverterFactory)
            {
                return options;
            }
        }

        options.Converters.Add(new StrongOfJsonConverterFactory());
        return options;
    }
}
