using ScuutCore.API;

namespace ScuutCore.Modules.AutoNuke
{
    public class AutoNuke : Module<Config>
    {
        public override string Name { get; } = "AutoNuke";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Warhead.Stopping += EventHandlers.OnWarheadStopping;
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Warhead.Stopping -= EventHandlers.OnWarheadStopping;
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}