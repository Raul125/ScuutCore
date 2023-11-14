namespace ScuutCore.Modules.Spawn3114;

using System;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using PlayerRoles.RoleAssign;
using ScuutCore.API.Features;

public class Spawn3114 : Module<Config>
{
    public override void OnEnabled()
    {
        RoleAssigner.OnPlayersSpawned += Scp3114Spawner.OnPlayersSpawned;
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        RoleAssigner.OnPlayersSpawned -= Scp3114Spawner.OnPlayersSpawned;
        base.OnDisabled();
    }

    private void OnPlayersSpawned()
    {
        Scp3114Spawner._ragdollsSpawned = false;
        if (UnityEngine.Random.value >= Config.Chance)
        {
            return;
        }
        Scp3114Spawner.SpawnCandidates.Clear();
        PlayerRolesUtils.ForEachRole<HumanRole>(Scp3114Spawner.SpawnCandidates.Add);
        if (Scp3114Spawner.SpawnCandidates.Count < Config.MaxScp3114)
        {
            return;
        }
        Scp3114Spawner.SpawnCandidates.RandomItem<ReferenceHub>().roleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RoundStart);
    }
}