using System.Linq.Expressions;
using System.Reflection;

namespace StrongOf.Factories;

/// <summary>
/// A factory class for creating Lambda Expressions.
/// </summary>
public static class StrongOfInstanceFactory
{
    /// <summary>
    /// Creates a lambda expression representing the instantiation of a constructor with one parameter.
    /// </summary>
    /// <typeparam name="TStrong">The type of object to be constructed.</typeparam>
    /// <typeparam name="TTarget">The type of the parameter for the constructor.</typeparam>
    /// <returns>A lambda expression representing the constructor invocation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no constructor is found for the specified type with the given parameter type.</exception>
    /// <example>
    /// The following example demonstrates how to use the CreateWithOneParameterExpression method.
    /// <code>
    /// var expression = LambdaExpressionHelper.CreateWithOneParameterExpression&lt;TestClass, int&gt;();
    /// Func&lt;int, TestClass&gt; func = (Func&lt;int, TestClass&gt;)expression.Compile();
    /// TestClass instance = func.Invoke(42);
    /// </code>
    /// </example>
    public static LambdaExpression CreateWithOneParameterExpression<TStrong, TTarget>()
        where TStrong : class, IStrongOf
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
        where TStrong : class, IStrongOf
    {
        return (Func<TTarget, TStrong>)CreateWithOneParameterExpression<TStrong, TTarget>().Compile();
    }
}
