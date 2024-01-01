using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

public class StrongDateTimeOffsetValidatorsTests
{
    // Validator
    private class TestValidator : AbstractValidator<TestModel> { }

    private readonly TestValidator _validator = new();

    // Model
    private sealed class TestStrongDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestStrongDateTimeOffsetOf>(Value) { }

    private class TestModel
    {
        public TestStrongDateTimeOffsetOf? Strong { get; set; }
        public TestStrongDateTimeOffsetOf? Other { get; set; }
    }

    [Fact]
    public void HasValue_ShouldFail_WhenStrongDateTimeOffsetIsNull()
    {
        _validator.RuleFor(x => x.Strong).HasValue();

        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldFail_WhenStrongDateTimeOffsetIsLessThanMin()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(new(2022, 1, 1, 23, 59, 59, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2021, 1, 1, 23, 59, 59, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldNotFail_WhenStrongDateTimeIsGreaterThanMin()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(new(2021, 1, 1, 23, 59, 59, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2022, 1, 1, 23, 59, 59, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximum_ShouldFail_WhenStrongDateTimeIsGreaterThanMax()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(new(2022, 1, 1, 23, 59, 59, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2023, 1, 1, 23, 59, 59, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximum_ShouldNotFail_WhenStrongDateTimeIsLessThanMax()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(new(2023, 1, 1, 23, 59, 59, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2022, 1, 1, 23, 59, 59, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }
}
