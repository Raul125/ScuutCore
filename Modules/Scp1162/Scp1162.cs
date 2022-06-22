using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Scp1162
{
    public class Scp1162 : Module<Config>
    {
        public override string Name { get; } = "Scp1162";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.DroppingItem += EventHandlers.OnDroppingItem;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.DroppingItem -= EventHandlers.OnDroppingItem;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}