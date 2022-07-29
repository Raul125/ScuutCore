using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Teslas
{
    public class Teslas : Module<Config>
    {
        public override string Name { get; } = "Teslas";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.TriggeringTesla += EventHandlers.OnTriggeringTesla;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.TriggeringTesla -= EventHandlers.OnTriggeringTesla;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}