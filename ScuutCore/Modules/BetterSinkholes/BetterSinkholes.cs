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
            Player.StayingOnEnvironmentalHazard += EventHandlers.OnStayingOnEnvironmentalHazard;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.StayingOnEnvironmentalHazard -= EventHandlers.OnStayingOnEnvironmentalHazard;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}