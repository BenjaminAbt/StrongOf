// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for StrongChar.
/// </summary>
public static class StrongCharValidators
{
    /// <summary>
    /// Validates that the strong char has a value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongChar<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong char is a letter.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsLetter<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongChar<TStrong>
        => rule.Must(strong => strong is not null && char.IsLetter(strong.Value));

    /// <summary>
    /// Validates that the strong char is a digit.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsDigit<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongChar<TStrong>
        => rule.Must(strong => strong is not null && char.IsDigit(strong.Value));

    /// <summary>
    /// Validates that the strong char is a letter or digit.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsLetterOrDigit<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongChar<TStrong>
        => rule.Must(strong => strong is not null && char.IsLetterOrDigit(strong.Value));

    /// <summary>
    /// Validates that the strong char is equal to another strong char.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong char.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongChar<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new EqualValidator<T, TStrong>(func, member, name)!);
    }

    /// <summary>
    /// Validates that the strong char is not equal to another strong char.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong char.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong char.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongChar<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new NotEqualValidator<T, TStrong>(func, member, name)!);
    }
}
