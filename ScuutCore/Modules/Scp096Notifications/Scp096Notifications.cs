using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.Scp096Notifications
{
    public class Scp096Notifications : Module<Config>
    {
        public override string Name { get; } = "Scp096Notifications";

        public override void OnEnabled()
        {
            EventManager.RegisterEvents<EventHandlers>(this);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);

            base.OnDisabled();
        }
    }
}