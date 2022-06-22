using ScuutCore.API;

namespace ScuutCore.Modules.AdminTools
{
    public class AdminTools : Module<Config>
    {
        public override string Name { get; } = "AdminTools";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Player.Verified += EventHandlers.OnPlayerVerified;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.OnRoundEnd;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= EventHandlers.OnPlayerVerified;
            Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.OnRoundEnd;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}