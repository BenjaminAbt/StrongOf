using Xunit;

namespace StrongOf.UnitTests;

public class StrongGuidTests
{
    private sealed class TestGuidOf(Guid value) : StrongGuid<TestGuidOf>(value) { }
    private sealed class OtherTestGuidOf(Guid value) : StrongGuid<OtherTestGuidOf>(value) { }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestGuidOf testOf = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        OtherTestGuidOf otherTestOf2 = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectOrder()
    {
        TestGuidOf strongGuid1 = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        TestGuidOf strongGuid2 = new(Guid.Parse("00000000-0000-0000-0000-000000000002"));
        Assert.True(strongGuid1.CompareTo(strongGuid2) < 0);
    }

    [Fact]
    public void NewFrom_ShouldBeTheSame()
    {
        TestGuidOf strongGuid1 = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        TestGuidOf strongGuid2 = TestGuidOf.From(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.Equal(strongGuid1.Value, strongGuid2.Value);
    }

    [Fact]
    public void Empty_ShouldReturnEmptyGuid()
    {
        TestGuidOf strongGuid = TestGuidOf.Empty();
        Assert.Equal(Guid.Empty, strongGuid.Value);
    }

    [Fact]
    public void IsEmpty_ShouldReturnTrueForEmptyGuid()
    {
        TestGuidOf strongGuid = TestGuidOf.Empty();
        Assert.True(strongGuid.IsEmpty());
    }

    [Fact]
    public void New_ShouldReturnNewGuid()
    {
        TestGuidOf strongGuid = TestGuidOf.New();
        Assert.NotEqual(Guid.Empty, strongGuid.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnTrueForValidGuid()
    {
        bool isValid = TestGuidOf.TryParse("00000000-0000-0000-0000-000000000001", out TestGuidOf? strongGuid);
        Assert.True(isValid);
        Assert.NotNull(strongGuid);
        Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000001"), strongGuid.Value);
    }

    [Fact]
    public void TryParse_ShouldReturnFalseForInvalidGuid()
    {
        bool isValid = TestGuidOf.TryParse("00000000-0000-0000-0000-00000000000X", out TestGuidOf? strongGuid);
        Assert.False(isValid);
        Assert.Null(strongGuid);
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        TestGuidOf strongGuid = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.Equal("00000000-0000-0000-0000-000000000001", strongGuid.ToString("D"));
    }

    [Fact]
    public void ToStringWithDashes_ShouldReturnCorrectFormat()
    {
        TestGuidOf strongGuid = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.Equal("00000000-0000-0000-0000-000000000001", strongGuid.ToStringWithDashes());
    }

    [Fact]
    public void ToStringWithoutDashes_ShouldReturnCorrectFormat()
    {
        TestGuidOf strongGuid = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.Equal("00000000000000000000000000000001", strongGuid.ToStringWithoutDashes());
    }
}
