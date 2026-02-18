// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Network;

/// <summary>
/// Represents a strongly-typed HTTP method.
/// </summary>
[DebuggerDisplay("{Value}")]
[TypeConverter(typeof(HttpMethodTypeConverter))]
public sealed class HttpMethod(string value) : StrongString<HttpMethod>(value)
{
    private static readonly HashSet<string> s_standardMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS", "TRACE", "CONNECT"
    };

    /// <summary>
    /// Determines whether the HTTP method is a standard method.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsStandard()
        => !string.IsNullOrWhiteSpace(Value) && s_standardMethods.Contains(Value);

    /// <summary>
    /// Returns the method in uppercase.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToUpperCase()
        => Value.ToUpperInvariant();
}

/// <summary>
/// Type converter for <see cref="HttpMethod"/>.
/// </summary>
public sealed class HttpMethodTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string stringValue ? new HttpMethod(stringValue) : base.ConvertFrom(context, culture, value);
}
