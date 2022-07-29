using Exiled.Events.EventArgs;

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
                ev.Scp096.SessionVariables.ContainsKey("IsNPC"))
                return;

            if (scp096Notifications.Config.Enable096SeenMessage)
            {
                ev.Target.ShowHint(scp096Notifications.Config.Scp096SeenMessage, scp096Notifications.Config.HintDuration);
            }

            if (scp096Notifications.Config.Enable096NewTargetMessage)
            {
                if (!scp096Notifications.Config.RoleStrings.TryGetValue(ev.Target.Role, out string translatedRole))
                    translatedRole = ev.Target.Role.ToString();

                string message = scp096Notifications.Config.Scp096NewTargetMessage
                    .Replace("$name", ev.Target.Nickname)
                    .Replace("$class", $"<color={ev.Target.Role.Color.ToHex()}>{translatedRole}</color>");

                ev.Scp096.ShowHint(message, scp096Notifications.Config.HintDuration);
            }
        }
    }
}