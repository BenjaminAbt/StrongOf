// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using StrongOf.Domains.Networking;

namespace StrongOf.NativeAot;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (args.Contains("--require-native", StringComparer.Ordinal))
        {
            Ensure(!RuntimeFeature.IsDynamicCodeSupported, "Run the published native executable.");
        }

        Ensure(!JsonSerializer.IsReflectionEnabledByDefault, "JSON reflection must stay disabled.");
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

        Payload value = new()
        {
            Active = Active.From(true),
            Grade = Grade.From('A'),
            CreatedAt = CreatedAt.From(new DateTime(2026, 10, 4, 12, 34, 56, DateTimeKind.Unspecified)),
            OccurredAt = OccurredAt.From(new DateTimeOffset(2026, 10, 4, 12, 34, 56, TimeSpan.FromHours(2))),
            Price = Price.From(12.34m),
            Ratio = Ratio.From(1.25),
            UserId = UserId.From(Guid.Parse("12345678-1234-1234-1234-123456789abc")),
            Count = Count.From(42),
            Sequence = Sequence.From(1234567890123L),
            Name = Name.From("Ada"),
            Elapsed = Elapsed.From(TimeSpan.FromMilliseconds(1234)),
            Email = EmailAddress.From("ada@example.com"),
        };

        string json = JsonSerializer.Serialize(value, SampleJsonContext.Default.Payload);
        Payload result = JsonSerializer.Deserialize(json, SampleJsonContext.Default.Payload)
            ?? throw new InvalidOperationException("JSON returned null.");
        Ensure(value.Active.Equals(result.Active), "Active JSON round trip failed.");
        Ensure(value.Grade.Equals(result.Grade), "Grade JSON round trip failed.");
        Ensure(value.CreatedAt.Equals(result.CreatedAt), "CreatedAt JSON round trip failed.");
        Ensure(value.OccurredAt.Equals(result.OccurredAt), "OccurredAt JSON round trip failed.");
        Ensure(value.Price.Equals(result.Price), "Price JSON round trip failed.");
        Ensure(value.Ratio.Equals(result.Ratio), "Ratio JSON round trip failed.");
        Ensure(value.UserId.Equals(result.UserId), "UserId JSON round trip failed.");
        Ensure(value.Count.Equals(result.Count), "Count JSON round trip failed.");
        Ensure(value.Sequence.Equals(result.Sequence), "Sequence JSON round trip failed.");
        Ensure(value.Name.Equals(result.Name), "Name JSON round trip failed.");
        Ensure(value.Elapsed.Equals(result.Elapsed), "Elapsed JSON round trip failed.");
        Ensure(value.OccurredAt.Value.EqualsExact(result.OccurredAt.Value), "The UTC offset was lost.");
        Ensure(result.Email.IsValidFormat(), "Domain validation failed.");
        Ensure(!EmailAddress.From("invalid").IsValidFormat(), "Invalid domain value accepted.");
        Ensure(Count.TryParse("42", CultureInfo.InvariantCulture, out Count? parsed) && parsed.Value == 42, "Parsing failed.");
        Ensure(Count.FromArray([1, 2, 3])![2].Value == 3, "Collection factory failed.");
        Ensure(new StrongInt32TypeConverter<Count>().ConvertFromInvariantString("42") is Count { Value: 42 }, "Type converter failed.");
        Ensure(OccurredAt.FromIso8601("2026-10-04T12:34:56+02:00").Value.Offset == TimeSpan.FromHours(2), "ISO offset changed.");
        Console.WriteLine("NativeAOT smoke passed: all 11 JSON converters, factories, parsing, collections and domain validation.");
    }

    private static void Ensure(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
