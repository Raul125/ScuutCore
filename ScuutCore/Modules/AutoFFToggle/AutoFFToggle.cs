using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.AutoFFToggle
{
    using ScuutCore.API.Features;

    public class AutoFFToggle : Module<Config>
    {
        public override string Name { get; } = "AutoFFToggle";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            EventManager.RegisterEvents(Plugin.Singleton, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(Plugin.Singleton, EventHandlers);
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}