﻿namespace ScuutCore.Modules.ScpReplace.Models;

using System;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using ScuutCore.API.Helpers;

public class ReplaceInfo
{
    public string UserId { get; set; }
    public RoleTypeId Role { get; set; }
    public float Health { get; set; }
    public DateTime LeaveTime { get; set; }

    private CoroutineHandle coroutine;
    public ReplaceInfo(Player player)
    {
        UserId = player.UserId;
        Role = player.Role;
        Health = player.Health;
        LeaveTime = DateTime.Now;

        ScpReplaceModule.Singleton.Debug($"Replace constr {UserId} role {Role}");
        coroutine = Timing.CallDelayed(ScpReplaceModule.Singleton.Config.SecondsClaimPeriod, () =>
        {
            ScpReplaceModule.Singleton.Debug($"Removed {UserId} role {Role} from replace list");
            ScpReplaceModule.ReplaceInfos.Remove(this);
        });
    }

    public void Apply(Player ply)
    {
        ScpReplaceModule.Singleton.Debug($"Apply {UserId} role {Role}");
        Cancel();
        GlobalHelpers.BroadcastToPermissions($"{ply.Nickname} has claimed {Role}!", "scpreplace");
        ply.SetRole(Role);
        Timing.CallDelayed(0.5f, () => ply.Health = Health);
    }

    public void Cancel() => Timing.KillCoroutines(coroutine);

    public override string ToString() =>
        $"{Role} has left and the grace period expired, use .scpreplace to replace them.";
}