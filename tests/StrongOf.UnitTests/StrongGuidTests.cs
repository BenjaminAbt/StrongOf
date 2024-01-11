using Xunit;

namespace StrongOf.UnitTests;

public class StrongGuidTests
{
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value) { }
    private sealed class OtherTestGuidOf(Guid Value) : StrongGuid<OtherTestGuidOf>(Value) { }

    [Fact]
    public void FromGuid_NullValue_ReturnsNull()
    {
        Assert.Null(TestGuidOf.FromGuid(null));
    }

    [Fact]
    public void FromGuid_ValidValue_ReturnsStrongGuid()
    {
        Guid guid = Guid.NewGuid();
        TestGuidOf strongGuid = TestGuidOf.FromGuid(guid);

        Assert.NotNull(strongGuid);
        Assert.Equal(guid, strongGuid.AsGuid());
    }

    [Fact]
    public void FromString_NullValue_ReturnsNull()
    {
        Assert.Null(TestGuidOf.FromString(null));
    }

    [Fact]
    public void FromString_InvalidValue_ReturnsNull()
    {
        Assert.Null(TestGuidOf.FromString("invalid"));
    }

    [Fact]
    public void FromString_ValidValue_ReturnsStrongGuid()
    {
        Guid guid = Guid.NewGuid();
        TestGuidOf strongGuid = TestGuidOf.FromString(guid.ToString());

        Assert.NotNull(strongGuid);
        Assert.Equal(guid, strongGuid.AsGuid());
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        TestGuidOf testOf = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        OtherTestGuidOf otherTestOf2 = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));

        Assert.False(testOf.Equals(otherTestOf2));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsTrue()
    {
        TestGuidOf testOf = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.True(testOf.Equals(Guid.Parse("00000000-0000-0000-0000-000000000001")));
    }

    [Fact]
    public void Equals_WithTargetType_ReturnsFalse()
    {
        TestGuidOf testOf = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.False(testOf.Equals(Guid.Parse("00000000-0000-0000-0000-000000000002")));
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
    public void OperatorEquals_Null()
    {
        TestGuidOf strongGuid = new(Guid.Parse("00000000-0000-0000-0000-000000000001"));
        Assert.True(strongGuid != null);
        Assert.False(strongGuid == null);
    }
}
