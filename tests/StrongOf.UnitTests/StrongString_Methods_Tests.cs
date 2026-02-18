// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongString_Methods_Tests
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
    public void EqualsStrong_ShouldReturnTrueWhenValuesAreEqual()
    {
        TestStringOf strongString1 = new("Test");
        TestStringOf strongString2 = new("Test");

        Assert.True(strongString1.Equals("Test", StringComparison.Ordinal));
        Assert.True(strongString2.Equals("Test", StringComparison.Ordinal));
    }

    [Fact]
    public void ToLower_ShouldReturnLowerCaseString()
    {
        TestStringOf strongString = new("TEST");
        TestStringOf result = strongString.ToLower();
        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void ToLowerInvariant_ShouldReturnLowerCaseString()
    {
        TestStringOf strongString = new("TEST");
        TestStringOf result = strongString.ToLowerInvariant();
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
    public void ToUpperInvariant_ShouldReturnUpperCaseString()
    {
        TestStringOf strongString = new("test");
        TestStringOf result = strongString.ToUpperInvariant();
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

            Assert.Equal(invalidCharsExpectedSet.ToList(), invalidChars);
        }
        else
        {
            Assert.Empty(invalidCharsExpected);
            Assert.Null(invalidChars);
        }
    }

    public static TheoryData<bool, string, string> ContainsInvalidBytesTestsData =>
        new()
        {
            { false, "", "Hello World" },
            { false, "", "test123" },
            { false, "", "" },
            { true, "🎉", "Hello🎉World" },
            { true, "☀", "Sun☀shine" },
        };

    [Fact]
    public void IsNullOrWhiteSpace_WithWhitespace_ReturnsTrue()
    {
        TestStringOf strongString = new("   ");
        Assert.True(strongString.IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_WithEmpty_ReturnsTrue()
    {
        TestStringOf strongString = new("");
        Assert.True(strongString.IsNullOrWhiteSpace());
    }

    [Fact]
    public void IsNullOrWhiteSpace_WithValue_ReturnsFalse()
    {
        TestStringOf strongString = new("Test");
        Assert.False(strongString.IsNullOrWhiteSpace());
    }

    [Fact]
    public void Contains_Char_ReturnsTrue()
    {
        TestStringOf strongString = new("Test");
        Assert.True(strongString.Contains('T'));
    }

    [Fact]
    public void Contains_Char_ReturnsFalse()
    {
        TestStringOf strongString = new("Test");
        Assert.False(strongString.Contains('X'));
    }

    [Fact]
    public void Contains_String_ReturnsTrue()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.Contains("World", StringComparison.Ordinal));
    }

    [Fact]
    public void Contains_String_WithComparison_ReturnsTrueCaseInsensitive()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.Contains("world", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void StartsWith_Char_ReturnsTrue()
    {
        TestStringOf strongString = new("Test");
        Assert.True(strongString.StartsWith('T'));
    }

    [Fact]
    public void StartsWith_String_ReturnsTrue()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.StartsWith("Hello", StringComparison.Ordinal));
    }

    [Fact]
    public void StartsWith_String_ReturnsFalse()
    {
        TestStringOf strongString = new("Hello World");
        Assert.False(strongString.StartsWith("World", StringComparison.Ordinal));
    }

    [Fact]
    public void StartsWith_String_WithComparison_ReturnsTrueCaseInsensitive()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.StartsWith("hello", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void EndsWith_Char_ReturnsTrue()
    {
        TestStringOf strongString = new("Test");
        Assert.True(strongString.EndsWith('t'));
    }

    [Fact]
    public void EndsWith_String_ReturnsTrue()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.EndsWith("World", StringComparison.Ordinal));
    }

    [Fact]
    public void EndsWith_String_ReturnsFalse()
    {
        TestStringOf strongString = new("Hello World");
        Assert.False(strongString.EndsWith("Hello", StringComparison.Ordinal));
    }

    [Fact]
    public void EndsWith_String_WithComparison_ReturnsTrueCaseInsensitive()
    {
        TestStringOf strongString = new("Hello World");
        Assert.True(strongString.EndsWith("world", StringComparison.OrdinalIgnoreCase));
    }
}
