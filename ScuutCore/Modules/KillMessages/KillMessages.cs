using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.KillMessages
{
    public class KillMessages : Module<Config>
    {
        public override string Name { get; } = "KillMessages";

        public static KillMessages Singleton;

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