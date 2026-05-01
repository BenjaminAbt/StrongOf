// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.Json;
using Xunit;

namespace StrongOf.Json.UnitTests;

public class StrongOfJsonConverterFactoryTests
{
    private sealed class TestBoolean(bool value) : StrongBoolean<TestBoolean>(value), IStrongOf<bool, TestBoolean>
    {
        public static TestBoolean Create(bool value) => new(value);
    }

    private sealed class TestChar(char value) : StrongChar<TestChar>(value), IStrongOf<char, TestChar>
    {
        public static TestChar Create(char value) => new(value);
    }

    private sealed class TestDateTime(DateTime value) : StrongDateTime<TestDateTime>(value), IStrongOf<DateTime, TestDateTime>
    {
        public static TestDateTime Create(DateTime value) => new(value);
    }

    private sealed class TestDateTimeOffset(DateTimeOffset value) : StrongDateTimeOffset<TestDateTimeOffset>(value), IStrongOf<DateTimeOffset, TestDateTimeOffset>
    {
        public static TestDateTimeOffset Create(DateTimeOffset value) => new(value);
    }

    private sealed class TestDecimal(decimal value) : StrongDecimal<TestDecimal>(value), IStrongOf<decimal, TestDecimal>
    {
        public static TestDecimal Create(decimal value) => new(value);
    }

    private sealed class TestDouble(double value) : StrongDouble<TestDouble>(value), IStrongOf<double, TestDouble>
    {
        public static TestDouble Create(double value) => new(value);
    }

    private sealed class TestGuid(Guid value) : StrongGuid<TestGuid>(value), IStrongOf<Guid, TestGuid>
    {
        public static TestGuid Create(Guid value) => new(value);
    }

    private sealed class TestInt32(int value) : StrongInt32<TestInt32>(value), IStrongOf<int, TestInt32>
    {
        public static TestInt32 Create(int value) => new(value);
    }

    private sealed class TestInt64(long value) : StrongInt64<TestInt64>(value), IStrongOf<long, TestInt64>
    {
        public static TestInt64 Create(long value) => new(value);
    }

    private sealed class TestString(string value) : StrongString<TestString>(value), IStrongOf<string, TestString>
    {
        public static TestString Create(string value) => new(value);
    }

    private sealed class TestTimeSpan(TimeSpan value) : StrongTimeSpan<TestTimeSpan>(value), IStrongOf<TimeSpan, TestTimeSpan>
    {
        public static TestTimeSpan Create(TimeSpan value) => new(value);
    }

    private sealed class TestPayload
    {
        public required TestBoolean Boolean { get; init; }
        public required TestChar Char { get; init; }
        public required TestDateTime DateTime { get; init; }
        public required TestDateTimeOffset DateTimeOffset { get; init; }
        public required TestDecimal Decimal { get; init; }
        public required TestDouble Double { get; init; }
        public required TestGuid Guid { get; init; }
        public required TestInt32 Int32 { get; init; }
        public required TestInt64 Int64 { get; init; }
        public required TestString String { get; init; }
        public required TestTimeSpan TimeSpan { get; init; }
    }

    [Fact]
    public void AddStrongOfConverters_WhenCalledMultipleTimes_AddsFactoryOnlyOnce()
    {
        // Arrange
        JsonSerializerOptions options = new();

        // Act
        JsonSerializerOptions result = options
            .AddStrongOfConverters()
            .AddStrongOfConverters();

        // Assert
        Assert.Same(options, result);
        Assert.Single(options.Converters);
        Assert.IsType<StrongOfJsonConverterFactory>(options.Converters[0]);
    }

    [Fact]
    public void CanConvert_WithSupportedStrongType_ReturnsTrue()
    {
        // Arrange
        StrongOfJsonConverterFactory factory = new();

        // Act
        bool result = factory.CanConvert(typeof(TestBoolean));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanConvert_WithUnsupportedType_ReturnsFalse()
    {
        // Arrange
        StrongOfJsonConverterFactory factory = new();

        // Act
        bool result = factory.CanConvert(typeof(string));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SerializeAndDeserialize_WithRegisteredFactory_RoundTripsAllSupportedStrongTypes()
    {
        // Arrange
        JsonSerializerOptions options = new JsonSerializerOptions().AddStrongOfConverters();
        TestPayload payload = new()
        {
            Boolean = new TestBoolean(true),
            Char = new TestChar('X'),
            DateTime = TestDateTime.FromIso8601("2026-03-07T10:11:12.0000000Z"),
            DateTimeOffset = TestDateTimeOffset.FromIso8601("2026-03-07T10:11:12.0000000+00:00"),
            Decimal = new TestDecimal(123.45m),
            Double = new TestDouble(98.5d),
            Guid = new TestGuid(Guid.Parse("92fd0c0d-f6f8-4ae0-b4cb-c6e9db04c651")),
            Int32 = new TestInt32(42),
            Int64 = new TestInt64(42000000000L),
            String = new TestString("hello-world"),
            TimeSpan = new TestTimeSpan(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(3) + TimeSpan.FromSeconds(4)),
        };

        // Act
        string json = JsonSerializer.Serialize(payload, options);
        TestPayload? result = JsonSerializer.Deserialize<TestPayload>(json, options);

        // Assert
        Assert.Contains("\"Boolean\":true", json, StringComparison.Ordinal);
        Assert.Contains("\"Double\":98.5", json, StringComparison.Ordinal);
        Assert.Contains("\"TimeSpan\":\"02:03:04\"", json, StringComparison.Ordinal);

        Assert.NotNull(result);
        Assert.Equal(payload.Boolean.Value, result.Boolean.Value);
        Assert.Equal(payload.Char.Value, result.Char.Value);
        Assert.Equal(payload.DateTime.Value.ToUniversalTime(), result.DateTime.Value.ToUniversalTime());
        Assert.Equal(payload.DateTimeOffset.Value, result.DateTimeOffset.Value);
        Assert.Equal(payload.Decimal.Value, result.Decimal.Value);
        Assert.Equal(payload.Double.Value, result.Double.Value);
        Assert.Equal(payload.Guid.Value, result.Guid.Value);
        Assert.Equal(payload.Int32.Value, result.Int32.Value);
        Assert.Equal(payload.Int64.Value, result.Int64.Value);
        Assert.Equal(payload.String.Value, result.String.Value);
        Assert.Equal(payload.TimeSpan.Value, result.TimeSpan.Value);
    }
}
