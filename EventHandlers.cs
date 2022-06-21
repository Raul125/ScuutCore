using MEC;

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
    }
}