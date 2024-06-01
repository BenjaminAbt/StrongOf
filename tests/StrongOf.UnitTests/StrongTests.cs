using Xunit;

namespace StrongOf.Tests;

public class StrongTests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }

    [Fact]
    public void IsNull_ShouldReturnTrue_WhenStrongIsNull()
    {
        TestStringOf? strong = null;
        Assert.True(Strong.IsNull(strong));
    }

    [Fact]
    public void IsNull_ShouldReturnFalse_WhenStrongIsNotNull()
    {
        TestStringOf strong = new("");
        Assert.False(Strong.IsNull(strong));
    }

    [Fact]
    public void IsNotNull_ShouldReturnTrue_WhenStrongIsNotNull()
    {
        TestStringOf strong = new("");
        Assert.True(Strong.IsNotNull(strong));
    }

    [Fact]
    public void IsNotNull_ShouldReturnFalse_WhenStrongIsNull()
    {
        TestStringOf? strong = null;
        Assert.False(Strong.IsNotNull(strong));
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenStrongStringIsNull()
    {
        TestStringOf? strongString = null;
        Assert.True(Strong.IsNullOrEmpty(strongString));
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenStrongStringIsEmpty()
    {
        TestStringOf strongString = new("");
        Assert.True(Strong.IsNullOrEmpty(strongString));
        Assert.False(Strong.HasValue(strongString));
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReturnFalse_WhenStrongStringIsNotNullOrEmpty()
    {
        TestStringOf strongString = new("test");
        Assert.False(Strong.IsNullOrEmpty(strongString));
        Assert.True(Strong.HasValue(strongString));
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldReturnTrue_WhenStrongStringIsNotNullOrEmpty()
    {
        TestStringOf strongString = new("test");
        Assert.True(Strong.IsNotNullOrEmpty(strongString));
        Assert.True(Strong.HasValue(strongString));
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldReturnFalse_WhenStrongStringIsNull()
    {
        TestStringOf? strongString = null;
        Assert.False(Strong.IsNotNullOrEmpty(strongString));
        Assert.False(Strong.HasValue(strongString));
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldReturnFalse_WhenStrongStringIsEmpty()
    {
        TestStringOf strongString = new("");
        Assert.False(Strong.IsNotNullOrEmpty(strongString));
        Assert.False(Strong.HasValue(strongString));
    }
}
