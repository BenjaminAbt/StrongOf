// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongInt64{TStrong}"/> values.
/// </summary>
public static class StrongInt64Validators
{
    /// <summary>
    /// Validates that the strong 64-bit integer has a value (is not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong 64-bit integer is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, long min)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Validates that the strong 64-bit integer is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, long max)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Validates that the strong 64-bit integer is within the specified inclusive range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, long min, long max)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the strong Int64 is equal to another strong Int64.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong Int64 to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Use FluentValidation's built-in EqualValidator to preserve standard
        // cross-property comparison messages.
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the strong Int64 is not equal to another strong Int64.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int64.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong Int64 to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongInt64<TStrong>, IStrongOf<long, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo behavior with FluentValidation's native NotEqualValidator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }
}
