using Exiled.Events.EventArgs;
using MEC;
using Exiled.API.Features;
using System.Collections.Generic;

namespace ScuutCore.Modules.AutoNuke
{
    public class EventHandlers
    {
        private AutoNuke autoNuke;
        public EventHandlers(AutoNuke at)
        {
            autoNuke = at;
        }

        private bool isAutoNuke = false;

        public void OnRoundStart()
        {
            isAutoNuke = false;
            Plugin.Coroutines.Add(Timing.RunCoroutine(AutoNukeCoroutine()));
        }

        public void OnWarheadStopping(StoppingEventArgs ev)
        {
            if (isAutoNuke)
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint(autoNuke.Config.CantDisableHint, autoNuke.Config.CantDisableHintTime);
            }
        }

        public IEnumerator<float> AutoNukeCoroutine()
        {
            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeWarn);

            if (Warhead.IsDetonated)
                yield break;

            Cassie.Message(autoNuke.Config.AutoNukeCassieWarn, false, false, false);
            Map.Broadcast(autoNuke.Config.AutoNukeWarnBroadcast);
            Map.ShowHint(autoNuke.Config.AutoNukeWarnHint, autoNuke.Config.AutoNukeWarnHintDuration);

            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeStartTime - autoNuke.Config.AutoNukeWarn);

            isAutoNuke = true;
            if (!Warhead.IsDetonated && !Warhead.IsInProgress)
            {
                Cassie.Message(autoNuke.Config.AutoNukeCassieStart, false, false, false);
                Map.Broadcast(autoNuke.Config.AutoNukeStartBroadcast);
                Map.ShowHint(autoNuke.Config.AutoNukeStartHint, autoNuke.Config.AutoNukeStartHintDuration);
                Warhead.Start();
            }
        }
    }
}