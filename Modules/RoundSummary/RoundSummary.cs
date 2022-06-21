using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace ScuutCore.Modules.RoundSummary
{
    public class RoundSummary : Module<Config>
    {
        public override string Name { get; } = "RoundSummary";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Died += EventHandlers.OnDied;
            Player.Escaping += EventHandlers.OnPlayerEscaping;
            Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded += EventHandlers.OnRoundEnd;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Died -= EventHandlers.OnDied;
            Player.Escaping -= EventHandlers.OnPlayerEscaping;
            Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Server.RoundEnded -= EventHandlers.OnRoundEnd;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}