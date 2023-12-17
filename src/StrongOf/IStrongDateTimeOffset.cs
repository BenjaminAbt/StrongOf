namespace StrongOf;

/// <summary>
/// Interface for strong type of DateTimeOffset.
/// </summary>
public interface IStrongDateTimeOffset : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    DateTimeOffset Value { get; }
}
