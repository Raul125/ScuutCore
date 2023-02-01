namespace ScuutCore.Modules.PFE
{
    using PluginAPI.Events;
    using ScuutCore.API.Features;

    public class PFE : Module<Config>
    {
        public override string Name { get; } = "Peanut fucking explodes";

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