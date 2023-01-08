namespace ScuutCore.Main
{
    using MEC;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Core;

    public class EventHandlers
    {
        public EventHandlers()
        {

        }

        [PluginEvent(ServerEventType.RoundRestart)]
        public void OnRoundRestarting()
        {
            // This prevent us from having unwanted coroutines running
            foreach (CoroutineHandle cor in Plugin.Coroutines)
                Timing.KillCoroutines(cor);

            Plugin.Coroutines.Clear();
        }

        [PluginEvent(ServerEventType.LczDecontaminationStart)]
        public bool OnDecontaminating()
        {
            if (Round.Duration.TotalMinutes < 6)
                return false;

            return true;
        }
    }
}