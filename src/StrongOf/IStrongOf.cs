namespace StrongOf;

/// <summary>
/// The StrongOf namespace contains the interface definition for strong types.
/// </summary>
public interface IStrongOf
{
    /// <summary>
    /// Returns a string that represents the value of the underlying type. This method is intended to be overridden by any class implementing the IStrongOf interface.
    /// </summary>
    /// <returns>A string that represents the underlying type.</returns>
    string ToString();
}
