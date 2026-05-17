// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides extension methods for FluentValidation's IRuleBuilder for strong string validation.
/// </summary>
/// <remarks>
/// These helpers intentionally validate on <see cref="StrongString{TStrong}.Value"/> to keep
/// rule definitions concise while preserving StrongOf semantics.
/// </remarks>
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
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
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
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
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
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
        => rule.Must(content => content?.Value is null || content.Value.Length <= maxLength);

    /// <summary>
    /// Validates that the strong string matches a regular expression.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="regex">The regular expression.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsRegexMatch<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, Regex regex)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
        // Delegate regex timeout and culture behavior to the caller-provided Regex instance.
        => rule.Must(content => content?.Value is not null && regex.IsMatch(content.Value));

    /// <summary>
    /// Validates that the strong string is equal to another strong string specified by an accessor.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong string to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Password).IsEqualTo(x => x.PasswordConfirm, nameof(Model.PasswordConfirm));
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Reuse FluentValidation's built-in EqualValidator so placeholders/message templates
        // behave consistently with native RuleFor(...).Equal(...).
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the strong string is not equal to another strong string specified by an accessor.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong string to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.NewPassword).IsNotEqualTo(x => x.OldPassword, nameof(Model.OldPassword));
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo semantics with FluentValidation's native NotEqualValidator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Specifies that the value being validated must only contain characters from the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of the parent object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong string being validated.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="chars">The collection of allowed characters.</param>
    /// <param name="messagePattern">The pattern used to format the error message if validation fails.</param>
    /// <param name="formatProvider">An optional <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>The rule builder options.</returns>
    /// <remarks>
    /// This method adds a custom validation rule to the rule builder that checks if the value being validated contains only characters from the specified collection.
    /// If the validation fails, an error message is added to the validation context using the provided message pattern.
    /// </remarks>
    public static IRuleBuilderOptionsConditions<T, TStrong?> AllowedChars<T, TStrong>(this IRuleBuilder<T, TStrong?> rule, ICollection<char> chars, string messagePattern, IFormatProvider? formatProvider = null)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        return rule.Custom((topic, context) =>
        {
            if (topic is TStrong strong && strong.IsEmpty() is false)
            {
                // Use Custom(...) instead of Must(...) so we can emit a detailed message
                // containing the actual invalid character set.
                if (strong.ContainsInvalidChars(chars, out ICollection<char>? invalidChars))
                {
                    context.AddFailure(string.Format(formatProvider, messagePattern, string.Concat(invalidChars)));
                }
            }
        });
    }
}
