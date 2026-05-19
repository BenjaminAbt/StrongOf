// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongTimeSpan{TStrong}"/> values.
/// </summary>
public static class StrongTimeSpanValidators
{
    /// <summary>
    /// Checks whether the strong time span has a value (is not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong time span is positive (greater than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsPositive<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
        => rule.Must(strong => strong is not null && strong.Value > TimeSpan.Zero);

    /// <summary>
    /// Validates that the strong time span is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum duration.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, TimeSpan min)
        where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Validates that the strong time span is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum duration.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, TimeSpan max)
        where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);
}
