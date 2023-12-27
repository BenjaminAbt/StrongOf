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
    /// Checks if the given strong string is null or empty.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="strong">The strong string to check.</param>
    /// <returns>true if the strong string is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty<TStrong>(TStrong? strong)
        where TStrong : StrongString<TStrong>
    {
        return strong is null || strong.IsEmpty();
    }

    /// <summary>
    /// Checks if the given strong string is not null or empty.
    /// </summary>
    /// <typeparam name="TStrong">The type of the strong string.</typeparam>
    /// <param name="strong">The strong string to check.</param>
    /// <returns>true if the strong string is not null or empty; otherwise, false.</returns>
    public static bool IsNotNullOrEmpty<TStrong>(TStrong? strong)
       where TStrong : StrongString<TStrong>
    {
        return strong is not null && strong.IsEmpty() is false;
    }
}
