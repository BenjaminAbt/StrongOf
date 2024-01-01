using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

public class StrongInt64ValidatorsTests
{
    // Validator
    private class TestValidator : AbstractValidator<TestModel> { }

    private readonly TestValidator _validator = new();

    private class TestModel
    {
        public TestInt64Of? Strong { get; set; }
        public TestInt64Of? Other { get; set; }
    }

    // Model
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value) { }

    [Fact]
    public void HasValue_ShouldFail_WhenNull()
    {
        _validator.RuleFor(x => x.Strong).HasValue();

        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldFail_WhenLessThanMin()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(6);

        TestModel model = new() { Strong = new TestInt64Of(5) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void HasMaximum_ShouldFail_WhenGreaterThanMax()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(4);

        TestModel model = new() { Strong = new TestInt64Of(5) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenOutsideRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(10, 20);

        TestModel model = new() { Strong = new TestInt64Of(5) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void HasRange_ShouldPass_WhenWithinRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(10, 20);

        TestModel model = new() { Strong = new TestInt64Of(15) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        Assert.True(result.IsValid);
    }
}
