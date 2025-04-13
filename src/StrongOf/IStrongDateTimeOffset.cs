// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

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

    /// <summary>
    /// Returns the value of the strong type as a DateTimeOffset.
    /// </summary>
    DateTimeOffset AsDateTimeOffset();

    /// <summary>
    /// Returns the value of the strong type as a DateTime.
    /// </summary>
    DateTime AsDateTime();

    /// <summary>
    /// Returns the value of the strong type as a DateTime UTC.
    /// </summary>
    DateTime AsDateTimeUtc();

    /// <summary>
    /// Returns the value of the strong type as a DateOnly.
    /// </summary>
    DateOnly AsDate();

    /// <summary>
    /// Returns the value of the strong type as a TimeOnly.
    /// </summary>
    TimeOnly AsTime();
}
