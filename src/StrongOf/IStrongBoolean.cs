// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Boolean.
/// </summary>
public interface IStrongBoolean : IStrongOf
{
    /// <summary>
    /// Gets the value of the Boolean.
    /// </summary>
    bool Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a bool.
    /// </summary>
    bool AsBool();
}
