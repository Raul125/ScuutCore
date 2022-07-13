using Exiled.Events.EventArgs;

namespace ScuutCore.Modules.CustomEscape
{
    public class EventHandlers
    {
        private CustomEscape customEscape;
        public EventHandlers(CustomEscape c)
        {
            customEscape = c;
        }

        public void OnEscaping(EscapingEventArgs ev)
        {
            if (customEscape.Config.CuffedRoleConversions.ContainsKey(ev.Player.Role.Type))
            {
                ev.NewRole = customEscape.Config.CuffedRoleConversions[ev.Player.Role.Type];
            }
        }
    }
}