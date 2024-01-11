namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Guid.
/// </summary>
public interface IStrongGuid : IStrongOf
{
    /// <summary>
    /// Gets the value of the Guid.
    /// </summary>
    Guid Value { get; }
}
