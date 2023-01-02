using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.RoundSummary
{
    public class RoundSummary : Module<Config>
    {
        public override string Name { get; } = "RoundSummary";

        private EventHandlers EventHandlers;

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