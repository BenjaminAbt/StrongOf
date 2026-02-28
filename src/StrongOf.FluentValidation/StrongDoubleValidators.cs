// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for StrongDouble.
/// </summary>
public static class StrongDoubleValidators
{
    /// <summary>
    /// Checks if the StrongDouble has a value (is not null).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Double.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDouble<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Checks if the StrongDouble has a minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Double.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, double min)
        where TStrong : StrongDouble<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Checks if the StrongDouble has a maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Double.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, double max)
        where TStrong : StrongDouble<TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Checks if the StrongDouble is within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Double.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, double min, double max)
        where TStrong : StrongDouble<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);
}
