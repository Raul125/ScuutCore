using ScuutCore.API;
using Server = Exiled.Events.Handlers.Server;

namespace ScuutCore.Modules.AutoFFToggle
{
    public class AutoFFToggle : Module<Config>
    {
        public override string Name { get; } = "AutoFFToggle";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers();
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnded;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnded;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}