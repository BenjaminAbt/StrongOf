using Xunit;

namespace StrongOf.Tests;

public class StrongDateTime_Operators_Tests
{
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value) { }
    private sealed class OtherTestDateTimeOf(DateTime Value) : StrongDateTime<OtherTestDateTimeOf>(Value) { }

    [Fact]
    public void OperatorEquals_ShouldReturnTrueForEqualValues()
    {
        TestDateTimeOf strongDateTime = new(new DateTime(2000, 1, 1));
        Assert.True(strongDateTime == new DateTime(2000, 1, 1));
    }

    [Fact]
    public void OperatorNotEquals_ShouldReturnTrueForDifferentValues()
    {
        TestDateTimeOf strongDateTime = new(new DateTime(2000, 1, 1));
        Assert.True(strongDateTime != new DateTime(2001, 1, 1));
    }

    [Fact]
    public void OperatorEquals_Null()
    {
        TestDateTimeOf strongDateTime = new(new DateTime(2000, 1, 1));
        Assert.True(strongDateTime != null);
        Assert.False(strongDateTime == null);
    }


    [Theory]
    [InlineData("2022-01-01T00:00:00", "2022-01-02T00:00:00")]
    [InlineData("2022-01-02T00:00:00", "2022-01-01T00:00:00")]
    [InlineData("2022-01-01T00:00:00", "2022-01-01T00:00:00")]
    public void OperatorGreaterThan_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOf testStrongDateTime = new(DateTime.Parse(value));
        DateTime otherDateTime = DateTime.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTime.Value > otherDateTime), (testStrongDateTime > otherDateTime));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00", "2022-01-02T00:00:00")]
    [InlineData("2022-01-02T00:00:00", "2022-01-01T00:00:00")]
    [InlineData("2022-01-01T00:00:00", "2022-01-01T00:00:00")]
    public void OperatorLessThan_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOf testStrongDateTime = new(DateTime.Parse(value));
        DateTime otherDateTime = DateTime.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTime.Value < otherDateTime), (testStrongDateTime < otherDateTime));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00", "2022-01-02T00:00:00")]
    [InlineData("2022-01-02T00:00:00", "2022-01-01T00:00:00")]
    [InlineData("2022-01-01T00:00:00", "2022-01-01T00:00:00")]
    public void OperatorLessThanOrEqual_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOf testStrongDateTime = new(DateTime.Parse(value));
        DateTime otherDateTime = DateTime.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTime.Value <= otherDateTime), (testStrongDateTime <= otherDateTime));
    }

    [Theory]
    [InlineData("2022-01-01T00:00:00", "2022-01-02T00:00:00")]
    [InlineData("2022-01-02T00:00:00", "2022-01-01T00:00:00")]
    [InlineData("2022-01-01T00:00:00", "2022-01-01T00:00:00")]
    public void OperatorGreaterThanOrEqual_ReturnsCorrectResult(string value, string other)
    {
        // Arrange
        TestDateTimeOf testStrongDateTime = new(DateTime.Parse(value));
        DateTime otherDateTime = DateTime.Parse(other);

        // Assert
        Assert.Equal((testStrongDateTime.Value >= otherDateTime), (testStrongDateTime >= otherDateTime));
    }
}
