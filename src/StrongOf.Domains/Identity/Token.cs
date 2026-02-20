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
[TypeConverter(typeof(StrongStringTypeConverter<Token>))]
public sealed class Token(string value) : StrongString<Token>(value)
{
    /// <summary>
    /// Determines whether the token has a non-empty value.
    /// </summary>
    /// <returns><see langword="true"/> if the token string is not <see langword="null"/>, empty, or whitespace; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool HasValue()
        => !string.IsNullOrWhiteSpace(Value);
}
