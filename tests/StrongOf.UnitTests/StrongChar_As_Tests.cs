using Xunit;

namespace StrongOf.UnitTests;

public class StrongChar_As_Tests
{
    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value) { }

    [Fact]
    public void AsChar_ReturnsCorrectResult()
    {
        // Arrange
        TestCharOf strong = new('a');

        // Assert
        Assert.Equal(strong.Value, strong.AsChar());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        // Arrange
        const char value = 'A';

        // Act
        TestCharOf result = TestCharOf.FromNullable(value);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        // Arrange
        char? value = null;

        // Act
        TestCharOf? result = TestCharOf.FromNullable(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromNullable_WithNotNull_ReturnsCorrectValue()
    {
        // Arrange
        const char value = 'A';

        // Act
        TestCharOf result = TestCharOf.FromNullable(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void CompareTo_WithSameType_ReturnsCorrectComparison()
    {
        // Arrange
        TestCharOf instance1 = new('A');
        TestCharOf instance2 = new('A');

        // Act
        int result = instance1.CompareTo(instance2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithDifferentType_ThrowsArgumentException()
    {
        // Arrange
        TestCharOf instance = new('A');
        object other = new ();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => instance.CompareTo(other));
    }

    [Fact]
    public void GetHashCode_ReturnsConsistentHashCodeForEqualObjects()
    {
        // Arrange
        TestCharOf instance1 = new('A');
        TestCharOf instance2 = new('A');

        // Act
        int hashCode1 = instance1.GetHashCode();
        int hashCode2 = instance2.GetHashCode();

        // Assert
        Assert.Equal(hashCode1, hashCode2);
    }
}
