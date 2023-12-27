using Xunit;

namespace StrongOf.UnitTests;

public class StrongStringMethodsTests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }


    [Fact]
    public void Trim_ShouldRemoveLeadingAndTrailingWhiteSpace()
    {
        TestStringOf strongString = new(" Test ");
        TestStringOf result = strongString.Trim();
        Assert.Equal("Test", result.Value);
    }

    [Fact]
    public void TrimStart_ShouldRemoveLeadingWhiteSpace()
    {
        TestStringOf strongString = new(" Test");
        StrongString<TestStringOf> result = strongString.TrimStart();
        Assert.Equal("Test", result.Value);
    }

    [Fact]
    public void TrimEnd_ShouldRemoveTrailingWhiteSpace()
    {
        TestStringOf strongString = new("Test ");
        StrongString<TestStringOf> result = strongString.TrimEnd();
        Assert.Equal("Test", result.Value);
    }

    [Fact]
    public void Equals_ShouldReturnTrueWhenValuesAreEqual()
    {
        TestStringOf strongString = new("Test");

        Assert.True(strongString.Equals("Test", StringComparison.Ordinal));
        Assert.False(strongString.Equals("test", StringComparison.Ordinal));
    }

    [Fact]
    public void ToLower_ShouldReturnLowerCaseString()
    {
        TestStringOf strongString = new("TEST");
        TestStringOf result = strongString.ToLower();
        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void ToUpper_ShouldReturnUpperCaseString()
    {
        TestStringOf strongString = new("test");
        TestStringOf result = strongString.ToUpper();
        Assert.Equal("TEST", result.Value);
    }

    [Fact]
    public void FirstChar_ShouldReturnFirstCharacterOfString()
    {
        TestStringOf strongString = new("Test");
        char result = strongString.FirstChar();
        Assert.Equal('T', result);
    }

    [Fact]
    public void FirstCharUpperInvariant_ShouldReturnFirstCharacterOfStringInUpperCase()
    {
        TestStringOf strongString = new("test");
        char result = strongString.FirstCharUpperInvariant();
        Assert.Equal('T', result);
    }
}
