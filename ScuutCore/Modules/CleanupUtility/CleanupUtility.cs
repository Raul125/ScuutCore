namespace ScuutCore.Modules.CleanupUtility
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class CleanupUtility : Module<Config>
    {
        public override string Name { get; } = "CleanupUtility";

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