using Xunit;

namespace StrongOf.UnitTests;

public class StrongOfTests_EqualsTests
{
    private sealed class TestOf(int Value) : StrongOf<int, TestOf>(Value) { }

    [Fact]
    public void GetHashCode_ReturnsExpectedHashCode()
    {
        int target = 31;
        TestOf testOf = new(target);

        int expectedHashCode = target.GetHashCode();
        int testOfHashCode = testOf.GetHashCode();

        Assert.Equal(expectedHashCode, testOfHashCode);
    }

    [Fact]
    public void ToString_ReturnsExpectedString()
    {
        int target = 31;
        TestOf testOf = new(target);

        string expectedString = target.ToString();
        string testOfString = testOf.ToString();

        Assert.Equal(expectedString, testOfString);
    }
}
