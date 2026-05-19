// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Binds incoming MVC values to <see cref="StrongDateTimeOffset{TStrong}"/> instances.
/// </summary>
/// <typeparam name="TStrong">Concrete StrongOf date-time-offset type to materialize.</typeparam>
public class StrongDateTimeOffsetBinder<TStrong> : StrongOfBinder
    where TStrong : StrongDateTimeOffset<TStrong>, IStrongOf<DateTimeOffset, TStrong>
{
    /// <summary>
    /// Attempts to parse the raw request value as an ISO 8601 date-time-offset value.
    /// </summary>
    /// <param name="value">Raw non-empty value from route, query or form binding.</param>
    /// <param name="result">Model binding result populated with success or failure.</param>
    /// <returns><see langword="true"/> when parsing succeeded; otherwise <see langword="false"/>.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        // ISO 8601 preserves explicit UTC offsets and avoids culture-specific parsing behavior.
        if (StrongDateTimeOffset<TStrong>.TryParseIso8601(value, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
