namespace ScuutCore.Modules.Scp008
{
    using PluginAPI.Events;
    using ScuutCore.API.Features;

    public class Scp008 : Module<Config>
    {
        public override string Name { get; } = "Scp008";
        public EventHandlers EventHandlers;

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