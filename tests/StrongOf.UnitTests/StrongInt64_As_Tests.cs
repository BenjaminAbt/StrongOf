using Xunit;

namespace StrongOf.Tests;

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
}
