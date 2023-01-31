namespace ScuutCore.Modules.Health
{
    using ScuutCore.API.Features;
    using PluginAPI.Events;

    public class Health : Module<Config>
    {
        public override string Name { get; } = "Health";

        private EventHandlers EventHandlers;

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