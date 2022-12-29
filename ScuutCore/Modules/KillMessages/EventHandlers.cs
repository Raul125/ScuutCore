using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;

namespace ScuutCore.Modules.KillMessages
{
    public class EventHandlers
    {
        private KillMessages killMessages;
        public EventHandlers(KillMessages btc)
        {
            killMessages = btc;
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker is null || ev.Player is null || ev.Player == ev.Attacker)
                return;

            ev.Attacker.ShowHint(killMessages.Config.Message.Replace("{name}", ev.Player.Nickname), 4);
        }
    }
}