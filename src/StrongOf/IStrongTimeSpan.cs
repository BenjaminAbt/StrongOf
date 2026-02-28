// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Interface for strong type of <see cref="TimeSpan"/>.
/// </summary>
public interface IStrongTimeSpan : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type.
    /// </summary>
    TimeSpan Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a <see cref="TimeSpan"/>.
    /// </summary>
    TimeSpan AsTimeSpan();
}
