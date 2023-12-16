using Xunit;

namespace StrongOf.UnitTests;

public class StrongInt64Tests
{
    private sealed class TestInt64Of(long value) : StrongInt64<TestInt64Of>(value) { }
    private sealed class OtherTestInt64Of(long value) : StrongInt64<OtherTestInt64Of>(value) { }

    [Fact]
    public void NewFrom_ShouldBeTheSame()
    {
        TestInt64Of strongInt1 = new(1);
        TestInt64Of strongInt2 = TestInt64Of.From(1);
        Assert.Equal(strongInt1.Value, strongInt2.Value);
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestInt64Of testOf = new(1);
        OtherTestInt64Of otherTestOf2 = new(1);

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestInt64Of strongInt1 = new(1);
        TestInt64Of strongInt2 = new(2);
        Assert.True(strongInt1.CompareTo(strongInt2) < 0);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidInt()
    {
        bool isValid = TestInt64Of.TryParse("123", out TestInt64Of? strongInt);
        Assert.True(isValid);
        Assert.NotNull(strongInt);
        Assert.Equal(123, strongInt.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidInt()
    {
        bool isValid = TestInt64Of.TryParse("X", out TestInt64Of? strongInt);
        Assert.False(isValid);
        Assert.Null(strongInt);
    }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestInt64Of strongInt = new(123);
        Assert.True(strongInt == 123);
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestInt64Of strongInt = new(123);
        Assert.True(strongInt != 456);
    }

    [Fact]
    public void Equals_ShouldReturnTrueForEqualValues()
    {
        TestInt64Of strongInt1 = new(123);
        TestInt64Of strongInt2 = new(123);
        Assert.True(strongInt1.Equals(strongInt2));
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameHashCodeForEqualValues()
    {
        TestInt64Of strongInt1 = new(123);
        TestInt64Of strongInt2 = new(123);
        Assert.Equal(strongInt1.GetHashCode(), strongInt2.GetHashCode());
    }
}