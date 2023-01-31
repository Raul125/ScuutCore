namespace ScuutCore.Modules.AutoNuke
{
    using MEC;
    using PluginAPI.Core;
    using System.Collections.Generic;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;

    public class EventHandlers
    {
        private AutoNuke autoNuke;
        public EventHandlers(AutoNuke at)
        {
            autoNuke = at;
        }

        public bool IsAutoNuke = false;

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart()
        {
            IsAutoNuke = false;
            Plugin.Coroutines.Add(Timing.RunCoroutine(AutoNukeCoroutine()));
        }

        [PluginEvent(ServerEventType.WarheadStop)]
        public bool OnWarheadStopping(Player player)
        {
            if (IsAutoNuke)
            {
                autoNuke.Config.CantDisableHint.Show(player);
                return false;
            }

            return true;
        }

        public IEnumerator<float> AutoNukeCoroutine()
        {
            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeWarn);

            if (Warhead.IsDetonated)
                yield break;

            autoNuke.Config.AutoNukeCassieWarn.Play();
            foreach (var ply in Player.GetPlayers())
                autoNuke.Config.AutoNukeWarnBroadcast.Show(ply);

            autoNuke.Config.AutoNukeWarnHint.Show();
            yield return Timing.WaitForSeconds(autoNuke.Config.AutoNukeStartTime - autoNuke.Config.AutoNukeWarn);

            IsAutoNuke = true;
            if (!Warhead.IsDetonated && !Warhead.IsDetonationInProgress)
            {
                autoNuke.Config.AutoNukeCassieStart.Play();
                foreach (var ply in Player.GetPlayers())
                    autoNuke.Config.AutoNukeStartBroadcast.Show(ply);

                autoNuke.Config.AutoNukeStartHint.Show();
                Warhead.Start();
            }
        }
    }
}