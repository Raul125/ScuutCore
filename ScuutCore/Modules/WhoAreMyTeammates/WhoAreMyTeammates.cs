namespace ScuutCore.Modules.WhoAreMyTeammates
{
    using PluginAPI.Events;
    using ScuutCore.API;

    public class WhoAreMyTeammates : Module<Config>
    {
        public override string Name { get; } = "WhoAreMyTeammates";

        public override void OnEnabled()
        {
            EventManager.RegisterEvents<EventHandlers>(this);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventManager.UnregisterEvents<EventHandlers>(this);

            base.OnDisabled();
        }
    }
}