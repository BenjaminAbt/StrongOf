// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

/// <summary>
/// Tests for StrongDecimal FluentValidation extensions.
/// </summary>
public class StrongDecimalValidatorsTests
{
    private class TestValidator : AbstractValidator<TestModel> { }
    private readonly TestValidator _validator = new();

    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value);

    private class TestModel
    {
        public TestDecimalOf? Strong { get; set; }
        public TestDecimalOf? Other { get; set; }
    }

    // ==================== HasValue ====================

    [Fact]
    public void HasValue_ShouldFail_WhenNull()
    {
        _validator.RuleFor(x => x.Strong).HasValue();
        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasValue_ShouldPass_WhenValid()
    {
        _validator.RuleFor(x => x.Strong).HasValue();
        TestModel model = new() { Strong = new TestDecimalOf(99.99m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== HasMinimum ====================

    [Fact]
    public void HasMinimum_ShouldPass_WhenAboveMinimum()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(10m);
        TestModel model = new() { Strong = new TestDecimalOf(15m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldPass_WhenEqualToMinimum()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(10m);
        TestModel model = new() { Strong = new TestDecimalOf(10m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimum_ShouldFail_WhenBelowMinimum()
    {
        _validator.RuleFor(x => x.Strong).HasMinimum(10m);
        TestModel model = new() { Strong = new TestDecimalOf(5m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== HasMaximum ====================

    [Fact]
    public void HasMaximum_ShouldPass_WhenBelowMaximum()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(100m);
        TestModel model = new() { Strong = new TestDecimalOf(50m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximum_ShouldFail_WhenAboveMaximum()
    {
        _validator.RuleFor(x => x.Strong).HasMaximum(100m);
        TestModel model = new() { Strong = new TestDecimalOf(150m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== HasRange ====================

    [Fact]
    public void HasRange_ShouldPass_WhenWithinRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(10m, 100m);
        TestModel model = new() { Strong = new TestDecimalOf(50m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenBelowRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(10m, 100m);
        TestModel model = new() { Strong = new TestDecimalOf(5m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasRange_ShouldFail_WhenAboveRange()
    {
        _validator.RuleFor(x => x.Strong).HasRange(10m, 100m);
        TestModel model = new() { Strong = new TestDecimalOf(150m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsPositive ====================

    [Fact]
    public void IsPositive_ShouldPass_WhenPositive()
    {
        _validator.RuleFor(x => x.Strong).IsPositive();
        TestModel model = new() { Strong = new TestDecimalOf(1m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsPositive_ShouldFail_WhenZero()
    {
        _validator.RuleFor(x => x.Strong).IsPositive();
        TestModel model = new() { Strong = new TestDecimalOf(0m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsPositive_ShouldFail_WhenNegative()
    {
        _validator.RuleFor(x => x.Strong).IsPositive();
        TestModel model = new() { Strong = new TestDecimalOf(-1m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsNotNegative ====================

    [Fact]
    public void IsNotNegative_ShouldPass_WhenPositive()
    {
        _validator.RuleFor(x => x.Strong).IsNotNegative();
        TestModel model = new() { Strong = new TestDecimalOf(1m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotNegative_ShouldPass_WhenZero()
    {
        _validator.RuleFor(x => x.Strong).IsNotNegative();
        TestModel model = new() { Strong = new TestDecimalOf(0m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotNegative_ShouldFail_WhenNegative()
    {
        _validator.RuleFor(x => x.Strong).IsNotNegative();
        TestModel model = new() { Strong = new TestDecimalOf(-1m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsEqualTo ====================

    [Fact]
    public void IsEqualTo_ShouldPass_WhenEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestDecimalOf(99.99m), Other = new TestDecimalOf(99.99m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsEqualTo_ShouldFail_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestDecimalOf(99.99m), Other = new TestDecimalOf(50m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsNotEqualTo ====================

    [Fact]
    public void IsNotEqualTo_ShouldPass_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestDecimalOf(99.99m), Other = new TestDecimalOf(50m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotEqualTo_ShouldFail_WhenEqual()
    {
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestDecimalOf(99.99m), Other = new TestDecimalOf(99.99m) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }
}
