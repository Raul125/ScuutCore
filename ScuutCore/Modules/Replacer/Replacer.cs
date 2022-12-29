using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Replacer
{
    public class Replacer : Module<Config>
    {
        public override string Name { get; } = "Replacer";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Destroying += EventHandlers.OnDestroying;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Destroying -= EventHandlers.OnDestroying;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}