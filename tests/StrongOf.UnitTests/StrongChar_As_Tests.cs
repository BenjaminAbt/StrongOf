using Xunit;

namespace StrongOf.Tests;

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
}
