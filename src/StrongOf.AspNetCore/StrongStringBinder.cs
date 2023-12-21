using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore;

/// <summary>
/// Represents a binder for StrongString type.
/// </summary>
/// <typeparam name="TStrong">The type of the StrongString.</typeparam>
public class StrongStringBinder<TStrong> : StrongOfBinder
    where TStrong : StrongString<TStrong>
{
    /// <summary>
    /// Tries to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public override bool TryHandle(string value, out ModelBindingResult result)
    {
        if (value is not null)
        {
            result = ModelBindingResult.Success(StrongString<TStrong>.FromTrimmed(value));
            return true;
        }

        result = ModelBindingResult.Failed();
        return false;
    }
}
