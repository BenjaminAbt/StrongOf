// Copyright © Benjamin Abt 2025. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace StrongOf.Domains.Identity;

/// <summary>
/// Represents a strongly-typed authentication or access token string.
/// </summary>
/// <remarks>
/// <para>
/// Use this type to pass tokens (JWT, API tokens, OAuth access tokens, etc.) between service layers
/// without accidentally mixing them up with other strings.
/// </para>
/// <para>
/// <b>Security note:</b> Never log or store tokens in plaintext. Use this type only for in-memory transfer.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// var token = new Token(jwtString);
/// httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
/// </code>
/// </example>
[DebuggerDisplay("Token[{Value.Length} chars]")]
[TypeConverter(typeof(TokenTypeConverter))]
public sealed class Token(string value) : StrongString<Token>(value)
{
    /// <summary>
    /// Determines whether the token has a non-empty value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => !string.IsNullOrWhiteSpace(Value);
}

/// <summary>
/// Type converter for <see cref="Token"/>.
/// </summary>
public sealed class TokenTypeConverter : TypeConverter
{
    /// <inheritdoc />
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc />
    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
        => value is string s ? new Token(s) : base.ConvertFrom(context, culture, value);
}
