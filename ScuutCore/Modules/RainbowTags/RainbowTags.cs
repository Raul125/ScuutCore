using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.RainbowTags
{
    public class RainbowTags : Module<Config>
    {
        public override string Name { get; } = "RainbowTags";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.ChangingGroup += EventHandlers.OnChangingGroup;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingGroup -= EventHandlers.OnChangingGroup;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}