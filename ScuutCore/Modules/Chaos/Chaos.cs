namespace ScuutCore.Modules.Chaos
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class Chaos : Module<Config>
    {
        public override string Name { get; } = "Chaos";

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