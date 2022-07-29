using ScuutCore.API;
using Server = Exiled.Events.Handlers.Server;

namespace ScuutCore.Modules.Chaos
{
    public class Chaos : Module<Config>
    {
        public override string Name { get; } = "Chaos";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Server.RespawningTeam += EventHandlers.OnRespawningTeam;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Server.RespawningTeam -= EventHandlers.OnRespawningTeam;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}