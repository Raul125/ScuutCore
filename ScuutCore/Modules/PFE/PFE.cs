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