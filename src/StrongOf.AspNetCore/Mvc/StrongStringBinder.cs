// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Binds incoming MVC values to <see cref="StrongString{TStrong}"/> instances.
/// </summary>
/// <typeparam name="TStrong">Concrete StrongOf string type to materialize.</typeparam>
public class StrongStringBinder<TStrong> : StrongOfBinder
    where TStrong : StrongString<TStrong>, IStrongOf<string, TStrong>
{
    /// <summary>
    /// Converts the raw request value into the configured string StrongOf type.
    /// </summary>
    /// <param name="value">Raw non-empty value from route, query or form binding.</param>
    /// <param name="result">Model binding result populated with success or failure.</param>
    /// <returns><see langword="true"/> when conversion succeeded; otherwise <see langword="false"/>.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (value is not null)
        {
            // Trim here to normalize typical HTTP input artifacts (for example accidental
            // whitespace in query strings) before creating the strong string type.
            result = ModelBindingResult.Success(StrongString<TStrong>.FromTrimmed(value));
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
