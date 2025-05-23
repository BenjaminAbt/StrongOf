﻿// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

namespace StrongOf;

/// <summary>
/// Defines a contract for a strong type of string.
/// This interface is used to create types that have a specific purpose and are not just a string.
/// </summary>
public interface IStrongString : IStrongOf
{
    /// <summary>
    /// Gets the value of the strong type of string.
    /// This property is used to access the underlying string value of the strong type.
    /// </summary>
    string Value { get; }

    /// <summary>
    /// Returns the value of the strong type as a string.
    /// </summary>
    string AsString();
}
