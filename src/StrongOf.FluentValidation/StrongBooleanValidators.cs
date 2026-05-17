// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides FluentValidation rules for <see cref="StrongBoolean{TStrong}"/> values.
/// </summary>
public static class StrongBooleanValidators
{
    /// <summary>
    /// Checks whether the strong boolean has a value (is not <see langword="null"/>).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>, IStrongOf<bool, TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong boolean evaluates to <see langword="true"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsTrue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>, IStrongOf<bool, TStrong>
        => rule.Must(strong => strong is not null && strong.Value);

    /// <summary>
    /// Validates that the strong boolean evaluates to <see langword="false"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsFalse<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>, IStrongOf<bool, TStrong>
        => rule.Must(strong => strong is not null && !strong.Value);
}
