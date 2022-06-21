using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace ScuutCore.Modules.AutoFFToggle
{
    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        public void OnRoundStarted()
        {
            Server.FriendlyFire = false;
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Server.FriendlyFire = true;
        }
    }
}