namespace ScuutCore.Modules.RespawnTimer.API.Extensions;

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using AutoNuke;
using Features;
using GameCore;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
using Respawning;
using UnityEngine;

public static class StringBuilderExtensions
{
    public static StringBuilder SetAllProperties(this StringBuilder builder, int? spectatorCount = null) => builder
        .SetRoundTime()
        .SetMinutesAndSeconds()
        .SetSpawnableTeam()
        .SetSpectatorCountAndTickets(spectatorCount)
        .SetWarheadStatus()
        .SetGeneratorCount()
        .SetTpsAndTickrate()
        .SetHint();

    private static StringBuilder SetRoundTime(this StringBuilder builder)
    {
        int minutes = RoundStart.RoundLength.Minutes;
        builder.Replace("{round_minutes}", $"{(TimerView.Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

        int seconds = RoundStart.RoundLength.Seconds;
        builder.Replace("{round_seconds}", $"{(TimerView.Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");

        return builder;
    }

    private static StringBuilder SetMinutesAndSeconds(this StringBuilder builder)
    {
        TimeSpan time = TimeSpan.FromSeconds(RespawnManager.Singleton._timeForNextSequence - RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds);

        if (RespawnManager.Singleton._curSequence is RespawnManager.RespawnSequencePhase.PlayingEntryAnimations or RespawnManager.RespawnSequencePhase.SpawningSelectedTeam || !TimerView.Current.Properties.TimerOffset)
        {
            int minutes = (int)time.TotalSeconds / 60;
            builder.Replace("{minutes}", $"{(TimerView.Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = (int)Math.Round(time.TotalSeconds % 60);
            builder.Replace("{seconds}", $"{(TimerView.Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
        }
        else
        {
            int offset = RespawnTokensManager.Counters[1].Amount >= 50 ? 18 : 14;

            int minutes = (int)(time.TotalSeconds + offset) / 60;
            builder.Replace("{minutes}", $"{(TimerView.Current.Properties.LeadingZeros && minutes < 10 ? "0" : string.Empty)}{minutes}");

            int seconds = (int)Math.Round((time.TotalSeconds + offset) % 60);
            builder.Replace("{seconds}", $"{(TimerView.Current.Properties.LeadingZeros && seconds < 10 ? "0" : string.Empty)}{seconds}");
        }

        return builder;
    }

    private static StringBuilder SetSpawnableTeam(this StringBuilder builder)
    {
        return Respawn.NextKnownTeam switch
        {
            SpawnableTeamType.NineTailedFox => builder.Replace("{team}", TimerView.Current.Properties.Ntf),
            SpawnableTeamType.ChaosInsurgency => builder.Replace("{team}", TimerView.Current.Properties.Ci),
            _ => builder
        };

    }

    private static StringBuilder SetSpectatorCountAndTickets(this StringBuilder builder, int? spectatorCount = null)
    {
        builder.Replace("{spectators_num}", spectatorCount?.ToString() ?? Player.GetPlayers().Count(x => x.Role == RoleTypeId.Spectator && !x.IsOverwatchEnabled).ToString());
        builder.Replace("{ntf_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[1].Amount).ToString());
        builder.Replace("{ci_tickets_num}", Mathf.Round(RespawnTokensManager.Counters[0].Amount).ToString());

        return builder;
    }

    private static StringBuilder SetWarheadStatus(this StringBuilder builder)
    {
        TimeSpan time = AutoNuke.Instance.EventHandlers.WarheadTime - DateTime.Now;
        if (Warhead.IsDetonationInProgress && !Warhead.IsDetonated)
        {
            if (AutoNuke.Instance.EventHandlers.IsAutoNuke)
                builder.Replace("{auto_nuke_rem}", $"<color=red>In Progress {(uint)Warhead.DetonationTime}</color>");
            else
                builder.Replace("{auto_nuke_rem}", "Stand-By");

            return builder;
        }

        if (Warhead.IsDetonated)
        {
            if (AutoNuke.Instance.EventHandlers.IsAutoNuke)
                builder.Replace("{auto_nuke_rem}", "Detonated");
            else
                builder.Replace("{auto_nuke_rem}", "Failed");

            return builder;
        }

        builder.Replace("{auto_nuke_rem}", time.Minutes > 0 ? $"{time.Minutes}m {time.Seconds}s" : $"<color=red>{time.Seconds}s</color>");
        return builder;
    }

    private static StringBuilder SetGeneratorCount(this StringBuilder builder)
    {
        builder.Replace("{generator_engaged}", Scp079Recontainer.AllGenerators.Count(x => x.Engaged).ToString());
        builder.Replace("{generator_count}", "3");
        return builder;
    }

    private static StringBuilder SetTpsAndTickrate(this StringBuilder builder)
    {
        builder.Replace("{tps}", Math.Round(1.0 / Time.smoothDeltaTime).ToString(CultureInfo.InvariantCulture));
        builder.Replace("{tickrate}", Application.targetFrameRate.ToString());

        return builder;
    }

    private static StringBuilder SetHint(this StringBuilder builder)
    {
        if (!TimerView.Current.Hints.Any())
            return builder;

        builder.Replace("{hint}", TimerView.Current.Hints[TimerView.Current.HintIndex]);
        return builder;
    }
}