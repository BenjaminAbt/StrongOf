// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

public class StrongString_Properties_Tests
{
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value), IStrongOf<string, TestStringOf>
    {
        public static TestStringOf Create(string value) => new(value);
    }

    [Fact]
    public void Length_ReturnsCorrectLength()
    {
        // Arrange
        TestStringOf testString = new("test");

        // Act
        int length = testString.Length;
        int lengthValue = testString.Value.Length;

        // Assert
        Assert.Equal(4, length);
        Assert.Equal(4, lengthValue);
    }
}
