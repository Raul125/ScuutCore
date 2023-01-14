using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.ScpSwap
{
    using ScuutCore.API.Features;

    public class ScpSwap : Module<Config>
    {
        public override string Name { get; } = "ScpSwap";

        private EventHandlers EventHandlers;

        public static ScpSwap Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;
            Singleton = null;

            base.OnDisabled();
        }
    }
}