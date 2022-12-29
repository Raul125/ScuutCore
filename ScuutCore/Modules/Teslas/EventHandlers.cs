using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;

namespace ScuutCore.Modules.Teslas
{
    public class EventHandlers
    {
        private Teslas teslas;
        public EventHandlers(Teslas te)
        {
            teslas = te;
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (teslas.Config.Roles.Contains(ev.Player.Role.Type))
            {
                ev.IsAllowed = false;
                ev.IsInIdleRange = false;
            }
        }
    }
}