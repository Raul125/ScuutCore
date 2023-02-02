namespace ScuutCore.Modules.Scp096Notifications
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class Scp096Notifications : Module<Config>
    {
        public override string Name { get; } = "Scp096Notifications";
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            // EventHandlers = new EventHandlers(this);
            // EventManager.RegisterEvents(Plugin.Singleton, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            // EventManager.UnregisterEvents(Plugin.Singleton, EventHandlers);
            // EventHandlers = null;

            base.OnDisabled();
        }
    }
}