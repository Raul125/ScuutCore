using ScuutCore.API;
using Players = Exiled.Events.Handlers.Player;

namespace ScuutCore.Modules.RemoteKeycard
{
    public class RemoteKeycard : Module<Config>
    {
        public override string Name { get; } = "RemoteKeycard";

        private EventHandlers EventHandlers;

        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Players.InteractingDoor += EventHandlers.OnDoorInteract;
            Players.UnlockingGenerator += EventHandlers.OnGeneratorUnlock;
            Players.InteractingLocker += EventHandlers.OnLockerInteract;
            Players.ActivatingWarheadPanel += EventHandlers.OnWarheadUnlock;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Players.InteractingDoor -= EventHandlers.OnDoorInteract;
            Players.UnlockingGenerator -= EventHandlers.OnGeneratorUnlock;
            Players.InteractingLocker -= EventHandlers.OnLockerInteract;
            Players.ActivatingWarheadPanel -= EventHandlers.OnWarheadUnlock;

            EventHandlers = null;
            base.OnDisabled();
        }
    }
}