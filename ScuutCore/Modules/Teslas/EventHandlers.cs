using Exiled.Events.EventArgs;

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
            if (teslas.Config.Roles.Contains(ev.Player.Role))
            {
                ev.IsTriggerable = false;
                ev.IsInIdleRange = false;
            }
        }
    }
}