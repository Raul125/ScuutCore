using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.BetterCandy
{
    public class BetterCandy : Module<Config>
    {
        public override string Name { get; } = "BetterCandy";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.InteractingScp330 += EventHandlers.OnInteractingWithScp330;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.InteractingScp330 -= EventHandlers.OnInteractingWithScp330;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}