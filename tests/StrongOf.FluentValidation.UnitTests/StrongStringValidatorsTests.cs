// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

public class StrongStringValidatorsTests
{
    // Validator
    private class TestValidator : AbstractValidator<TestModel> { }

    private readonly TestValidator _validator = new();

    // Model
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value) { }

    private class TestModel
    {
        public TestStringOf? Strong { get; set; }
        public TestStringOf? Other { get; set; }
    }

    [Fact]
    public void HasValue_ShouldFail_WhenValueIsNull()
    {
        _validator.RuleFor(x => x.Strong).HasValue();

        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMinimumLength_ShouldFail_WhenLengthIsLessThanMinLength()
    {
        _validator.RuleFor(x => x.Strong).HasMinimumLength(5);
        TestModel model = new() { Strong = new("test") };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximumLength_ShouldFail_WhenLengthIsGreaterThanMaxLength()
    {
        _validator.RuleFor(x => x.Strong).HasMaximumLength(5);
        TestModel model = new() { Strong = new("testing") };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasMaximumLength_ShouldNotFail_WhenValueIsNull()
    {
        _validator.RuleFor(x => x.Strong).HasMaximumLength(5);

        TestModel model = new() { Strong = null };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsRegexMatch_ShouldFail_WhenValueDoesNotMatchRegex()
    {
        _validator.RuleFor(x => x.Strong).IsRegexMatch(new Regex(@"^\d+$", RegexOptions.None, matchTimeout: TimeSpan.FromSeconds(1)));
        TestModel model = new() { Strong = new("test") };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsEqualTo_ShouldFail_WhenValuesAreNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);

        TestModel model = new() { Strong = new("test"), Other = new("other") };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void AllowedChars_ShouldFail_WhenValueContainsNotAllowedChars()
    {
        _validator.RuleFor(x => x.Strong).AllowedChars(['a', 'b', 'c'], "Invalid characters: {0}");
        TestModel model = new() { Strong = new("test") };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }
}
