// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

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
    private sealed class TestStrongDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestStrongDateTimeOffsetOf>(Value), IStrongOf<DateTimeOffset, TestStrongDateTimeOffsetOf>
    {
        public static TestStrongDateTimeOffsetOf Create(DateTimeOffset value) => new(value);
    }

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

    [Fact]
    public void HasRange_ShouldNotFail_WhenStrongDateTimeOffsetIsWithinRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(
            new(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new(2023, 1, 1, 0, 0, 0, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2022, 6, 15, 12, 0, 0, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenStrongDateTimeOffsetIsBeforeMin()
    {
        _validator.RuleFor(x => x.Strong).HasRange(
            new(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new(2023, 1, 1, 0, 0, 0, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenStrongDateTimeOffsetIsAfterMax()
    {
        _validator.RuleFor(x => x.Strong).HasRange(
            new(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
            new(2023, 1, 1, 0, 0, 0, TimeSpan.Zero));

        TestModel model = new() { Strong = new(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void AccessorComparesTheOtherProperty()
    {
        TestModel model = new() { Strong = new(DateTimeOffset.UnixEpoch), Other = new(DateTimeOffset.UnixEpoch) };
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
