using Exiled.Events.EventArgs;

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
            if (ev.Killer is null || ev.Target is null || ev.Target == ev.Killer)
                return;

            ev.Killer.ShowHint(killMessages.Config.Message.Replace("{name}", ev.Target.Nickname), 4);
        }
    }
}