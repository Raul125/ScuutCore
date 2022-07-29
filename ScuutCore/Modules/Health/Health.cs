using ScuutCore.API;
using Player = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.Health
{
    public class Health : Module<Config>
    {
        public override string Name { get; } = "Health";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);

            Player.ChangingRole += EventHandlers.OnChangingRole;
            Player.Died += EventHandlers.OnPlayerDied;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.ChangingRole -= EventHandlers.OnChangingRole;
            Player.Died -= EventHandlers.OnPlayerDied;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}