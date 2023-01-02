using Exiled.Events.EventArgs;
using MEC;
using PluginAPI.Core;
using System.Collections.Generic;
using LightContainmentZoneDecontamination;
using Mirror;
using Respawning.NamingRules;
using Exiled.Events.EventArgs.Warhead;

namespace ScuutCore.Modules.AutoNuke
{
    public class EventHandlers
    {
        private AutoNuke autoNuke;
        public EventHandlers(AutoNuke at)
        {
            autoNuke = at;
        }

        public bool IsAutoNuke = false;

        public void OnRoundStart()
        {
            IsAutoNuke = false;
            Plugin.Coroutines.Add(Timing.RunCoroutine(AutoNukeCoroutine()));
        }

        public void OnWarheadStopping(StoppingEventArgs ev)
        {
            if (IsAutoNuke)
            {
                ev.IsAllowed = false;
                autoNuke.Config.CantDisableHint.Show(ev.Player);
            }
        }

        public IEnumerator<float> AutoNukeCoroutine()
        {
            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeWarn);

            if (Warhead.IsDetonated)
                yield break;

            autoNuke.Config.AutoNukeCassieWarn.Play();
            Map.Broadcast(autoNuke.Config.AutoNukeWarnBroadcast);
            autoNuke.Config.AutoNukeWarnHint.Show();

            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeStartTime - autoNuke.Config.AutoNukeWarn);

            IsAutoNuke = true;
            if (!Warhead.IsDetonated && !Warhead.IsInProgress)
            {
                autoNuke.Config.AutoNukeCassieStart.Play();
                Map.Broadcast(autoNuke.Config.AutoNukeStartBroadcast);
                autoNuke.Config.AutoNukeStartHint.Show();
                Warhead.Start();
            }
        }
    }
}