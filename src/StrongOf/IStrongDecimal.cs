namespace StrongOf;

/// <summary>
/// Interface for strong type of Decimal.
/// </summary>
public interface IStrongDecimal : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    Decimal Value { get; }
}
