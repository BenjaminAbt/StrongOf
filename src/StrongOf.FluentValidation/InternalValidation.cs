// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace StrongOf.FluentValidation;

internal static class InternalValidation
{
    internal static string GetDisplayName<T, TProperty>(MemberInfo member, Expression<Func<T, TProperty>> expression)
       => ValidatorOptions.Global.DisplayNameResolver(typeof(T), member, expression) ?? member?.Name!;

    internal static Func<T, TProperty> CreateAccessor<T, TProperty>(MemberInfo member)
        => instance => member switch
        {
            PropertyInfo property => (TProperty?)property.GetValue(instance)!,
            FieldInfo field => (TProperty?)field.GetValue(instance)!,
            _ => throw new NotSupportedException($"Member type '{member.MemberType}' is not supported.")
        };
}
