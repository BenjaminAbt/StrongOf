// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.Domains.UnitTests;

/// <summary>
/// Tests for <see cref="EntityId"/>.
/// </summary>
public class EntityIdTests
{
    [Fact]
    public void Constructor_WithValidValue_SetsValue()
    {
        // Arrange
        Guid value = Guid.NewGuid();

        // Act
        EntityId entityId = new EntityId(value);

        // Assert
        Assert.Equal(value, entityId.Value);
    }

    [Fact]
    public void New_CreatesNewGuid()
    {
        // Act
        EntityId entityId = EntityId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, entityId.Value);
    }

    [Fact]
    public void Empty_ReturnsEmptyGuid()
    {
        // Act
        EntityId entityId = EntityId.Empty;

        // Assert
        Assert.Equal(Guid.Empty, entityId.Value);
    }

    [Fact]
    public void IsEmpty_WithEmptyGuid_ReturnsTrue()
    {
        // Arrange
        EntityId entityId = new EntityId(Guid.Empty);

        // Act
        bool result = entityId.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyGuid_ReturnsFalse()
    {
        // Arrange
        EntityId entityId = new EntityId(Guid.NewGuid());

        // Act
        bool result = entityId.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        // Arrange
        EntityId entityId = new EntityId(Guid.NewGuid());

        // Act
        bool result = entityId.HasValue();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        // Arrange
        EntityId entityId = new EntityId(Guid.Empty);

        // Act
        bool result = entityId.HasValue();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToShortString_ReturnsFirst8Characters()
    {
        // Arrange
        Guid guid = Guid.Parse("12345678-1234-1234-1234-123456789012");
        EntityId entityId = new EntityId(guid);

        // Act
        string result = entityId.ToShortString();

        // Assert
        Assert.Equal(8, result.Length);
        Assert.Equal("12345678", result);
    }

    [Fact]
    public void Equality_SameValue_ReturnsTrue()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        EntityId id1 = new EntityId(guid);
        EntityId id2 = new EntityId(guid);

        // Act & Assert
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        EntityId id1 = new EntityId(Guid.NewGuid());
        EntityId id2 = new EntityId(Guid.NewGuid());

        // Act & Assert
        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        EntityId id1 = new EntityId(guid);
        EntityId id2 = new EntityId(guid);

        // Act & Assert
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        EntityId entityId = EntityId.From(guid);

        // Assert
        Assert.Equal(guid, entityId.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        // Arrange
        StrongGuidTypeConverter<EntityId> converter = new StrongGuidTypeConverter<EntityId>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(Guid));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        StrongGuidTypeConverter<EntityId> converter = new StrongGuidTypeConverter<EntityId>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsEntityId()
    {
        // Arrange
        StrongGuidTypeConverter<EntityId> converter = new StrongGuidTypeConverter<EntityId>();
        Guid guid = Guid.NewGuid();

        // Act
        EntityId? result = converter.ConvertFrom(guid) as EntityId;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsEntityId()
    {
        // Arrange
        StrongGuidTypeConverter<EntityId> converter = new StrongGuidTypeConverter<EntityId>();
        Guid guid = Guid.NewGuid();
        string guidString = guid.ToString();

        // Act
        EntityId? result = converter.ConvertFrom(guidString) as EntityId;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        StrongGuidTypeConverter<EntityId> converter = new StrongGuidTypeConverter<EntityId>();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
