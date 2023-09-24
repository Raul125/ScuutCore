namespace ScuutCore.Modules.RespawnTimer;

using System.Collections.Generic;
using System.Linq;
using API.Features;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using ScuutCore.API.Features;
using ScuutCore.Modules.Patreon;
using UnityEngine;

public sealed class EventHandlers : InstanceBasedEventHandler<RespawnTimer>
{
    private CoroutineHandle _timerCoroutine;

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    internal void OnWaitingForPlayers()
    {
        if (Module.Config.Timers.IsEmpty())
        {
            Log.Error("Timer list is empty!");
            return;
        }

        string chosenTimerName = Module.Config.Timers[Random.Range(0, Module.Config.Timers.Count)];
        TimerView.GetNew(chosenTimerName);
    }

    [PluginEvent(ServerEventType.RoundStart)]
    internal void OnRoundStart()
    {
        if (_timerCoroutine.IsRunning)
            Timing.KillCoroutines(_timerCoroutine);

        _timerCoroutine = Timing.RunCoroutine(TimerCoroutine());

        Log.Debug($"RespawnTimer coroutine started successfully!", Module.Config.Debug);
    }

    private IEnumerator<float> TimerCoroutine()
    {
        do
        {
            yield return Timing.WaitForSeconds(1f);

            Spectators.Clear();

            foreach (var player in Player.GetPlayers().Where(player => player is { IsServer: false }))
            {
                if (player.IsAlive)
                {
                    if (player.ShouldShowSpectatorList() && Module.Config.EnableSpectatorList)
                        player.ShowSpectators();
                }
                else
                {
                    Spectators.Add(player);
                }
            }

            string text = TimerView.Current.GetText(Spectators.Count);

            foreach (var player in Spectators)
            {
                if (player.Role == RoleTypeId.Overwatch && Module.Config.HideTimerForOverwatch || API.API.TimerHidden.Contains(player.UserId))
                    continue;

                player.ReceiveHint(text, 1.25f);
            }

        } while (!global::RoundSummary.singleton._roundEnded);
    }

    private static readonly List<Player> Spectators = new(25);
}