// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

public class TenantIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_SetsValue()
    {
        Guid guid = Guid.NewGuid();
        TenantId id = new TenantId(guid);
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void New_ReturnsNonEmptyTenantId()
    {
        TenantId id = TenantId.New();
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        TenantId id = new TenantId(Guid.NewGuid());
        Assert.True(id.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        TenantId id = TenantId.Empty();
        Assert.False(id.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        TenantId id1 = new TenantId(guid);
        TenantId id2 = new TenantId(guid);
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        TenantId id1 = new TenantId(Guid.NewGuid());
        TenantId id2 = new TenantId(Guid.NewGuid());
        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        TenantId id = new TenantId(guid);
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        StrongGuidTypeConverter<TenantId> converter = new StrongGuidTypeConverter<TenantId>();
        Assert.True(converter.CanConvertFrom(typeof(Guid)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        StrongGuidTypeConverter<TenantId> converter = new StrongGuidTypeConverter<TenantId>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsTenantId()
    {
        Guid guid = Guid.NewGuid();
        StrongGuidTypeConverter<TenantId> converter = new StrongGuidTypeConverter<TenantId>();
        TenantId? result = converter.ConvertFrom(guid) as TenantId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsTenantId()
    {
        Guid guid = Guid.NewGuid();
        StrongGuidTypeConverter<TenantId> converter = new StrongGuidTypeConverter<TenantId>();
        TenantId? result = converter.ConvertFrom(guid.ToString()) as TenantId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }
}
