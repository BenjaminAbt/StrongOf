﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore;

/// <summary>
/// Represents a binder for StrongGuid type.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongGuid.</typeparam>
public class StrongGuidBinder<TStrong> : StrongOfBinder
    where TStrong : StrongGuid<TStrong>
{
    /// <summary>
    /// Tries to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongGuid<TStrong>.TryParse(value, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
