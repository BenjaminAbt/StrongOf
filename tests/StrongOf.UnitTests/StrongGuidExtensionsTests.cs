using Xunit;

namespace StrongOf.UnitTests;

public class StrongGuidExtensionsTests
{
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value) { }
    private sealed class OtherTestGuidOf(Guid Value) : StrongGuid<OtherTestGuidOf>(Value) { }

    [Fact]
    public void Empty_ShouldReturnEmptyGuid()
    {
        TestGuidOf strongGuid = TestGuidOf.Empty();
        Assert.Equal(Guid.Empty, strongGuid.Value);
    }

    [Fact]
    public void Empty_ShouldReturnTrueForEmptyGuid()
    {
        TestGuidOf strongGuid = TestGuidOf.Empty();
        Assert.True(strongGuid.IsEmpty());
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
