using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt32Tests
{
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value) { }
    private sealed class OtherTestInt32Of(int Value) : StrongInt32<OtherTestInt32Of>(Value) { }

    [Fact]
    public void NewFrom_ShouldBeTheSame()
    {
        TestInt32Of strongGuid1 = new(1);
        TestInt32Of strongGuid2 = TestInt32Of.From(1);
        Assert.Equal(strongGuid1.Value, strongGuid2.Value);
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestInt32Of testOf = new(1);
        OtherTestInt32Of otherTestOf2 = new(1);

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestInt32Of testOf = new(1);
        Assert.True(testOf.Equals(1));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestInt32Of testOf = new(1);
        Assert.False(testOf.Equals(2));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestInt32Of strongInt1 = new(1);
        TestInt32Of strongInt2 = new(2);
        Assert.True(strongInt1.CompareTo(strongInt2) < 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidInt()
    {
        bool isValid = TestInt32Of.TryParse("123", out TestInt32Of? strongInt);
        Assert.True(isValid);
        Assert.NotNull(strongInt);
        Assert.Equal(123, strongInt.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidInt()
    {
        bool isValid = TestInt32Of.TryParse("X", out TestInt32Of? strongInt);
        Assert.False(isValid);
        Assert.Null(strongInt);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualValues()
    {
        TestInt32Of strongInt1 = new(123);
        TestInt32Of strongInt2 = new(123);
        Assert.True(strongInt1.Equals(strongInt2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualValues()
    {
        TestInt32Of strongInt1 = new(123);
        TestInt32Of strongInt2 = new(123);
        Assert.Equal(strongInt1.GetHashCode(), strongInt2.GetHashCode());
    }
}
