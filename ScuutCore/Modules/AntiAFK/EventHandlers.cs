using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Player;

namespace ScuutCore.Modules.AntiAFK
{
    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<AfkComponent>();
        }
    }
}