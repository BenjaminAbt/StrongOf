// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongDateTime{TStrong}"/> values.
/// </summary>
/// <remarks>
/// Comparisons are performed using raw <see cref="DateTime"/> ordering. Ensure that compared
/// values use compatible <see cref="DateTime.Kind"/> semantics in your domain.
/// </remarks>
public static class StrongDateTimeValidators
{
    /// <summary>
    /// Validates that the strong date-time has a value (is not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong date-time is greater than or equal to the specified minimum.
    /// </summary>
    /// <remarks>
    /// No kind normalization is applied. Pass values in the same temporal convention
    /// (for example all UTC or all local) to avoid ambiguous comparisons.
    /// </remarks>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTime min)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Validates that the strong date-time is less than or equal to the specified maximum.
    /// </summary>
    /// <remarks>
    /// No kind normalization is applied. Pass values in the same temporal convention
    /// (for example all UTC or all local) to avoid ambiguous comparisons.
    /// </remarks>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTime max)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Validates that the strong date-time is within the specified inclusive range.
    /// </summary>
    /// <remarks>
    /// Range bounds are inclusive and compared directly against the wrapped <see cref="DateTime"/>.
    /// </remarks>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTime min, DateTime max)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the strong DateTime is equal to another strong DateTime.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong DateTime to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Reuse FluentValidation's native validator to preserve placeholder formatting and
        // message behavior for cross-property comparisons.
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the strong DateTime is not equal to another strong DateTime.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong DateTime.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong DateTime to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDateTime<TStrong>, IStrongOf<DateTime, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo behavior with FluentValidation's native NotEqual validator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }
}
