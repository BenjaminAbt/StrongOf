// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

/// <summary>
/// Tests for StrongChar FluentValidation extensions.
/// </summary>
public class StrongCharValidatorsTests
{
    private class TestValidator : AbstractValidator<TestModel> { }
    private readonly TestValidator _validator = new();

    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value);

    private class TestModel
    {
        public TestCharOf? Strong { get; set; }
        public TestCharOf? Other { get; set; }
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
    public void HasValue_ShouldPass_WhenValidChar()
    {
        _validator.RuleFor(x => x.Strong).HasValue();
        TestModel model = new() { Strong = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsLetter ====================

    [Fact]
    public void IsLetter_ShouldPass_WhenLetter()
    {
        _validator.RuleFor(x => x.Strong).IsLetter();
        TestModel model = new() { Strong = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsLetter_ShouldFail_WhenDigit()
    {
        _validator.RuleFor(x => x.Strong).IsLetter();
        TestModel model = new() { Strong = new TestCharOf('1') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsLetter_ShouldFail_WhenSymbol()
    {
        _validator.RuleFor(x => x.Strong).IsLetter();
        TestModel model = new() { Strong = new TestCharOf('@') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsDigit ====================

    [Fact]
    public void IsDigit_ShouldPass_WhenDigit()
    {
        _validator.RuleFor(x => x.Strong).IsDigit();
        TestModel model = new() { Strong = new TestCharOf('5') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsDigit_ShouldFail_WhenLetter()
    {
        _validator.RuleFor(x => x.Strong).IsDigit();
        TestModel model = new() { Strong = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsLetterOrDigit ====================

    [Fact]
    public void IsLetterOrDigit_ShouldPass_WhenLetter()
    {
        _validator.RuleFor(x => x.Strong).IsLetterOrDigit();
        TestModel model = new() { Strong = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsLetterOrDigit_ShouldPass_WhenDigit()
    {
        _validator.RuleFor(x => x.Strong).IsLetterOrDigit();
        TestModel model = new() { Strong = new TestCharOf('5') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsLetterOrDigit_ShouldFail_WhenSymbol()
    {
        _validator.RuleFor(x => x.Strong).IsLetterOrDigit();
        TestModel model = new() { Strong = new TestCharOf('@') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsEqualTo ====================

    [Fact]
    public void IsEqualTo_ShouldPass_WhenEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestCharOf('A'), Other = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsEqualTo_ShouldFail_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestCharOf('A'), Other = new TestCharOf('B') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsNotEqualTo ====================

    [Fact]
    public void IsNotEqualTo_ShouldPass_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestCharOf('A'), Other = new TestCharOf('B') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotEqualTo_ShouldFail_WhenEqual()
    {
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestCharOf('A'), Other = new TestCharOf('A') };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }
}
