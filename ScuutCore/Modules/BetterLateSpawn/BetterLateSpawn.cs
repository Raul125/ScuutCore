using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.BetterLateSpawn
{
    public class BetterLateSpawn : Module<Config>
    {
        public override string Name { get; } = "BetterLateSpawn";

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