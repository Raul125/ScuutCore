namespace ScuutCore.Modules.ScpSwap
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class ScpSwap : Module<Config>
    {
        public override string Name { get; } = "ScpSwap";

        private EventHandlers EventHandlers;

        public static ScpSwap Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(Plugin.Singleton, EventHandlers);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(Plugin.Singleton, EventHandlers);
            EventHandlers = null;
            Singleton = null;

            base.OnDisabled();
        }
    }
}