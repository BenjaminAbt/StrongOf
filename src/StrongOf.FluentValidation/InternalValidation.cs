
using System.Linq.Expressions;
using System.Reflection;
using FluentValidation;

namespace StrongOf.FluentValidation;

internal static class InternalValidation
{
    internal static string GetDisplayName<T, TProperty>(MemberInfo member, Expression<Func<T, TProperty>> expression)
       => ValidatorOptions.Global.DisplayNameResolver(typeof(T), member, expression) ?? member?.Name!;
}
