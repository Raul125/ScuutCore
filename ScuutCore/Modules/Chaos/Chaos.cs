using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.Chaos
{
    using ScuutCore.API.Features;

    public class Chaos : Module<Config>
    {
        public override string Name { get; } = "Chaos";

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