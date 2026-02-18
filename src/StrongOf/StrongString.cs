// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

/// <summary>
/// Represents a strongly-typed wrapper around a <see cref="string"/> value, providing compile-time type safety
/// and preventing parameter order mistakes when working with multiple string values.
/// </summary>
/// <typeparam name="TStrong">The concrete strong type that derives from this class (CRTP pattern).</typeparam>
/// <remarks>
/// <para>
/// Use this class to create domain-specific string types like <c>Email</c>, <c>FirstName</c>, <c>PhoneNumber</c>, etc.
/// The compiler will prevent accidental mixing of different string types.
/// </para>
/// <para>
/// <b>Performance Note:</b> Prefer <c>new()</c> over <see cref="StrongOf{TTarget,TStrong}.From(TTarget)"/>
/// for instantiation. The <see cref="Empty"/> method returns a cached instance.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // Define strongly-typed string types
/// public sealed class Email(string value) : StrongString&lt;Email&gt;(value) { }
/// public sealed class FirstName(string value) : StrongString&lt;FirstName&gt;(value) { }
///
/// // Usage - compiler prevents mixing up parameters
/// public void CreateUser(FirstName firstName, Email email)
/// {
///     // Cannot accidentally swap firstName and email!
/// }
///
/// // Create instances
/// var email = new Email("user@example.com");         // Fastest
/// var email = Email.From("user@example.com");        // For generic scenarios
/// var trimmed = Email.FromTrimmed("  user@example.com  "); // Auto-trim
/// var empty = Email.Empty();                         // Cached empty instance
/// </code>
/// </example>
/// <param name="Value">The underlying <see cref="string"/> value.</param>
public abstract partial class StrongString<TStrong>(string Value)
        : StrongOf<string, TStrong>(Value), IComparable, IComparable<TStrong>, IEquatable<TStrong>, IStrongString
    where TStrong : StrongString<TStrong>
{
    /// <summary>
    /// Gets the underlying <see cref="string"/> value.
    /// </summary>
    /// <returns>The underlying <see cref="string"/> value.</returns>
    /// <example>
    /// <code>
    /// var email = new Email("user@example.com");
    /// string rawString = email.AsString(); // "user@example.com"
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string AsString() => Value;

    /// <summary>
    /// Creates a strong type instance from a nullable <see cref="string"/> value.
    /// </summary>
    /// <param name="value">The nullable <see cref="string"/> value to convert.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> if <paramref name="value"/> is not <c>null</c>;
    /// otherwise, <c>null</c>.
    /// </returns>
    /// <example>
    /// <code>
    /// string? nullableEmail = GetOptionalEmail();
    /// Email? email = Email.FromNullable(nullableEmail);
    ///
    /// // Or with a value
    /// Email? email = Email.FromNullable("user@example.com"); // Returns Email
    /// Email? empty = Email.FromNullable(null);               // Returns null
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong? FromNullable(string? value)
    {
        if (value is not null)
        {
            return From(value);
        }

        return null;
    }

    /// <summary>
    /// Creates a strong type instance from the specified value after trimming leading and trailing whitespace.
    /// </summary>
    /// <param name="value">The value to trim and wrap.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TStrong"/> with the trimmed value,
    /// or an empty instance if the trimmed result is empty.
    /// </returns>
    /// <remarks>
    /// This method is useful for user input validation where leading/trailing whitespace should be removed.
    /// </remarks>
    /// <example>
    /// <code>
    /// Email email = Email.FromTrimmed("  user@example.com  ");
    /// Console.WriteLine(email.Value); // "user@example.com"
    ///
    /// Email empty = Email.FromTrimmed("   ");
    /// Console.WriteLine(empty.IsEmpty()); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TStrong FromTrimmed(string value)
    {
        string trimmed = value.Trim();
        return trimmed.Length == 0 ? Empty() : From(trimmed);
    }

    /// <summary>
    /// Tries to create a strong type instance from the specified string and creates a strong type instance.
    /// </summary>
    /// <param name="content">The string to wrap.</param>
    /// <param name="strong">
    /// When this method returns, contains the parsed strong type if the input is not <c>null</c>;
    /// otherwise, <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if <paramref name="content"/> is not <c>null</c>; otherwise, <c>false</c>.</returns>
    /// <example>
    /// <code>
    /// if (Email.TryParse(input, out Email? email))
    /// {
    ///     Console.WriteLine($"Email: {email}");
    /// }
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static bool TryParse(string? content, [NotNullWhen(true)] out TStrong? strong)
    {
        if (content is not null)
        {
            strong = From(content);
            return true;
        }

        strong = null;
        return false;
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
            return string.CompareOrdinal(Value, otherStrong.Value);
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

        return string.CompareOrdinal(Value, other.Value);
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
    /// var email1 = new Email("user@example.com");
    /// var email2 = new Email("user@example.com");
    /// bool areEqual = email1.Equals(email2); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(TStrong? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Value, other.Value, StringComparison.Ordinal);
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
        => Value.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Cached empty instance for performance. Created once per concrete type.
    /// </summary>
    private static readonly TStrong s_empty = From("");

    /// <summary>
    /// Returns a cached instance representing an empty <see cref="string"/>.
    /// </summary>
    /// <returns>A cached instance with an empty string as the underlying value.</returns>
    /// <remarks>
    /// This method returns a cached instance, avoiding allocation on repeated calls.
    /// Use this instead of <c>new TStrong("")</c> for better performance.
    /// </remarks>
    /// <example>
    /// <code>
    /// Email emptyEmail = Email.Empty();
    /// bool isEmpty = emptyEmail.IsEmpty(); // true
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TStrong Empty()
        => s_empty;

    /// <summary>
    /// Determines whether the underlying <see cref="string"/> is empty.
    /// </summary>
    /// <returns><c>true</c> if the underlying value is an empty string; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method checks for an empty string only (length 0), not whitespace.
    /// Use <see cref="StrongString{TStrong}.IsNullOrWhiteSpace"/> for whitespace checks.
    /// </remarks>
    /// <example>
    /// <code>
    /// var email = Email.Empty();
    /// bool isEmpty = email.IsEmpty(); // true
    ///
    /// var whitespace = new Email("   ");
    /// bool isWhitespaceEmpty = whitespace.IsEmpty(); // false (contains whitespace)
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool IsEmpty()
        => Value.Length == 0;
}
