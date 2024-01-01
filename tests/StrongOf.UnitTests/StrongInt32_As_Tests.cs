using Xunit;

namespace StrongOf.Tests;

public class StrongInt32_As_Tests
{
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value) { }

    [Fact]
    public void AsInt_ReturnsCorrectResult()
    {
        // Arrange
        TestInt32Of strong = new(77);

        // Assert
        Assert.Equal(strong.Value, strong.AsInt());
    }

    [Fact]
    public void AsInt32_ReturnsCorrectResult()
    {
        // Arrange
        TestInt32Of strong = new(92);

        // Assert
        Assert.Equal(strong.Value, strong.AsInt32());
    }
}
