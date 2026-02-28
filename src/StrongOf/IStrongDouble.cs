// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Double.
/// </summary>
public interface IStrongDouble : IStrongOf
{
    /// <summary>
    /// Gets the value of the Double.
    /// </summary>
    double Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a double.
    /// </summary>
    double AsDouble();
}
