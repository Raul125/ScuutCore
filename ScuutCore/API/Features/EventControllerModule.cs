namespace ScuutCore.API.Features
{
    using Interfaces;
    using PluginAPI.Events;

    public abstract class EventControllerModule<TModuleConfig, TEventHandler> : Module<TModuleConfig>
    where TModuleConfig : IModuleConfig, new() where TEventHandler : IEventHandler, new()
    {
        protected TEventHandler EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new TEventHandler();
            if (EventHandlers is IInstanceBasedEventHandler<EventControllerModule<TModuleConfig, TEventHandler>> handler)
                handler.AssignModule(this);
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