// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for StrongDateTimeOffset.
/// </summary>
public static class StrongDateTimeOffsetValidators
{
    /// <summary>
    /// Checks if the StrongDateTimeOffset has a value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongDateTimeOffset<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Checks if the StrongDateTimeOffset has a minimum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset min)
        where TStrong : StrongDateTimeOffset<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Checks if the StrongDateTimeOffset has a maximum value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset max)
        where TStrong : StrongDateTimeOffset<TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Checks if the StrongDateTimeOffset is within a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, DateTimeOffset min, DateTimeOffset max)
        where TStrong : StrongDateTimeOffset<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the StrongDateTimeOffset is equal to another StrongDateTimeOffset.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other StrongDateTimeOffset.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongDateTimeOffset<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new EqualValidator<T, TStrong>(func, member, name)!);
    }

    /// <summary>
    /// Validates that the StrongDateTimeOffset is not equal to another StrongDateTimeOffset.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the StrongDateTimeOffset.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other StrongDateTimeOffset.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongDateTimeOffset<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new NotEqualValidator<T, TStrong>(func, member, name)!);
    }
}
