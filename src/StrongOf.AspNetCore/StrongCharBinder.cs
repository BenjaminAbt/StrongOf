// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore;

/// <summary>
/// Represents a binder for StrongChar type.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongChar.</typeparam>
public class StrongCharBinder<TStrong> : StrongOfBinder
    where TStrong : StrongChar<TStrong>
{
    /// <summary>
    /// Tries to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongChar<TStrong>.TryParse(value, null, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
