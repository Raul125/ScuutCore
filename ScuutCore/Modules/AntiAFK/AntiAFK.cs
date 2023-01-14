using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.AntiAFK
{
    using ScuutCore.API.Features;

    public class AntiAFK : Module<Config>
    {
        public override string Name { get; } = "AntiAFK";

        public static AntiAFK Singleton;

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();
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