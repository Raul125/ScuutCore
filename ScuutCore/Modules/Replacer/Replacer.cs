namespace ScuutCore.Modules.Replacer
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;
    using ScuutCore.API;

    public class Replacer : Module<Config>
    {
        public override string Name { get; } = "Replacer";

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