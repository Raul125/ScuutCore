using ScuutCore.API;
using Server = Exiled.Events.Handlers.Server;

namespace ScuutCore.Modules.WhoAreMyTeammates
{
    public class WhoAreMyTeammates : Module<Config>
    {
        public override string Name { get; } = "WhoAreMyTeammates";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Server.RoundStarted += EventHandlers.OnRoundStarted;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RoundStarted -= EventHandlers.OnRoundStarted;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}