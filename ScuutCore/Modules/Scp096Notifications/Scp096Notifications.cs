using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.Scp096Notifications
{
    using ScuutCore.API.Features;

    public class Scp096Notifications : Module<Config>
    {
        public override string Name { get; } = "Scp096Notifications";
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}