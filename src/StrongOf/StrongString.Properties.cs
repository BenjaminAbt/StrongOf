namespace StrongOf;

/// <summary>
/// Represents a strong string of a specific type.
/// </summary>
/// <typeparam name="TStrong">The type of the strong string.</typeparam>
public abstract partial class StrongString<TStrong>
{
    /// <summary>
    /// Gets the length of the value.
    /// </summary>
    public int Length
    {
        get { return Value.Length; }
    }
}
