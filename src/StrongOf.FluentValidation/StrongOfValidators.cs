// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using FluentValidation;

namespace StrongOf.FluentValidation;

/// <summary>
/// Generic FluentValidation extensions that work for any <see cref="StrongOf{TTarget,TStrong}"/>
/// regardless of the underlying primitive type.
/// </summary>
/// <remarks>
/// <para>
/// These helpers complement the per-primitive validators (<c>StrongStringValidators</c>,
/// <c>StrongInt32Validators</c>, ...) by providing universal rules that don't depend on the
/// underlying value type.
/// </para>
/// </remarks>
public static class StrongOfValidators
{
    /// <summary>
    /// Validates that the strong type instance is not <c>null</c>.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options for chaining.</returns>
    public static IRuleBuilderOptions<T, TStrong?> IsNotNull<T, TStrong>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : class, IStrongOf
        => rule.Must(strong => strong is not null);

    /// <summary>
    /// Validates that the strong type instance is non-null and that its underlying value
    /// is not the <c>default(TTarget)</c>.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <returns>The rule builder options for chaining.</returns>
    public static IRuleBuilderOptions<T, TStrong?> HasNonDefaultValue<T, TStrong, TTarget>(this IRuleBuilder<T, TStrong?> rule)
        where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
        => rule.Must(strong => Strong.HasNonDefaultValue<TStrong, TTarget>(strong));

    /// <summary>
    /// Validates that the strong type's underlying value satisfies a custom predicate.
    /// </summary>
    /// <typeparam name="T">The model type being validated.</typeparam>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="rule">The rule builder.</param>
    /// <param name="predicate">Predicate evaluated against the underlying value.</param>
    /// <returns>The rule builder options for chaining.</returns>
    public static IRuleBuilderOptions<T, TStrong?> ValueMust<T, TStrong, TTarget>(
        this IRuleBuilder<T, TStrong?> rule,
        Func<TTarget, bool> predicate)
        where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return rule.Must(strong => strong is not null && predicate(strong.Value));
    }
}
