using PluginAPI.Events;
using ScuutCore.API;

namespace ScuutCore.Modules.Teslas
{
    using ScuutCore.API.Features;

    public class Teslas : Module<Config>
    {
        public override string Name { get; } = "Teslas";
        public EventHandlers EventHandlers;
        public static Teslas Singleton;

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