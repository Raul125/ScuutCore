namespace ScuutCore.Modules.RainbowTags
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class RainbowTags : Module<Config>
    {
        public override string Name { get; } = "RainbowTags";

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