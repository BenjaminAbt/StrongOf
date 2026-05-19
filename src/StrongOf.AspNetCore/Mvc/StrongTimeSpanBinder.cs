// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Binds incoming MVC values to <see cref="StrongTimeSpan{TStrong}"/> instances.
/// </summary>
/// <typeparam name="TStrong">Concrete StrongOf time-span type to materialize.</typeparam>
public class StrongTimeSpanBinder<TStrong> : StrongOfBinder
    where TStrong : StrongTimeSpan<TStrong>, IStrongOf<TimeSpan, TStrong>
{
    /// <summary>
    /// Attempts to parse the raw request value as the configured time-span StrongOf type.
    /// </summary>
    /// <param name="value">Raw non-empty value from route, query or form binding.</param>
    /// <param name="result">Model binding result populated with success or failure.</param>
    /// <returns><see langword="true"/> when parsing succeeded; otherwise <see langword="false"/>.</returns>
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
