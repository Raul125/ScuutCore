using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.ScpSwap
{
    public class ScpSwap : Module<Config>
    {
        public override string Name { get; } = "ScpSwap";

        private EventHandlers EventHandlers;

        public static ScpSwap Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            EventManager.RegisterEvents<EventHandlers>(this);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);
            Singleton = null;

            base.OnDisabled();
        }
    }
}