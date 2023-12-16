namespace StrongOf;

/// <summary>
/// Interface for strong type of DateTime.
/// </summary>
public interface IStrongDateTime : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    DateTime Value { get; }
}
