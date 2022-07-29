using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Subclasses
{
    public class Subclasses : Module<Config>
    {
        public override string Name { get; } = "Subclasses";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}