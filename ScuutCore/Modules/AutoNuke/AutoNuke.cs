namespace ScuutCore.Modules.AutoNuke
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class AutoNuke : Module<Config>
    {
        public override string Name { get; } = "AutoNuke";

        public static AutoNuke Instance;
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(Plugin.Singleton, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(Plugin.Singleton, EventHandlers);
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}