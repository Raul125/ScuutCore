using ScuutCore.API;

namespace ScuutCore.Modules.ScpSwap
{
    public class ScpSwap : Module<Config>
    {
        public override string Name { get; } = "ScpSwap";

        private EventHandlers EventHandlers;

        public static ScpSwap Singleton;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers += EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RestartingRound += EventHandlers.OnRestartingRound;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= EventHandlers.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RestartingRound -= EventHandlers.OnRestartingRound;

            EventHandlers = null;
            Singleton = this;

            base.OnDisabled();
        }
    }
}