// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json.Serialization;
using StrongOf.Json;
using StrongOf.SourceGeneration;
using StrongOf.Domains.Networking;

namespace StrongOf.NativeAot;

[StrongBoolean]
[JsonConverter(typeof(StrongBooleanJsonConverter<Active>))]
public sealed partial class Active;

[StrongChar]
[JsonConverter(typeof(StrongCharJsonConverter<Grade>))]
public sealed partial class Grade;

[StrongDateTime]
[JsonConverter(typeof(StrongDateTimeJsonConverter<CreatedAt>))]
public sealed partial class CreatedAt;

[StrongDateTimeOffset]
[JsonConverter(typeof(StrongDateTimeOffsetJsonConverter<OccurredAt>))]
public sealed partial class OccurredAt;

[StrongDecimal]
[JsonConverter(typeof(StrongDecimalJsonConverter<Price>))]
public sealed partial class Price;

[StrongDouble]
[JsonConverter(typeof(StrongDoubleJsonConverter<Ratio>))]
public sealed partial class Ratio;

[StrongGuid]
[JsonConverter(typeof(StrongGuidJsonConverter<UserId>))]
public sealed partial class UserId;

[StrongInt32]
[JsonConverter(typeof(StrongInt32JsonConverter<Count>))]
public sealed partial class Count;

[StrongInt64]
[JsonConverter(typeof(StrongInt64JsonConverter<Sequence>))]
public sealed partial class Sequence;

[StrongString]
[JsonConverter(typeof(StrongStringJsonConverter<Name>))]
public sealed partial class Name;

[StrongTimeSpan]
[JsonConverter(typeof(StrongTimeSpanJsonConverter<Elapsed>))]
public sealed partial class Elapsed;

public sealed class Payload
{
    public Active Active { get; set; } = null!;
    public Grade Grade { get; set; } = null!;
    public CreatedAt CreatedAt { get; set; } = null!;
    public OccurredAt OccurredAt { get; set; } = null!;
    public Price Price { get; set; } = null!;
    public Ratio Ratio { get; set; } = null!;
    public UserId UserId { get; set; } = null!;
    public Count Count { get; set; } = null!;
    public Sequence Sequence { get; set; } = null!;
    public Name Name { get; set; } = null!;
    public Elapsed Elapsed { get; set; } = null!;

    // Property-level converters work for types already compiled in another assembly.
    [JsonConverter(typeof(StrongStringJsonConverter<EmailAddress>))]
    public EmailAddress Email { get; set; } = null!;
}

[JsonSerializable(typeof(Payload))]
internal partial class SampleJsonContext : JsonSerializerContext;
