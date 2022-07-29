using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.BetterLateSpawn
{
    public class BetterLateSpawn : Module<Config>
    {
        public override string Name { get; } = "BetterLateSpawn";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Player.Verified += EventHandlers.OnVerified;
            Player.Destroying += EventHandlers.OnDestroying;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= EventHandlers.OnVerified;
            Player.Destroying -= EventHandlers.OnDestroying;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}