// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.SourceGeneration;

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="bool"/>.
/// The StrongOf source generator emits the primary constructor, the base type
/// (<c>StrongBoolean&lt;TStrong&gt;</c>) and the AOT-friendly static
/// <c>Create</c> member.
/// </summary>
/// <example>
/// <code>
/// [StrongBoolean] public partial class IsActive;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongBooleanAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="char"/>.
/// The StrongOf source generator emits the primary constructor, the base type
/// (<c>StrongChar&lt;TStrong&gt;</c>) and the AOT-friendly static
/// <c>Create</c> member.
/// </summary>
/// <example>
/// <code>
/// [StrongChar] public partial class Grade;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongCharAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="DateTime"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongDateTime] public partial class CreatedAt;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongDateTimeAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="DateTimeOffset"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongDateTimeOffset] public partial class OccurredAt;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongDateTimeOffsetAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="decimal"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongDecimal] public partial class Amount;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongDecimalAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="double"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongDouble] public partial class Ratio;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongDoubleAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="Guid"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongGuid] public partial class UserId;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongGuidAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="int"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongInt32] public partial class Quantity;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongInt32Attribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="long"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongInt64] public partial class FileSize;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongInt64Attribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="string"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongString] public partial class Email;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongStringAttribute : Attribute { }

/// <summary>
/// Marks a partial class as a strong type wrapping <see cref="TimeSpan"/>.
/// </summary>
/// <example>
/// <code>
/// [StrongTimeSpan] public partial class Timeout;
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class StrongTimeSpanAttribute : Attribute { }
