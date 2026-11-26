// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using Xunit;

namespace StrongOf.FluentValidation.UnitTests;

public sealed class PrimitiveValidatorTests
{
    private sealed class Flag(bool value) : StrongBoolean<Flag>(value), IStrongOf<bool, Flag>
    {
        public static Flag Create(bool value) => new(value);
    }

    private sealed class Amount(double value) : StrongDouble<Amount>(value), IStrongOf<double, Amount>
    {
        public static Amount Create(double value) => new(value);
    }

    private sealed class Duration(TimeSpan value) : StrongTimeSpan<Duration>(value), IStrongOf<TimeSpan, Duration>
    {
        public static Duration Create(TimeSpan value) => new(value);
    }

    private sealed record Values(Flag? Flag = null, Amount? Amount = null, Duration? Duration = null);

    [Theory]
    [InlineData(null, false, false, false)]
    [InlineData(true, true, true, false)]
    [InlineData(false, true, false, true)]
    public void BooleanRules(bool? value, bool present, bool isTrue, bool isFalse)
    {
        Values model = new(Flag: value.HasValue ? new Flag(value.Value) : null);
        InlineValidator<Values> validator = new();
        validator.RuleFor(x => x.Flag).HasValue().WithErrorCode("present");
        validator.RuleFor(x => x.Flag).IsTrue().WithErrorCode("true");
        validator.RuleFor(x => x.Flag).IsFalse().WithErrorCode("false");

        HashSet<string> errors = validator.Validate(model).Errors.Select(e => e.ErrorCode).ToHashSet(StringComparer.Ordinal);
        Assert.Equal(present, !errors.Contains("present"));
        Assert.Equal(isTrue, !errors.Contains("true"));
        Assert.Equal(isFalse, !errors.Contains("false"));
    }

    [Theory]
    [InlineData(null, false, false, false, false)]
    [InlineData(-1d, true, false, true, false)]
    [InlineData(0d, true, true, true, true)]
    [InlineData(1d, true, true, true, true)]
    [InlineData(2d, true, true, false, false)]
    [InlineData(double.NaN, true, false, false, false)]
    public void DoubleRules(double? value, bool present, bool minimum, bool maximum, bool range)
    {
        Values model = new(Amount: value.HasValue ? new Amount(value.Value) : null);
        InlineValidator<Values> validator = new();
        validator.RuleFor(x => x.Amount).HasValue().WithErrorCode("present");
        validator.RuleFor(x => x.Amount).HasMinimum(0).WithErrorCode("minimum");
        validator.RuleFor(x => x.Amount).HasMaximum(1).WithErrorCode("maximum");
        validator.RuleFor(x => x.Amount).HasRange(0, 1).WithErrorCode("range");

        HashSet<string> errors = validator.Validate(model).Errors.Select(e => e.ErrorCode).ToHashSet(StringComparer.Ordinal);
        Assert.Equal(present, !errors.Contains("present"));
        Assert.Equal(minimum, !errors.Contains("minimum"));
        Assert.Equal(maximum, !errors.Contains("maximum"));
        Assert.Equal(range, !errors.Contains("range"));
    }

    [Theory]
    [InlineData(null, false, false, false, false)]
    [InlineData(-1, true, false, false, true)]
    [InlineData(0, true, false, true, true)]
    [InlineData(1, true, true, true, true)]
    [InlineData(2, true, true, true, false)]
    public void TimeSpanRules(int? ticks, bool present, bool positive, bool minimum, bool maximum)
    {
        Values model = new(Duration: ticks.HasValue ? new Duration(TimeSpan.FromTicks(ticks.Value)) : null);
        InlineValidator<Values> validator = new();
        validator.RuleFor(x => x.Duration).HasValue().WithErrorCode("present");
        validator.RuleFor(x => x.Duration).IsPositive().WithErrorCode("positive");
        validator.RuleFor(x => x.Duration).HasMinimum(TimeSpan.Zero).WithErrorCode("minimum");
        validator.RuleFor(x => x.Duration).HasMaximum(TimeSpan.FromTicks(1)).WithErrorCode("maximum");

        HashSet<string> errors = validator.Validate(model).Errors.Select(e => e.ErrorCode).ToHashSet(StringComparer.Ordinal);
        Assert.Equal(present, !errors.Contains("present"));
        Assert.Equal(positive, !errors.Contains("positive"));
        Assert.Equal(minimum, !errors.Contains("minimum"));
        Assert.Equal(maximum, !errors.Contains("maximum"));
    }

    [Theory]
    [InlineData(null, 3)]
    [InlineData(0d, 2)]
    [InlineData(2d, 0)]
    public void GenericRulesValidateWrappedValue(double? value, int expectedErrors)
    {
        InlineValidator<Values> validator = new();
        validator.RuleFor(x => x.Amount).IsNotNull();
        validator.RuleFor(x => x.Amount).HasNonDefaultValue<Values, Amount, double>();
        validator.RuleFor(x => x.Amount).ValueMust<Values, Amount, double>(v => v > 0);
        Values model = new(Amount: value.HasValue ? new Amount(value.Value) : null);
        Assert.Equal(expectedErrors, validator.Validate(model).Errors.Count);
    }
}
