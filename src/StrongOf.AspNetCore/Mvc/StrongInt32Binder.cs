// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Binds incoming MVC values to <see cref="StrongInt32{TStrong}"/> instances.
/// </summary>
/// <typeparam name="TStrong">Concrete StrongOf 32-bit integer type to materialize.</typeparam>
public class StrongInt32Binder<TStrong> : StrongOfBinder
    where TStrong : StrongInt32<TStrong>, IStrongOf<int, TStrong>
{
    /// <summary>
    /// Attempts to parse the raw request value as the configured 32-bit integer StrongOf type.
    /// </summary>
    /// <param name="value">Raw non-empty value from route, query or form binding.</param>
    /// <param name="result">Model binding result populated with success or failure.</param>
    /// <returns><see langword="true"/> when parsing succeeded; otherwise <see langword="false"/>.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (StrongInt32<TStrong>.TryParse(value, out TStrong? strong))
        {
            result = ModelBindingResult.Success(strong);
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
