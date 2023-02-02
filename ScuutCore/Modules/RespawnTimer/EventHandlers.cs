namespace ScuutCore.Modules.RespawnTimer
{
    using MEC;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Respawning;
    using NorthwoodLib.Pools;
    using System.Text;
    using Hints;
    using PlayerRoles;
    using PluginAPI.Core;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class EventHandlers
    {
        private RespawnTimer rt;
        public EventHandlers(RespawnTimer rt)
        {
            this.rt = rt;
        }

        private CoroutineHandle _timerCoroutine;

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        internal void OnWaitingForPlayers()
        {
            if (rt.Config.Timers.IsEmpty())
            {
                Log.Error("Timer list is empty!");
                return;
            }

            string chosenTimerName = rt.Config.Timers[Random.Range(0, rt.Config.Timers.Count)];
            TimerView.GetNew(chosenTimerName);
        }

        [PluginEvent(ServerEventType.RoundStart)]
        internal void OnRoundStart()
        {
            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);

            _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());

            Log.Debug($"RespawnTimer coroutine started successfully!", rt.Config.Debug);
        }

        private IEnumerator<float> TimerCoroutine()
        {
            do
            {
                yield return Timing.WaitForSeconds(1f);

                Spectators.Clear();
                Spectators.AddRange(ReferenceHub.AllHubs.Select(Player.Get).Where(x => !x.IsServer && !x.IsAlive));
                string text = TimerView.Current.GetText(Spectators.Count);

                foreach (Player player in Spectators)
                {
                    if (player.Role == RoleTypeId.Overwatch && rt.Config.HideTimerForOverwatch || API.TimerHidden.Contains(player.UserId))
                        continue;

                    ShowHint(player, text, 1.25f);
                }

            } while (!global::RoundSummary.singleton._roundEnded);
        }

        public void ShowHint(Player player, string message, float duration = 3f)
        {
            HintParameter[] parameters =
            {
                new StringHintParameter(message)
            };

            player.ReferenceHub.networkIdentity.connectionToClient.Send(new HintMessage(new TextHint(message, parameters, durationScalar: duration)));
        }

        private static readonly List<Player> Spectators = new(25);
    }
}