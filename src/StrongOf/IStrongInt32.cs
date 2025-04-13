// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Int32.
/// </summary>
public interface IStrongInt32 : IStrongOf
{
    /// <summary>
    /// Gets the value of the Int32.
    /// </summary>
    /// <value>
    /// The value of the Int32.
    /// </value>
    int Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a int.
    /// </summary>
    int AsInt();

    /// <summary>
    /// Returns the value of the strong type as a int.
    /// </summary>
    int AsInt32();
}
