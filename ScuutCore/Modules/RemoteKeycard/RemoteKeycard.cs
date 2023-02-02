using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.RemoteKeycard
{
    using ScuutCore.API.Features;

    public class RemoteKeycard : Module<Config>
    {
        public override string Name { get; } = "RemoteKeycard";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
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