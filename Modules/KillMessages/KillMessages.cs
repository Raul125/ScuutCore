using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.KillMessages
{
    public class KillMessages : Module<Config>
    {
        public override string Name { get; } = "KillMessages";

        public static KillMessages Singleton;

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            Singleton = this;
            Database.Open();
            EventHandlers = new EventHandlers(this);

            Player.Died += EventHandlers.OnDied;
            Player.Verified += EventHandlers.OnVerified;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Died += EventHandlers.OnDied;
            Player.Verified += EventHandlers.OnVerified;

            EventHandlers = null;
            Database.Close();

            Singleton = null;

            base.OnDisabled();
        }
    }
}