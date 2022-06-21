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
            if (ev.Target.GetDisabled() || ev.Killer == null || ev.Target == null || string.IsNullOrEmpty(ev.Killer.GetMessage()) || (!killMessages.Config.ShowOnSuicide && ev.Target == ev.Killer)) return;

            string message = $"<size={killMessages.Config.MessageSize}><color={ev.Killer.GetColor()}>{killMessages.Config.Message.Replace("$message", ev.Killer.GetMessage()).Replace("$author", ev.Killer.Nickname)}</color></size>";

            if (killMessages.Config.UseBroadcast)
            {
                ev.Target.Broadcast(killMessages.Config.MessageDuration, message);
                return;
            }

            ev.Target.ShowHint(message, killMessages.Config.MessageDuration);
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            if (killMessages.Config.SendConsoleMessage)
            {
                string current = ev.Player.GetMessage();
                string color = ev.Player.GetColor();
                ev.Player.SendConsoleMessage(killMessages.Config.ConsoleMessage.Replace("$helpmsg", killMessages.Config.HelpMessage).Replace("$current", string.IsNullOrEmpty(current) ? killMessages.Config.MessageNotSet : current).Replace("$color", color),
                    "default");
            }
        }
    }
}