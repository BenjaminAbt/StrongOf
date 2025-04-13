// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Interface for strong type of Decimal.
/// </summary>
public interface IStrongDecimal : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    decimal Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a Decimal.
    /// </summary>
    decimal AsDecimal();
}
