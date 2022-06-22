using ScuutCore.API;

namespace ScuutCore.Modules.CleanupUtility
{
    public class CleanupUtility : Module<Config>
    {
        public override string Name { get; } = "CleanupUtility";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.OnRoundStart;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}