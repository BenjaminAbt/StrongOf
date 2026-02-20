// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="Guid"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple Guid identifiers.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific identifiers like <c>UserId</c>, <c>OrderId</c>, <c>TenantId</c>, etc.
/// The compiler will prevent accidental mixing of different identifier types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation. The <see cref="Empty"/> method returns a cached instance.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed identifiers
/// public sealed class UserId(Guid value) : StrongGuid&lt;UserId&gt;(value) { }
/// public sealed class TenantId(Guid value) : StrongGuid&lt;TenantId&gt;(value) { }
///
/// // Usage - compiler prevents mixing up identifiers
/// public User GetUser(UserId userId, TenantId tenantId)
/// {
///     // Cannot accidentally swap userId and tenantId!
/// }
///
/// // Create instances
/// var userId = new UserId(Guid.NewGuid());           // Fastest
/// var userId = UserId.From(Guid.NewGuid());          // For generic scenarios
/// var userId = UserId.New();                         // Generate new Guid
/// var empty = UserId.Empty();                        // Cached empty instance
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="Guid"/> value.</param>
public abstract partial class StrongGuid<TStrong>(Guid Value)
        : StrongOf<Guid, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongGuid
    where TStrong : StrongGuid<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="Guid"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="Guid"/> value.</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.NewGuid());
    /// Guid rawGuid = userId.AsGuid();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Guid AsGuid() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="Guid"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> has a value;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// Guid? nullableGuid = GetOptionalGuid();
    /// UserId? userId = UserId.FromGuid(nullableGuid);
    ///
    /// // Or with a value
    /// UserId? userId = UserId.FromGuid(Guid.NewGuid()); // Returns UserId
    /// UserId? empty = UserId.FromGuid(null);            // Returns null
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromGuid(Guid? value)
    {
        if (value.HasValue)
        {
            return From(value.Value);
        }

        return null;
    }

    /// <summary>
    /// Creates a strong type instance by parsing a string representation of a <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">The string to parse as a <see cref="Guid"/>.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if parsing succeeds;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="Guid.TryParse(string?, out Guid)"/> internally and accepts
    /// all standard Guid formats (with or without dashes, braces, etc.).
    /// </remarks>
    /// <example>
    /// <code>
    /// UserId? userId = UserId.FromString("550e8400-e29b-41d4-a716-446655440000");
    /// UserId? fromNoDashes = UserId.FromString("550e8400e29b41d4a716446655440000");
    /// UserId? invalid = UserId.FromString("not-a-guid"); // Returns null
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromString(string? value)
    {
        if (value is not null && Guid.TryParse(value, out Guid result))
        {
            return From(result);
        }

        return null;
    }

    /// <summary>
    /// Compares the current instance with another object and returns an integer indicating
    /// their relative position in the sort order.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    /// A negative value if this instance precedes <paramref name="other"/>;
    /// zero if they are equal;
    /// a positive value if this instance follows <paramref name="other"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="other"/> is not of type <typeparamref name="TStrong"/>.
    /// </exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int CompareTo(object? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (other is TStrong otherStrong)
        {
            return Value.CompareTo(otherStrong.Value);
        }

        throw new ArgumentException($"Object is not a {typeof(TStrong)}", nameof(other));
    }

    /// <summary>
    /// Compares the current instance with another strong type of the same kind.
    /// </summary>
    /// <param name="other">The strong type to compare with this instance.</param>
    /// <returns>
    /// A negative value if this instance precedes <paramref name="other"/>;
    /// zero if they are equal;
    /// a positive value if this instance follows <paramref name="other"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int CompareTo(TStrong? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// Determines whether the specified strong type instance is equal to the current instance.
    /// </summary>
    /// <param name="other">The strong type to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified instance is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// var id1 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// var id2 = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// bool areEqual = id1.Equals(id2); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(TStrong? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value.Equals(other.Value);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override bool Equals(object? obj)
        => obj is TStrong other && Equals(other);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public override int GetHashCode()
        => Value.GetHashCode();

    /// <summary>
    /// Cached empty instance for performance. Created once per concrete type.
    /// </summary>
    private static readonly TStrong s_empty = From(Guid.Empty);

    /// <summary>
    /// Returns a cached instance representing an empty <see cref="Guid"/> (<see cref="Guid.Empty"/>).
    /// </summary>
    /// <returns>A cached instance with <see cref="Guid.Empty"/> as the underlying value.</returns>
    /// <remarks>
    /// This method returns a cached instance, avoiding allocation on repeated calls.
    /// Use this instead of <c>new TStrong(Guid.Empty)</c> for better performance.
    /// </remarks>
    /// <example>
    /// <code>
    /// UserId emptyId = UserId.Empty();
    /// bool isEmpty = emptyId.IsEmpty(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong Empty()
        => s_empty;

    /// <summary>
    /// Determines whether the underlying <see cref="Guid"/> is <see cref="Guid.Empty"/>.
    /// </summary>
    /// <returns><c>true</c> if the underlying value is <see cref="Guid.Empty"/>; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// var userId = UserId.Empty();
    /// bool isEmpty = userId.IsEmpty(); // true
    ///
    /// var newId = UserId.New();
    /// bool isNewEmpty = newId.IsEmpty(); // false
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsEmpty()
        => Value == Guid.Empty;

    /// <summary>
    /// Creates a new instance with a newly generated <see cref="Guid"/>.
    /// </summary>
    /// <returns>A new instance of <typeparamref name="TStrong"/> with a unique <see cref="Guid"/>.</returns>
    /// <example>
    /// <code>
    /// UserId newUserId = UserId.New();
    /// TenantId newTenantId = TenantId.New();
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong New()
         => From(Guid.NewGuid());

    /// <summary>
    /// Tries to parse a <see cref="Guid"/> from a character span and creates a strong type instance.
    /// </summary>
    /// <param name="content">The character span containing the <see cref="Guid"/> to parse.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if successful;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method is allocation-free for the parsing operation itself and accepts
    /// all standard Guid formats.
    /// </remarks>
    /// <example>
    /// <code>
    /// if (UserId.TryParse("550e8400-e29b-41d4-a716-446655440000", out UserId? userId))
    /// {
    ///     Console.WriteLine($"Parsed: {userId}");
    /// }
    ///
    /// // Works with spans
    /// ReadOnlySpan&lt;char&gt; span = "550e8400-e29b-41d4-a716-446655440000".AsSpan();
    /// if (UserId.TryParse(span, out UserId? id))
    /// {
    ///     // Use id
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(ReadOnlySpan<char> content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (Guid.TryParse(content, out Guid value))
        {
            strong = From(value);
            return true;
        }

        strong = null;
        return false;
    }

    /// <summary>
    /// Returns the string representation of the underlying <see cref="Guid"/> using the specified format.
    /// </summary>
    /// <param name="format">
    /// A single format specifier:
    /// <list type="bullet">
    /// <item><description><c>"N"</c> - 32 digits without dashes</description></item>
    /// <item><description><c>"D"</c> - 32 digits with dashes (default)</description></item>
    /// <item><description><c>"B"</c> - 32 digits with dashes, enclosed in braces</description></item>
    /// <item><description><c>"P"</c> - 32 digits with dashes, enclosed in parentheses</description></item>
    /// <item><description><c>"X"</c> - Four hexadecimal values in braces</description></item>
    /// </list>
    /// </param>
    /// <returns>The formatted string representation.</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    ///
    /// userId.ToString("N"); // "550e8400e29b41d4a716446655440000"
    /// userId.ToString("D"); // "550e8400-e29b-41d4-a716-446655440000"
    /// userId.ToString("B"); // "{550e8400-e29b-41d4-a716-446655440000}"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToString(string format)
        => Value.ToString(format);

    /// <summary>
    /// Returns the string representation of the underlying <see cref="Guid"/> with dashes (format "D").
    /// </summary>
    /// <returns>The <see cref="Guid"/> formatted as "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx".</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.Parse("550e8400e29b41d4a716446655440000"));
    /// string withDashes = userId.ToStringWithDashes(); // "550e8400-e29b-41d4-a716-446655440000"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToStringWithDashes()
        => Value.ToString("D");

    /// <summary>
    /// Returns the string representation of the underlying <see cref="Guid"/> without dashes (format "N").
    /// </summary>
    /// <returns>The <see cref="Guid"/> formatted as "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx".</returns>
    /// <example>
    /// <code>
    /// var userId = new UserId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    /// string noDashes = userId.ToStringWithoutDashes(); // "550e8400e29b41d4a716446655440000"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string ToStringWithoutDashes()
        => Value.ToString("N");
}
