namespace ScuutCore.API.Features;

/// <summary>
/// A base class for event handlers that require an instance of a module.
/// </summary>
/// <typeparam name="TModule"></typeparam>
public abstract class InstanceBasedEventHandler<TModule> : IEventHandler
{
    /// <summary>The module instance.</summary>
    public TModule Module { get; set; }

}