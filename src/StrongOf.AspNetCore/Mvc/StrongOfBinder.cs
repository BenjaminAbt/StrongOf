// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrongOf.AspNetCore.Mvc;

/// <summary>
/// Abstract class for StrongOfBinder which implements IModelBinder interface.
/// </summary>
public abstract class StrongOfBinder : IModelBinder
{
    /// <summary>
    /// Abstract method to handle the model binding result.
    /// </summary>
    /// <param name="value">The value to be handled.</param>
    /// <param name="result">The result of the model binding.</param>
    /// <returns>Returns a boolean indicating the success of the operation.</returns>
    public abstract bool TryHandle(string value, out ModelBindingResult result);

    /// <summary>
    /// Asynchronously binds the model for a given context.
    /// </summary>
    /// <param name="bindingContext">The context within which to bind the model.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        string modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
        string? value = valueProviderResult.FirstValue;

        // Check if the argument value is null or empty
        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        // in both cases we get a result
        TryHandle(value, out ModelBindingResult result);
        bindingContext.Result = result;

        return Task.CompletedTask;
    }
}
