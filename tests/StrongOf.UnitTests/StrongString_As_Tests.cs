using Xunit;

namespace StrongOf.Tests;

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
}
