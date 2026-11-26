// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

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
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value), IStrongOf<long, TestInt64Of>
    {
        public static TestInt64Of Create(long value) => new(value);
    }

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

    [Fact]
    public void AccessorComparesTheOtherProperty()
    {
        TestModel model = new() { Strong = new(1L), Other = new(1L) };
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other, nameof(TestModel.Other));
        Assert.True(_validator.Validate(model).IsValid);
        TestValidator different = new();
        different.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other, nameof(TestModel.Other));
        Assert.False(different.Validate(model).IsValid);
        model.Other = null;
        Assert.False(_validator.Validate(model).IsValid);
        Assert.True(different.Validate(model).IsValid);
    }
}
