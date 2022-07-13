using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.CustomEscape
{
    public class CustomEscape : Module<Config>
    {
        public override string Name { get; } = "CustomEscape";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Player.Escaping += EventHandlers.OnEscaping;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Escaping -= EventHandlers.OnEscaping;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}