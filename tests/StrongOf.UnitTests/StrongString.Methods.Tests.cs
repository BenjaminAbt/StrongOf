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

    [Theory]
    // https://github.com/xunit/xunit/issues/2024
    [MemberData(nameof(ContainsInvalidBytesTestsData), DisableDiscoveryEnumeration = true)]
    public void ContainsInvalidCharsTests(bool expected, string invalidCharsExpected, string input)
    {
        const string allowedChars =
            " !\"§#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`´abcdefghijklmnopqrstuvwxyz{|}~c«r°²³'µ·»¼AÄIÖÜßàáaäèéíñöü--–',.?€";

        TestStringOf stringOf = new(input);

        HashSet<char> allowedCharsSet = new(allowedChars);
        Assert.Equal(expected, stringOf.ContainsInvalidChars(allowedCharsSet, out ICollection<char>? invalidChars));

        if (expected)
        {
            Assert.NotEmpty(invalidCharsExpected);
            HashSet<char> invalidCharsExpectedSet = new(invalidCharsExpected.ToCharArray());

            Assert.Equal(invalidCharsExpectedSet, invalidChars);
        }
        else
        {
            Assert.Empty(invalidCharsExpected);
            Assert.Null(invalidChars);
        }
    }

    public static IEnumerable<object[]> ContainsInvalidBytesTestsData()
    {
        yield return new object[] { true, "𝐻𝑒𝓁𝑜𝒯𝓈𝓉", "𝐻𝑒𝓁𝓁𝑜 𝒯𝑒𝓈𝓉" };
        yield return new object[] { false, "", "Object list Filter elements" };
        yield return new object[] { false, "", "4th annual C# advent" };
        yield return new object[] { false, "", "Why is Serialport.ReadExisting with binary data so slow with C#?" };
        yield return new object[] { false, "", "[solved] How can I display a print dialog for an RDLC report from the code?" };
        yield return new object[] { false, "", "Regex: Find everything between an @ and a whitespace/end of string" };
        yield return new object[] { false, "", "dll created with .NET Core and Roslyn throws \"The type 'Object' is defined...\" Error in another dll" };
    }
}
