// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Base MVC model binder for StrongOf value objects.
/// </summary>
/// <remarks>
/// This type handles the ASP.NET Core binding pipeline concerns (value-provider access,
/// model-state population and early exits) while concrete binders only implement parsing.
/// Keeping parsing in derived classes ensures each primitive type can enforce its own
/// semantics without duplicating MVC boilerplate.
/// </remarks>
public abstract class StrongOfBinder : IModelBinder
{
    /// <summary>
    /// Tries to parse a raw incoming string into a strongly typed model value.
    /// </summary>
    /// <param name="value">The non-empty raw value resolved from the active value provider.</param>
    /// <param name="result">The model-binding result to assign when handling is complete.</param>
    /// <returns><see langword="true"/> when parsing succeeded; otherwise <see langword="false"/>.</returns>
    public abstract bool TryHandle(string value, out ModelBindingResult result);

    /// <summary>
    /// Binds the model from the configured MVC value providers.
    /// </summary>
    /// <param name="bindingContext">The active model-binding context.</param>
    /// <returns>A completed task because binding is intentionally synchronous.</returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        string modelName = bindingContext.ModelName;

        // No value means "binder not applicable"; we let the remaining MVC pipeline continue.
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        // Always store the attempted raw value so model-state error messages and diagnostics
        // can report what the client sent even when parsing fails later.
        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        string? value = valueProviderResult.FirstValue;

        // Empty input is treated as "no meaningful value" to align with standard MVC
        // binder behavior and avoid forcing StrongOf parsing for blank fields.
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        // Both success and failure paths provide a concrete ModelBindingResult.
        TryHandle(value, out ModelBindingResult result);
        bindingContext.Result = result;

        return Task.CompletedTask;
    }
}
