// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Base marker interface for all strong types.
/// </summary>
public interface IStrongOf
{
    /// <summary>
    /// Returns a string that represents the value of the underlying type. This method is intended to be overridden by any class implementing the IStrongOf interface.
    /// </summary>
    /// <returns>A string that represents the underlying type.</returns>
    string ToString();
}

/// <summary>
/// Generic interface for strong types that enables compile-time safe factory methods
/// using C# 11+ static abstract members.
/// </summary>
/// <typeparam name="TTarget">The underlying primitive type being wrapped.</typeparam>
/// <typeparam name="TSelf">The concrete strong type (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// This interface allows generic code to create instances of strong types without
/// reflection or cached delegates, providing a type-safe factory pattern.
/// </para>
/// <para>
/// <b>Usage in generic code:</b>
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public static TSelf CreateDefault&lt;TTarget, TSelf&gt;(TTarget value)
///     where TSelf : IStrongOf&lt;TTarget, TSelf&gt;
/// {
///     return TSelf.Create(value);
/// }
///
/// // Usage
/// UserId userId = CreateDefault&lt;Guid, UserId&gt;(Guid.NewGuid());
/// </code>
/// </example>
public interface IStrongOf<TTarget, TSelf> : IStrongOf
    where TSelf : IStrongOf<TTarget, TSelf>
{
    /// <summary>
    /// Gets the underlying primitive value.
    /// </summary>
    TTarget Value { get; }

    /// <summary>
    /// Creates a new instance of the strong type from the specified underlying value.
    /// </summary>
    /// <param name="value">The underlying value to wrap.</param>
    /// <returns>A new instance of <typeparamref name="TSelf"/>.</returns>
    static abstract TSelf Create(TTarget value);
}
