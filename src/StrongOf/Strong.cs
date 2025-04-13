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
    public static bool IsNullOrEmpty<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>
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
    public static bool HasValue<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>
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
    public static bool IsNotNullOrEmpty<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>
    {
        return strong?.IsEmpty() is false;
    }
}
