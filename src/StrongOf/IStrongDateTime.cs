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

    /// <summary>
    /// Returns the value of the strong type as a DateTime.
    /// </summary>
    DateTime AsDateTime();

    /// <summary>
    /// Returns the value of the strong type as a DateTimeOffset.
    /// </summary>
    DateTimeOffset AsDateTimeOffset();

    /// <summary>
    /// Returns the value of the strong type as a DateOnly.
    /// </summary>
    DateOnly AsDate();

    /// <summary>
    /// Returns the value of the strong type as a TimeOnly.
    /// </summary>
    TimeOnly AsTime();
}
