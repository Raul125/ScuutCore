namespace ScuutCore.Modules.AdminTools
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;
    public class AdminTools : Module<Config>
    {
        public override string Name { get; } = "AdminTools";

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