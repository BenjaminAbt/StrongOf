namespace StrongOf;

/// <summary>
/// Interface for strong type of char.
/// </summary>
public interface IStrongChar : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    char Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a char.
    /// </summary>
    char AsChar();
}
