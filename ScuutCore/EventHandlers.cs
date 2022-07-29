using MEC;
using Exiled.Events.EventArgs;
using Exiled.API.Features;

namespace ScuutCore.EventHandlers
{
    public class EventHandlers
    {
        public EventHandlers()
        {

        }

        public void OnRoundRestarting()
        {
            // This prevent us from having unwanted coroutines running
            foreach (CoroutineHandle cor in Plugin.Coroutines)
                Timing.KillCoroutines(cor);

            Plugin.Coroutines.Clear();
        }

        public void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            if (Round.ElapsedTime.TotalMinutes < 6)
                ev.IsAllowed = false;
        }
    }
}