// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of Guid.
/// </summary>
public interface IStrongGuid : IStrongOf
{
    /// <summary>
    /// Gets the value of the Guid.
    /// </summary>
    Guid Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a Guid.
    /// </summary>
    Guid AsGuid();

    /// <summary>
    /// Checks if the Guid is empty.
    /// </summary>
    /// <returns>True if the Guid is empty; otherwise, false.</returns>
    bool IsEmpty();

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation using the specified format.
    /// </summary>
    /// <param name="format">A standard or custom format string.</param>
    /// <returns>The string representation of the value of this instance as specified by format.</returns>
    string ToString(string format);

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation with dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance with dashes.</returns>
    string ToStringWithDashes();

    /// <summary>
    /// Converts the value of this instance to its equivalent string representation without dashes.
    /// </summary>
    /// <returns>The string representation of the value of this instance without dashes.</returns>
    string ToStringWithoutDashes();
}
