namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Int64.
/// </summary>
public interface IStrongInt64 : IStrongOf
{
    /// <summary>
    /// Gets the value of the Int64.
    /// </summary>
    /// <value>
    /// The value of the Int64.
    /// </value>
    long Value { get; }
}
