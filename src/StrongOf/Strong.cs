// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Provides utility methods for strong types.
/// </summary>
public static class Strong
{
    /// <summary>
    /// Checks if the given strong type is null.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong object.</typeparam>
    /// <param name="strong">The strong object to check.</param>
    /// <returns>true if the strong object is null; otherwise, false.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool IsNull<TStrong>(TStrong? strong)
        where TStrong : IStrongOf
    {
        return strong is null;
    }

    /// <summary>
    /// Checks if the given strong type is not null.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong object.</typeparam>
    /// <param name="strong">The strong object to check.</param>
    /// <returns>true if the strong object is not null; otherwise, false.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<TStrong>(TStrong? strong)
        where TStrong : IStrongOf
    {
        return strong is not null;
    }

    /// <summary>
    /// Determines whether the specified <typeparamref name="TStrong"/> instance is <c>null</c> or empty.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong string. Must inherit from <see cref="StrongString{TStrong}"/>.</typeparam>
    /// <param name="strong">The <typeparamref name="TStrong"/> instance to check.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> instance is <c>null</c> or empty; otherwise, <c>false</c>.
    /// </returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        return strong?.IsEmpty() is not false;
    }

    /// <summary>
    /// Determines whether the specified <typeparamref name="TStrong"/> instance has a value.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong string. Must inherit from <see cref="StrongString{TStrong}"/>.</typeparam>
    /// <param name="strong">The <typeparamref name="TStrong"/> instance to check.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> instance is not <c>null</c> and is not empty; otherwise, <c>false</c>.
    /// </returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool HasValue<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        return IsNotNullOrEmpty(strong);
    }

    /// <summary>
    /// Determines whether the specified <typeparamref name="TStrong"/> instance is not <c>null</c> and not empty.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong string. Must inherit from <see cref="StrongString{TStrong}"/>.</typeparam>
    /// <param name="strong">The <typeparamref name="TStrong"/> instance to check.</param>
    /// <returns>
    /// <c>true</c> if the <paramref name="strong"/> instance is not <c>null</c> and not empty; otherwise, <c>false</c>.
    /// </returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNullOrEmpty<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
    {
        return strong?.IsEmpty() is false;
    }

    /// <summary>
    /// Determines whether the underlying value of the specified strong type equals the default value
    /// for the underlying primitive (<c>default(TTarget)</c>).
    /// </summary>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="strong">The strong type instance to inspect. May be <c>null</c>.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="strong"/> is <c>null</c> or its <c>Value</c> equals
    /// <c>default(TTarget)</c>; otherwise <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Generalises <see cref="IsNullOrEmpty{TStrong}"/> beyond strings: works for <c>Guid</c>
    /// (compares with <see cref="Guid.Empty"/>), numerics (compares with <c>0</c>),
    /// <see cref="DateTime"/> (compares with <see cref="DateTime.MinValue"/>) and so on.
    /// </remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool IsDefault<TStrong, TTarget>(TStrong? strong)
        where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
    {
        if (strong is null)
        {
            return true;
        }

        return EqualityComparer<TTarget>.Default.Equals(strong.Value, default!);
    }

    /// <summary>
    /// Inverse of <see cref="IsDefault{TStrong,TTarget}"/>: returns <c>true</c> when the strong
    /// type instance is non-null and its underlying value differs from <c>default(TTarget)</c>.
    /// </summary>
    /// <typeparam name="TStrong">The concrete strong type.</typeparam>
    /// <typeparam name="TTarget">The underlying primitive type.</typeparam>
    /// <param name="strong">The strong type instance to inspect.</param>
    /// <returns><c>true</c> if non-null and not the default underlying value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool HasNonDefaultValue<TStrong, TTarget>(TStrong? strong)
        where TStrong : StrongOf<TTarget, TStrong>, IStrongOf<TTarget, TStrong>
        => !IsDefault<TStrong, TTarget>(strong);
}
