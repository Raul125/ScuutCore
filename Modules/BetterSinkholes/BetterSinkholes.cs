using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.BetterSinkholes
{
    public class BetterSinkholes : Module<Config>
    {
        public override string Name { get; } = "BetterSinkholes";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.WalkingOnSinkhole += EventHandlers.OnWalkingOnSinkhole;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.WalkingOnSinkhole -= EventHandlers.OnWalkingOnSinkhole;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}