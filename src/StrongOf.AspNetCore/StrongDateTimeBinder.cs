﻿// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore;

/// <summary>
/// Represents a binder for StrongDateTime type.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongDateTime.</typeparam>
public class StrongDateTimeBinder<TStrong> : StrongOfBinder
    where TStrong : StrongDateTime<TStrong>
{
    /// <summary>
    /// Tries to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongDateTime<TStrong>.TryParseIso8601(value, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
