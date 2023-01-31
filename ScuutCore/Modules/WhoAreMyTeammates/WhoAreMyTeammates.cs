namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using PluginAPI.Events;
    using ScuutCore.API.Features;

    public class WhoAreMyTeammates : Module<Config>
    {
        public override string Name { get; } = "WhoAreMyTeammates";
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