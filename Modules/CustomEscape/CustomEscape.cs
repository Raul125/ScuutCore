using ScuutCore.API;
using Server = Exiled.Events.Handlers.Server;

namespace ScuutCore.Modules.CustomEscape
{
    public class CustomEscape : Module<Config>
    {
        public override string Name { get; } = "CustomEscape";

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