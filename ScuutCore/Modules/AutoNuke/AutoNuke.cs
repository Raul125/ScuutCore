namespace ScuutCore.Modules.AutoNuke
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;
    using static PlayerList;

    public class AutoNuke : Module<Config>
    {
        public override string Name { get; } = "AutoNuke";

        public static AutoNuke Instance;
        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Instance = this;

            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}