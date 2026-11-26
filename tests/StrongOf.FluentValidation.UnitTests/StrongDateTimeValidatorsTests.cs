// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

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
    private sealed class TestStrongDateTimeOf(DateTime Value) : StrongDateTime<TestStrongDateTimeOf>(Value), IStrongOf<DateTime, TestStrongDateTimeOf>
    {
        public static TestStrongDateTimeOf Create(DateTime value) => new(value);
    }

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

    [Fact]
    public void HasRange_ShouldNotFail_WhenStrongDateTimeIsWithinRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(new(2021, 1, 1), new(2023, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2022, 6, 15)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenStrongDateTimeIsBeforeMin()
    {
        _validator.RuleFor(x => x.Strong).HasRange(new(2021, 1, 1), new(2023, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2020, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenStrongDateTimeIsAfterMax()
    {
        _validator.RuleFor(x => x.Strong).HasRange(new(2021, 1, 1), new(2023, 1, 1));

        TestModel model = new() { Strong = new(new DateTime(2024, 1, 1)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void AccessorComparesTheOtherProperty()
    {
        TestModel model = new() { Strong = new(DateTime.UnixEpoch), Other = new(DateTime.UnixEpoch) };
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
