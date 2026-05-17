// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;
using FluentValidation.Validators;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongGuid{TStrong}"/> values.
/// </summary>
public static class StrongGuidValidators
{
    /// <summary>
    /// Validates that the strong Guid has a non-empty value.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongGuid<TStrong>, IStrongOf<Guid, TStrong>
        => rule.Must(strong => strong is not null && strong.Value != Guid.Empty);

    /// <summary>
    /// Validates that the strong Guid is not empty (<see cref="Guid.Empty"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEmpty<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongGuid<TStrong>, IStrongOf<Guid, TStrong>
        => rule.Must(strong => strong is not null && strong.Value != Guid.Empty);

    /// <summary>
    /// Validates that the strong Guid is equal to another strong Guid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong Guid to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongGuid<TStrong>, IStrongOf<Guid, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Use FluentValidation's built-in EqualValidator to preserve standard
        // cross-property comparison messages.
        return rule.SetValidator(new EqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }

    /// <summary>
    /// Validates that the strong Guid is not equal to another strong Guid.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Guid.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="accessor">A delegate that returns the other strong Guid to compare against.</param>
    /// <param name="memberName">The display name of the compared member, used in error messages.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotEqualTo<T, TStrong>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<T, TStrong?> accessor,
        string memberName)
        where TStrong : StrongGuid<TStrong>, IStrongOf<Guid, TStrong>
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(memberName);
        // Mirror IsEqualTo behavior with FluentValidation's native NotEqualValidator.
        return rule.SetValidator(new NotEqualValidator<T, TStrong?>(accessor, null!, memberName)!);
    }
}
