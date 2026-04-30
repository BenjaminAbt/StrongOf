// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Lightweight validation helpers intended for use inside <c>TryCreate</c> /
/// <c>Create</c> factory methods on derived strong types.
/// </summary>
/// <remarks>
/// <para>
/// StrongOf intentionally does not run validation inside the (primary) constructor
/// to keep allocation paths hot and to avoid virtual calls during initialisation.
/// Instead, encode invariants in static factory methods and rely on these helpers
/// to keep the call sites concise and uniform across the codebase.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public sealed class Email(string value) : StrongString&lt;Email&gt;(value)
/// {
///     public static bool TryCreate(string? value, out Email? email)
///         => StrongValidation.TryCreate(value, IsValid, v => new Email(v), out email);
///
///     private static bool IsValid(string value) =&gt; value.Contains('@');
/// }
/// </code>
/// </example>
public static class StrongValidation
{
    /// <summary>
    /// Validates <paramref name="value"/> using <paramref name="predicate"/> and produces a
    /// strong type via <paramref name="factory"/> when valid.
    /// </summary>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <param name="value">The candidate underlying value. May be <c>null</c>.</param>
    /// <param name="predicate">Validation predicate; must return <c>true</c> for valid input.</param>
    /// <param name="factory">Factory invoked when validation succeeds.</param>
    /// <param name="result">The created strong instance, or <c>null</c> when invalid/null.</param>
    /// <returns><c>true</c> when validation succeeded and <paramref name="result"/> is set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCreate<TTarget, TStrong>(
        TTarget? value,
        Func<TTarget, bool> predicate,
        Func<TTarget, TStrong> factory,
        out TStrong? result)
        where TStrong : class, IStrongOf
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(factory);

        if (value is null || !predicate(value))
        {
            result = null;
            return false;
        }

        result = factory(value);
        return true;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentException"/> when <paramref name="value"/> does not satisfy
    /// <paramref name="predicate"/>; otherwise returns <paramref name="value"/> unchanged.
    /// </summary>
    /// <typeparam name="T">The argument type.</typeparam>
    /// <param name="value">The argument value.</param>
    /// <param name="predicate">The invariant predicate.</param>
    /// <param name="message">Optional message used when the invariant fails.</param>
    /// <param name="paramName">Captured argument name (do not pass).</param>
    /// <returns><paramref name="value"/> when valid.</returns>
    /// <exception cref="ArgumentException">Thrown when the predicate returns <c>false</c>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Require<T>(
        T value,
        Func<T, bool> predicate,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (!predicate(value))
        {
            throw new ArgumentException(message ?? "Strong type invariant violated.", paramName);
        }
        return value;
    }
}
