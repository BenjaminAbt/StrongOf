// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Gets the length of the underlying string value.
    /// </summary>
    /// <example>
    /// <code>
    /// var email = new Email("user@example.com");
    /// int length = email.Length; // 16
    /// </code>
    /// </example>
    public int Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        get => Value.Length;
    }
}
