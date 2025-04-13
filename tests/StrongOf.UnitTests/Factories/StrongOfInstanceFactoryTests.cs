// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Linq.Expressions;
using System.Reflection;
using StrongOf.Factories;
using Xunit;

namespace StrongOf.UnitTests.Factories;

public class StrongOfInstanceFactoryTests
{
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value) { }

    [Fact]
    public void CreateWithOneParameterExpression_SuccessfullyCreatesLambdaExpression()
    {
        // Arrange
        Type expectedStrong = typeof(TestInt32Of);
        Type expectedType = typeof(int);
        ConstructorInfo? expectedCtor = expectedStrong.GetConstructor([expectedType]);

        ArgumentNullException.ThrowIfNull(expectedCtor);

        // Act
        LambdaExpression lambdaExpression = StrongOfInstanceFactory.CreateWithOneParameterExpression<TestInt32Of, int>();

        // Assert
        Assert.NotNull(lambdaExpression);

        // Assert type
        Assert.IsAssignableFrom<LambdaExpression>(lambdaExpression);
        Assert.IsAssignableFrom<Expression<Func<int, TestInt32Of>>>(lambdaExpression);

        // Assert instance
        Func<int, TestInt32Of> func = (Func<int, TestInt32Of>)lambdaExpression.Compile();
        TestInt32Of newInstance = func.Invoke(10);

        Assert.NotNull(newInstance);
        Assert.IsType<TestInt32Of>(newInstance);

        // Assert params
        Expression? ctorParameter = ((NewExpression)lambdaExpression.Body).Arguments.First();
        Assert.Equal(expectedType, ctorParameter.Type);

        ConstructorInfo? ctor = ((NewExpression)lambdaExpression.Body).Constructor;
        Assert.Equal(expectedCtor, ctor);
    }
}
