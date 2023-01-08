using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.AdminTools
{
    public class AdminTools : Module<Config>
    {
        public override string Name { get; } = "AdminTools";

        private EventHandlers EventHandlers;

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