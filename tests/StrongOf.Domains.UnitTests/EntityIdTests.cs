// Copyright © Benjamin Abt 2025. All rights reserved.

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
        var entityId = new EntityId(value);

        // Assert
        Assert.Equal(value, entityId.Value);
    }

    [Fact]
    public void New_CreatesNewGuid()
    {
        // Act
        var entityId = EntityId.New();

        // Assert
        Assert.NotEqual(Guid.Empty, entityId.Value);
    }

    [Fact]
    public void Empty_ReturnsEmptyGuid()
    {
        // Act
        var entityId = EntityId.Empty;

        // Assert
        Assert.Equal(Guid.Empty, entityId.Value);
    }

    [Fact]
    public void IsEmpty_WithEmptyGuid_ReturnsTrue()
    {
        // Arrange
        var entityId = new EntityId(Guid.Empty);

        // Act
        bool result = entityId.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyGuid_ReturnsFalse()
    {
        // Arrange
        var entityId = new EntityId(Guid.NewGuid());

        // Act
        bool result = entityId.IsEmpty();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasValue_WithNonEmptyGuid_ReturnsTrue()
    {
        // Arrange
        var entityId = new EntityId(Guid.NewGuid());

        // Act
        bool result = entityId.HasValue();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasValue_WithEmptyGuid_ReturnsFalse()
    {
        // Arrange
        var entityId = new EntityId(Guid.Empty);

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
        var entityId = new EntityId(guid);

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
        var id1 = new EntityId(guid);
        var id2 = new EntityId(guid);

        // Act & Assert
        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
    }

    [Fact]
    public void Equality_DifferentValue_ReturnsFalse()
    {
        // Arrange
        var id1 = new EntityId(Guid.NewGuid());
        var id2 = new EntityId(Guid.NewGuid());

        // Act & Assert
        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void GetHashCode_SameValue_ReturnsSameHashCode()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        var id1 = new EntityId(guid);
        var id2 = new EntityId(guid);

        // Act & Assert
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void From_CreatesInstance()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        var entityId = EntityId.From(guid);

        // Assert
        Assert.Equal(guid, entityId.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromGuid()
    {
        // Arrange
        var converter = new EntityIdTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(Guid));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_CanConvertFromString()
    {
        // Arrange
        var converter = new EntityIdTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(string));

        // Assert
        Assert.True(canConvert);
    }

    [Fact]
    public void TypeConverter_ConvertFromGuid_ReturnsEntityId()
    {
        // Arrange
        var converter = new EntityIdTypeConverter();
        Guid guid = Guid.NewGuid();

        // Act
        var result = converter.ConvertFrom(guid) as EntityId;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_ConvertFromString_ReturnsEntityId()
    {
        // Arrange
        var converter = new EntityIdTypeConverter();
        Guid guid = Guid.NewGuid();
        string guidString = guid.ToString();

        // Act
        var result = converter.ConvertFrom(guidString) as EntityId;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void TypeConverter_CanConvertFromInt_ReturnsFalse()
    {
        // Arrange
        var converter = new EntityIdTypeConverter();

        // Act
        bool canConvert = converter.CanConvertFrom(typeof(int));

        // Assert
        Assert.False(canConvert);
    }
}
