// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Provides validation rules for StrongBoolean.
/// </summary>
public static class StrongBooleanValidators
{
    /// <summary>
    /// Checks if the StrongBoolean has a value (is not null).
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasValue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Checks if the StrongBoolean is true.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsTrue<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>
        => rule.Must(strong => strong is not null && strong.Value);

    /// <summary>
    /// Checks if the StrongBoolean is false.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <typeparam name="TStrong">The type of the strong Boolean.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsFalse<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongBoolean<TStrong>
        => rule.Must(strong => strong is not null && !strong.Value);
}
