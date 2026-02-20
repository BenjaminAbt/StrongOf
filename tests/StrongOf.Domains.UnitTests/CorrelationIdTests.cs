// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Identity.UnitTests;

public class CorrelationIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_SetsValue()
    {
        Guid guid = Guid.NewGuid();
        var id = new CorrelationId(guid);
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void New_ReturnsNonEmptyCorrelationId()
    {
        var id = CorrelationId.New();
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        var id = new CorrelationId(Guid.NewGuid());
        Assert.True(id.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        var id = CorrelationId.Empty();
        Assert.False(id.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        var id1 = new CorrelationId(guid);
        var id2 = new CorrelationId(guid);
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        var id = new CorrelationId(guid);
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        var converter = new StrongGuidTypeConverter<CorrelationId>();
        Assert.True(converter.CanConvertFrom(typeof(Guid)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new StrongGuidTypeConverter<CorrelationId>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsCorrelationId()
    {
        Guid guid = Guid.NewGuid();
        var converter = new StrongGuidTypeConverter<CorrelationId>();
        var result = converter.ConvertFrom(guid) as CorrelationId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsCorrelationId()
    {
        Guid guid = Guid.NewGuid();
        var converter = new StrongGuidTypeConverter<CorrelationId>();
        var result = converter.ConvertFrom(guid.ToString()) as CorrelationId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }
}
