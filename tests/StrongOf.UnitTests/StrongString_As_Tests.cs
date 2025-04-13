using Xunit;

namespace StrongOf.UnitTests;

public class StrongString_As_Tests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }

    [Fact]
    public void AsString_ReturnsCorrectResult()
    {
        // Arrange
        TestStringOf strong = new("We need Type Abbreviations in C#");

        // Assert
        Assert.Equal(strong.Value, strong.AsString());
    }

    [Fact]
    public void FromNullable_WithValue_ReturnsNonNull()
    {
        // Arrange
        string value = "A";

        // Act
        TestStringOf result = TestStringOf.FromNullable(value);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void FromNullable_WithNull_ReturnsNull()
    {
        // Arrange
        string? value = null;

        // Act
        TestStringOf? result = TestStringOf.FromNullable(value);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void FromNullable_WithNotNull_ReturnsCorrectValue()
    {
        // Arrange
        string value = "A";

        // Act
        TestStringOf result = TestStringOf.FromNullable(value);

        // Assert
        Assert.Equal(value, result.Value);
    }
}
