namespace ScuutCore.API.Features
{
    using Interfaces;
    using PluginAPI.Events;

    /// <summary>
    /// A module that handles events.
    /// </summary>
    /// <typeparam name="TModuleConfig">The module config type.</typeparam>
    /// <typeparam name="THandler">The event handler type.</typeparam>
    /// <remarks>If this instance is required to be used by the event handler it should inherit the <see cref="InstanceBasedEventHandler{TModule}"/> class.</remarks>
    public abstract class EventControllerModule<TModuleConfig, THandler> : Module<TModuleConfig>
    where TModuleConfig : IModuleConfig, new() where THandler : IEventHandler, new()
    {
        public THandler EventHandlers { get; protected set; }

        public override void OnEnabled()
        {
            EventHandlers = new THandler();
            if (EventHandlers is InstanceBasedEventHandler<EventControllerModule<TModuleConfig, THandler>> handler)
                handler.Module = this;
            EventManager.RegisterEvents(Plugin.Singleton, EventHandlers);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(Plugin.Singleton, EventHandlers);
            EventHandlers = default;
            base.OnDisabled();
        }
    }
}