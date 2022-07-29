using ScuutCore.API;
using Handlers = Exiled.Events.Handlers;

namespace ScuutCore.Modules.BetterCandy
{
    public class BetterCandy : Module<Config>
    {
        public override string Name { get; } = "BetterCandy";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Handlers.Scp330.InteractingScp330 += EventHandlers.OnInteractingWithScp330;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Handlers.Scp330.InteractingScp330 -= EventHandlers.OnInteractingWithScp330;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}