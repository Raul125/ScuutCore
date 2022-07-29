using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.CuffedTK
{
    public class CuffedTK : Module<Config>
    {
        public override string Name { get; } = "CuffedTK";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Hurting += EventHandlers.OnHurting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Hurting -= EventHandlers.OnHurting;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}