// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Denotes a strong type that can validate its own value against a format constraint.
/// </summary>
/// <remarks>
/// Implement this interface on domain types that carry a regex or structural validation rule
/// (e.g. <c>EmailAddress</c>, <c>Sku</c>, <c>Slug</c>).
/// Generic code (validators, middleware, logging) can then depend on <see cref="IValidatable"/>
/// without knowing the concrete type.
/// </remarks>
/// <example>
/// <code>
/// void AssertValid(IValidatable value)
/// {
///     if (!value.IsValidFormat())
///         throw new InvalidOperationException($"Invalid value: {value}");
/// }
///
/// AssertValid(new EmailAddress("bad")); // throws
/// </code>
/// </example>
public interface IValidatable
{
    /// <summary>
    /// Returns <c>true</c> when the wrapped value satisfies the type's format constraints.
    /// </summary>
    bool IsValidFormat();
}
