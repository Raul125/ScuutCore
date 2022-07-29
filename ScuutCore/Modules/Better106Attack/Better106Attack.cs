using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Better106Attack
{
    public class Better106Attack : Module<Config>
    {
        public override string Name { get; } = "Better106Attack";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.EnteringPocketDimension += EventHandlers.OnEnteringPocketDimension;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.EnteringPocketDimension -= EventHandlers.OnEnteringPocketDimension;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}