namespace ScuutCore.Modules.Restart
{
    using PluginAPI.Events;
    using ScuutCore.API;
    using ScuutCore.API.Features;

    public class Restart : Module<Config>
    {
        public override string Name { get; } = "Restart";
        public EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            EventManager.RegisterEvents(this, EventHandlers);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, EventHandlers);
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}