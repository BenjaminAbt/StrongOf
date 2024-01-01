using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for StrongInt32.
/// </summary>
public static class StrongIn32Validators
{
    /// <summary>
    /// Checks if the StrongInt32 has a value.
    /// </summary>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongInt32<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Checks if the StrongInt32 has a minimum value.
    /// </summary>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, int min)
        where TStrong : StrongInt32<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min);

    /// <summary>
    /// Checks if the StrongInt32 has a maximum value.
    /// </summary>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximum<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, int max)
        where TStrong : StrongInt32<TStrong>
        => rule.Must(strong => strong is not null && strong.Value <= max);

    /// <summary>
    /// Checks if the StrongInt32 is within a specified range.
    /// </summary>
    public static IRuleBuilderOptions<T, TStrong?> HasRange<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, int min, int max)
        where TStrong : StrongInt32<TStrong>
        => rule.Must(strong => strong is not null && strong.Value >= min && strong.Value <= max);

    /// <summary>
    /// Validates that the strong Int32 is equal to another strong Int32.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int32.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong Int32.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongInt32<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new EqualValidator<T, TStrong>(func, member, name)!);
    }

    /// <summary>
    /// Validates that the strong Int32 is not equal to another strong Int32.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Int32.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong Int32.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongInt32<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new NotEqualValidator<T, TStrong>(func, member, name)!);
    }
}
