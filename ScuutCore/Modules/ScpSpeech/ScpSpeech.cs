using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.ScpSpeech
{
    public class ScpSpeech : Module<Config>
    {
        public override string Name { get; } = "ScpSpeech";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Transmitting += EventHandlers.OnTransmitting;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Transmitting -= EventHandlers.OnTransmitting;
            EventHandlers = null;

            base.OnDisabled();
        }
    }
}