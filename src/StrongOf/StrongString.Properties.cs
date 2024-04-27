namespace StrongOf;

#pragma warning disable MA0097 // A class that implements IComparable<T> or IComparable should override comparison operators

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
