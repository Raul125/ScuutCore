using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Scp096;

namespace ScuutCore.Modules.Scp096Notifications
{
    public class EventHandlers
    {
        private Scp096Notifications scp096Notifications;
        public EventHandlers(Scp096Notifications scp096nt)
        {
            scp096Notifications = scp096nt;
        }

        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (ev.Target == null ||
                ev.Target.SessionVariables.ContainsKey("IsNPC") ||
                ev.Player.SessionVariables.ContainsKey("IsNPC"))
                return;

            if (scp096Notifications.Config.Enable096SeenMessage)
            {
                ev.Target.ShowHint(scp096Notifications.Config.Scp096SeenMessage, scp096Notifications.Config.HintDuration);
            }

            if (scp096Notifications.Config.Enable096NewTargetMessage)
            {
                if (!scp096Notifications.Config.RoleStrings.TryGetValue(ev.Target.Role.Type, out string translatedRole))
                    translatedRole = ev.Target.Role.Type.ToString();

                string message = scp096Notifications.Config.Scp096NewTargetMessage
                    .Replace("$name", ev.Target.Nickname)
                    .Replace("$class", $"<color={ev.Target.Role.Color.ToHex()}>{translatedRole}</color>");

                ev.Player.ShowHint(message, scp096Notifications.Config.HintDuration);
            }
        }
    }
}