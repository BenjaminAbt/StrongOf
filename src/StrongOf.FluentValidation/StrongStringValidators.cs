
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides extension methods for FluentValidation's IRuleBuilder for strong string validation.
/// </summary>
public static class StrongStringValidators
{
    /// <summary>
    /// Validates that the strong string has a value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongString<TStrong>
        => rule.Must(strong => strong?.IsEmpty() is false);

    /// <summary>
    /// Validates that the strong string has a minimum length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="minLength">The minimum length.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMinimumLength<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, int minLength)
        where TStrong : StrongString<TStrong>
        => rule.Must(content => content?.Value is not null && content.Value.Length >= minLength);

    /// <summary>
    /// Validates that the strong string has a maximum length.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasMaximumLength<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, int maxLength)
        where TStrong : StrongString<TStrong>
        => rule.Must(content => content?.Value is null ? true : content.Value.Length <= maxLength);

    /// <summary>
    /// Validates that the strong string matches a regular expression.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="regex">The regular expression.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsRegexMatch<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Regex regex)
        where TStrong : StrongString<TStrong>
        => rule.Must(content => content?.Value is not null && regex.IsMatch(content.Value));

    /// <summary>
    /// Validates that the strong string is equal to another strong string.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong string.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongString<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new EqualValidator<T, TStrong>(func, member, name)!);
    }
    /// <summary>
    /// Validates that the strong string is equal to another strong string.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="expression">The expression that specifies the other strong string.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Expression<Func<T, TStrong>> expression)
        where TStrong : StrongString<TStrong>
    {
        MemberInfo member = expression.GetMember();
        Func<T, TStrong> func = AccessorCache<T>.GetCachedAccessor(member, expression);
        string name = InternalValidation.GetDisplayName(member, expression);
        return rule.SetValidator(new NotEqualValidator<T, TStrong>(func, member, name)!);
    }

    /// <summary>
    /// Validates that the strong string only contains allowed characters.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="chars">The set of allowed characters.</param>
    /// <param name="messagePattern">The message pattern to use if the validation fails.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptionsConditions<T, TStrong?> AllowedChars<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, HashSet<char> chars, string messagePattern)
        where TStrong : StrongString<TStrong>
    {
        return rule.Custom((topic, context) =>
        {
            if (topic is TStrong strong && strong.IsEmpty() is false)
            {
                if (strong.ContainsInvalidChars(chars, out ICollection<char>? invalidChars))
                {
                    context.AddFailure(string.Format(messagePattern, string.Concat(invalidChars)));
                }
            }
        });
    }
}
