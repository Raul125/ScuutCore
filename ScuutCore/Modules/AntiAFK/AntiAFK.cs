using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.AntiAFK
{
    public class AntiAFK : Module<Config>
    {
        public override string Name { get; } = "AntiAFK";

        public static AntiAFK Singleton;

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            EventHandlers = new EventHandlers();
            Player.Verified += EventHandlers.OnVerified;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= EventHandlers.OnVerified;
            EventHandlers = null;
            Singleton = null;

            base.OnDisabled();
        }
    }
}