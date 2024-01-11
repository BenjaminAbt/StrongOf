namespace StrongOf;

/// <summary>
/// Provides extension methods for StrongGuids.
/// </summary>
public static class StrongGuidExtensions
{
    /// <summary>
    /// Returns the value of the strong type as a Guid.
    /// </summary>
    /// <param name="strong">The strong type to convert.</param>
    /// <returns>The Guid value of the strong type.</returns>
    public static Guid AsGuid(this IStrongGuid strong)
        => strong.Value;

    /// <summary>
    /// Checks if the Guid is empty.
    /// </summary>
    /// <param name="strong">The strong type to check.</param>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    public static bool IsEmpty(this IStrongGuid strong)
        => strong.Value == Guid.Empty;

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="strong">The strong type to convert.</param>
    /// <param name="format">A standard or custom format string.</param>
    /// <returns>The string representation of the value of this instance as specified by format.</returns>
    public static string ToString(this IStrongGuid strong, string format)
        => strong.Value.ToString(format);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation with dashes.
    /// </summary>
    /// <param name="strong">The strong type to convert.</param>
    /// <returns>The string representation of the value of this instance with dashes.</returns>
    public static string ToStringWithDashes(this IStrongGuid strong)
        => strong.Value.ToString("D");

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation without dashes.
    /// </summary>
    /// <param name="strong">The strong type to convert.</param>
    /// <returns>The string representation of the value of this instance without dashes.</returns>
    public static string ToStringWithoutDashes(this IStrongGuid strong)
        => strong.Value.ToString("N");
}
