// Copyright © Benjamin Abt 2025. All rights reserved.

namespace StrongOf.Domains.Identity.UnitTests;

public class TenantIdTests
{
    [Fact]
    public void Constructor_WithValidGuid_SetsValue()
    {
        Guid guid = Guid.NewGuid();
        var id = new TenantId(guid);
        Assert.Equal(guid, id.Value);
    }

    [Fact]
    public void New_ReturnsNonEmptyTenantId()
    {
        var id = TenantId.New();
        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        var id = new TenantId(Guid.NewGuid());
        Assert.True(id.HasValue());
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        var id = TenantId.Empty();
        Assert.False(id.HasValue());
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        var id1 = new TenantId(guid);
        var id2 = new TenantId(guid);
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void Equality_DifferentValues_ReturnsFalse()
    {
        var id1 = new TenantId(Guid.NewGuid());
        var id2 = new TenantId(Guid.NewGuid());
        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        var id = new TenantId(guid);
        Assert.Equal(guid.ToString(), id.ToString());
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        var converter = new StrongGuidTypeConverter<TenantId>();
        Assert.True(converter.CanConvertFrom(typeof(Guid)));
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        var converter = new StrongGuidTypeConverter<TenantId>();
        Assert.True(converter.CanConvertFrom(typeof(string)));
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsTenantId()
    {
        Guid guid = Guid.NewGuid();
        var converter = new StrongGuidTypeConverter<TenantId>();
        var result = converter.ConvertFrom(guid) as TenantId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsTenantId()
    {
        Guid guid = Guid.NewGuid();
        var converter = new StrongGuidTypeConverter<TenantId>();
        var result = converter.ConvertFrom(guid.ToString()) as TenantId;
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }
}
