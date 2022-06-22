using ScuutCore.API;
using Handlers = Exiled.Events.Handlers;

namespace ScuutCore.Modules.Stalky106
{
    public class Stalky106 : Module<Config>
    {
        public override string Name { get; } = "Stalky106";

        private EventHandlers EventHandlers;
        public Methods Methods;

        public override void OnEnabled()
        {
            Methods = new Methods(this);
            EventHandlers = new EventHandlers(this);
            Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;
            Handlers.Player.ChangingRole += EventHandlers.OnChangingRole;
            Handlers.Scp106.CreatingPortal += EventHandlers.OnCreatePortal;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Handlers.Player.ChangingRole -= EventHandlers.OnChangingRole;
            Handlers.Scp106.CreatingPortal -= EventHandlers.OnCreatePortal;

            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}