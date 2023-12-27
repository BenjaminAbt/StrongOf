using Xunit;

namespace StrongOf.UnitTests;

public class StrongOfTests_Operators
{
    private sealed class TestOf(int Value) : StrongOf<int, TestOf>(Value) { }

    [Fact]
    public void OperatorEquals_WithNullObjects_ReturnsTrue()
    {
        TestOf obj1 = null!;
        TestOf obj2 = null!;
        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void OperatorEquals_WithNullObject_ReturnsFalse()
    {
        TestOf obj1 = new(123);
        TestOf obj2 = null!;
        Assert.False(obj1 == obj2);
    }

    [Fact]
    public void OperatorEquals_WithNonNullObjects_ReturnsTrue()
    {
        TestOf obj1 = new(123);
        TestOf obj2 = new(123);
        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void OperatorNotEquals_WithDifferentValues_ReturnsTrue()
    {
        TestOf obj1 = new(123);
        TestOf obj2 = new(456);
        Assert.True(obj1 != obj2);
    }
}
