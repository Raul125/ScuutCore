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
            Exiled.Events.Handlers.Map.Decontaminating += EventHandlers.OnDecontaminating;
            Exiled.Events.Handlers.Warhead.Detonated += EventHandlers.OnDetonated;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.OnRoundStart;
            Exiled.Events.Handlers.Map.Decontaminating -= EventHandlers.OnDecontaminating;
            Exiled.Events.Handlers.Warhead.Detonated -= EventHandlers.OnDetonated;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}