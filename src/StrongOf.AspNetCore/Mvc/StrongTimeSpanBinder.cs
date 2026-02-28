// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Represents a binder for <see cref="StrongTimeSpan{TStrong}"/> types.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongTimeSpan.</typeparam>
public class StrongTimeSpanBinder<TStrong> : StrongOfBinder
    where TStrong : StrongTimeSpan<TStrong>
{
    /// <summary>
    /// Tries to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongTimeSpan<TStrong>.TryParse(value, null, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
