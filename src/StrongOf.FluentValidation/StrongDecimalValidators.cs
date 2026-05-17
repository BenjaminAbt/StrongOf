// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongDecimal{TStrong}"/> values.
/// </summary>
public static class StrongDecimalValidators
{
    /// <summary>
    /// Validates that the strong decimal has a value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong decimal has a minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, decimal min)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Validates that the strong decimal has a maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, decimal max)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Validates that the strong decimal is within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, decimal min, decimal max)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the strong decimal is positive (greater than zero).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsPositive<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null && strong.Value > 0);

    /// <summary>
    /// Validates that the strong decimal is not negative (greater than or equal to zero).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotNegative<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= 0);

    /// <summary>
    /// Validates that the strong decimal is equal to another strong decimal.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong decimal to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Use FluentValidation's native EqualValidator to keep cross-property comparison
        // error messages consistent with standard Equal(...) rules.
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the strong decimal is not equal to another strong decimal.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong decimal.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong decimal to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDecimal<TStrong>, IStrongOf<decimal, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo semantics with FluentValidation's built-in NotEqualValidator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }
}
