// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public class CorrelationIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_SetsValue()
    {
        Guid guid = Guid.NewGuid();
        CorrelationId id = new CorrelationId(guid);
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void New_ReturnsNonEmptyCorrelationId()
    {
        CorrelationId id = CorrelationId.New();
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        CorrelationId id = new CorrelationId(Guid.NewGuid());
        Assert.True(id.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        CorrelationId id = CorrelationId.Empty();
        Assert.False(id.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        CorrelationId id1 = new CorrelationId(guid);
        CorrelationId id2 = new CorrelationId(guid);
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        CorrelationId id = new CorrelationId(guid);
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        StrongGuidTypeConverter<CorrelationId> converter = new StrongGuidTypeConverter<CorrelationId>();
        Assert.True(converter.CanConvertFrom(typeof(Guid)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        StrongGuidTypeConverter<CorrelationId> converter = new StrongGuidTypeConverter<CorrelationId>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsCorrelationId()
    {
        Guid guid = Guid.NewGuid();
        StrongGuidTypeConverter<CorrelationId> converter = new StrongGuidTypeConverter<CorrelationId>();
        CorrelationId? result = converter.ConvertFrom(guid) as CorrelationId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsCorrelationId()
    {
        Guid guid = Guid.NewGuid();
        StrongGuidTypeConverter<CorrelationId> converter = new StrongGuidTypeConverter<CorrelationId>();
        CorrelationId? result = converter.ConvertFrom(guid.ToString()) as CorrelationId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }
}
