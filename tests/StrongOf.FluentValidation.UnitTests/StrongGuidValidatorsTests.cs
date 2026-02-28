// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

/// <summary>
/// Tests for StrongGuid FluentValidation extensions.
/// </summary>
public class StrongGuidValidatorsTests
{
    private class TestValidator : AbstractValidator<TestModel> { }
    private readonly TestValidator _validator = new();

    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value);

    private class TestModel
    {
        public TestGuidOf? Strong { get; set; }
        public TestGuidOf? Other { get; set; }
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
    public void HasValue_ShouldFail_WhenEmptyGuid()
    {
        _validator.RuleFor(x => x.Strong).HasValue();
        TestModel model = new() { Strong = new TestGuidOf(Guid.Empty) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void HasValue_ShouldPass_WhenValidGuid()
    {
        _validator.RuleFor(x => x.Strong).HasValue();
        TestModel model = new() { Strong = new TestGuidOf(Guid.NewGuid()) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsNotEmpty ====================

    [Fact]
    public void IsNotEmpty_ShouldFail_WhenEmptyGuid()
    {
        _validator.RuleFor(x => x.Strong).IsNotEmpty();
        TestModel model = new() { Strong = new TestGuidOf(Guid.Empty) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotEmpty_ShouldPass_WhenValidGuid()
    {
        _validator.RuleFor(x => x.Strong).IsNotEmpty();
        TestModel model = new() { Strong = new TestGuidOf(Guid.NewGuid()) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsEqualTo ====================

    [Fact]
    public void IsEqualTo_ShouldPass_WhenEqual()
    {
        Guid guid = Guid.NewGuid();
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestGuidOf(guid), Other = new TestGuidOf(guid) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsEqualTo_ShouldFail_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestGuidOf(Guid.NewGuid()), Other = new TestGuidOf(Guid.NewGuid()) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }

    // ==================== IsNotEqualTo ====================

    [Fact]
    public void IsNotEqualTo_ShouldPass_WhenNotEqual()
    {
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestGuidOf(Guid.NewGuid()), Other = new TestGuidOf(Guid.NewGuid()) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Strong);
    }

    [Fact]
    public void IsNotEqualTo_ShouldFail_WhenEqual()
    {
        Guid guid = Guid.NewGuid();
        _validator.RuleFor(x => x.Strong).IsNotEqualTo(x => x.Other!);
        TestModel model = new() { Strong = new TestGuidOf(guid), Other = new TestGuidOf(guid) };
        TestValidationResult<TestModel> result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Strong);
    }
}
