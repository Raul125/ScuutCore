namespace ScuutCore.Modules.ScpReplace.Models
{
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

        private CoroutineHandle[] coroutines;
        public ReplaceInfo(Player player)
        {
            UserId = player.UserId;
            Role = player.Role;
            Health = player.Health;
            LeaveTime = DateTime.Now;

            var gracePeriod = ScpReplaceModule.Singleton!.Config.SecondsGracePeriod;
            coroutines = new CoroutineHandle[2];
            coroutines[0] = Timing.CallDelayed(gracePeriod, () =>
            {
                GlobalHelpers.BroadcastToPermissions(ToString(), "scpreplace");
            });
            coroutines[1] = Timing.CallDelayed(gracePeriod + ScpReplaceModule.Singleton.Config.SecondsClaimPeriod, () =>
            {
                ScpReplaceModule.ReplaceInfos.Remove(this);
            });
        }

        public void Apply(Player ply)
        {
            Cancel();
            GlobalHelpers.BroadcastToPermissions($"{ply.Nickname} has claimed {Role}!", "scpreplace");
            ply.SetRole(Role);
            Timing.CallDelayed(0.5f, () => ply.Health = Health - ply.MaxHealth *
                // ReSharper disable once PossibleLossOfFraction
                (ScpReplaceModule.Singleton!.Config.HealthDeductionPercent / 100));
        }

        public void Cancel() => Timing.KillCoroutines(coroutines);

        public override string ToString() =>
            $"{Role} has left and the grace period expired, use .scpreplace to replace them.";
    }
}