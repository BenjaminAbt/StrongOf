namespace StrongOf;

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
