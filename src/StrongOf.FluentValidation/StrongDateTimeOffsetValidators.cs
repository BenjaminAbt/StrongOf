// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongDateTimeOffset{TStrong}"/> values.
/// </summary>
/// <remarks>
/// Comparisons use native <see cref="DateTimeOffset"/> ordering and therefore preserve
/// offset-aware instant semantics.
/// </remarks>
public static class StrongDateTimeOffsetValidators
{
    /// <summary>
    /// Validates that the strong date-time-offset has a value (is not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong date-time-offset is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset min)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Validates that the strong date-time-offset is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset max)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Validates that the strong date-time-offset is within the specified inclusive range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset min, DateTimeOffset max)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the StrongDateTimeOffset is equal to another StrongDateTimeOffset.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong DateTimeOffset to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Use FluentValidation's native EqualValidator to preserve standard cross-property
        // message formatting and placeholders.
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the StrongDateTimeOffset is not equal to another StrongDateTimeOffset.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong DateTimeOffset to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo behavior with FluentValidation's built-in NotEqualValidator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }
}
