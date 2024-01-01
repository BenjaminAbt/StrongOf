using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

public class StrongDateTimeValidatorsTests
{
    // Validator
    private class TestValidator : AbstractValidator<TestModel> { }

    private readonly TestValidator _validator = new();

    // Model
    private sealed class TestStrongDateTimeOf(DateTime Value) : StrongDateTime<TestStrongDateTimeOf>(Value) { }

    private class TestModel
    {
        public TestStrongDateTimeOf? Strong { get; set; }
        public TestStrongDateTimeOf? Other { get; set; }
    }

    [Fact]
    public void HasValue_ShouldFail_WhenStrongDateTimeIsNull()
    {
        _validator.RuleFor(x => x.Strong).HasValue();

        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldFail_WhenStrongDateTimeIsLessThanMin()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(new(2022, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2021, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldNotFail_WhenStrongDateTimeIsGreaterThanMin()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(new(2021, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2022, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximum_ShouldFail_WhenStrongDateTimeIsGreaterThanMax()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(new(2022, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2023, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximum_ShouldNotFail_WhenStrongDateTimeIsLessThanMax()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(new(2023, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2022, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }
}
