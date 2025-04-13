using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt64_As_Tests
{
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value) { }

    [Fact]
    public void AsLong_ReturnsCorrectResult()
    {
        // Arrange
        TestInt64Of strong = new(77);

        // Assert
        Assert.Equal(strong.Value, strong.AsLong());
    }

    [Fact]
    public void AsInt64_ReturnsCorrectResult()
    {
        // Arrange
        TestInt64Of strong = new(92);

        // Assert
        Assert.Equal(strong.Value, strong.AsInt64());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        // Arrange
        long value = 7;

        // Act
        TestInt64Of result = TestInt64Of.FromNullable(value);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        // Arrange
        long? value = null;

        // Act
        TestInt64Of? result = TestInt64Of.FromNullable(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromNullable_WithNotNull_ReturnsCorrectValue()
    {
        // Arrange
        long value = 7;

        // Act
        TestInt64Of result = TestInt64Of.FromNullable(value);

        // Assert
        Assert.Equal(value, result.Value);
    }
}
