// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for <see cref="StrongTimeSpan{TStrong}"/>.
/// </summary>
public static class StrongTimeSpanValidators
{
    /// <summary>
    /// Checks if the StrongTimeSpan has a value (is not null).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongTimeSpan<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Checks if the StrongTimeSpan is positive (greater than <see cref="TimeSpan.Zero"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsPositive<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongTimeSpan<TStrong>
        => rule.Must(strong => strong is not null && strong.Value > TimeSpan.Zero);

    /// <summary>
    /// Checks if the StrongTimeSpan has a minimum duration.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum duration.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, TimeSpan min)
        where TStrong : StrongTimeSpan<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Checks if the StrongTimeSpan has a maximum duration.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong TimeSpan.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum duration.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, TimeSpan max)
        where TStrong : StrongTimeSpan<TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);
}
