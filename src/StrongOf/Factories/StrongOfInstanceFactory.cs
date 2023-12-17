using System.Linq.Expressions;
using System.Reflection;

namespace StrongOf.Factories;

/// <summary>
/// A factory class for creating Lambda Expressions.
/// </summary>
internal static class StrongOfInstanceFactory
{
    /// <summary>
    /// Creates a LambdaExpression with one parameter.
    /// </summary>
    /// <typeparam name="TStrong">The type of the object to be created.</typeparam>
    /// <typeparam name="TTarget">The type of the parameter for the constructor.</typeparam>
    /// <returns>A LambdaExpression that represents the creation of an object of type TStrong with a constructor parameter of type TTarget.</returns>
    public static LambdaExpression CreateWithOneParameterExpression<TStrong, TTarget>()
    {
        Type ctorParameterType = typeof(TTarget);
        ParameterExpression ctorParameter = Expression.Parameter(ctorParameterType);

        ConstructorInfo ctor = typeof(TStrong)
            .GetConstructor([ctorParameterType]) ?? throw new InvalidOperationException($"No constructor found for type {typeof(TStrong)} with parameter type {ctorParameterType}.");

        NewExpression newExp = Expression.New(ctor, ctorParameter);

        LambdaExpression lambda = Expression.Lambda<Func<TTarget, TStrong>>(newExp, ctorParameter);

        return lambda;
    }

    /// <summary>
    /// Creates a delegate with one parameter.
    /// </summary>
    /// <typeparam name="TStrong">The type of the object to be created.</typeparam>
    /// <typeparam name="TTarget">The type of the parameter for the constructor.</typeparam>
    /// <returns>A Func delegate that represents the creation of an object of type TStrong with a constructor parameter of type TTarget.</returns>
    public static Func<TTarget, TStrong> CreateWithOneParameterDelegate<TStrong, TTarget>()
    {
        return (Func<TTarget, TStrong>)CreateWithOneParameterExpression<TStrong, TTarget>().Compile();
    }
}
