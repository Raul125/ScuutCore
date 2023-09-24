using PlayerRoles;
using PluginAPI.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScuutCore.Modules.ChooseRole;

using ScuutCore.Modules.ChooseRole.Components;

public class TeamTrigger : MonoBehaviour
{
    public Team Team;
    public readonly List<Player> Players = new();

    public void Start()
    {
        CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player ply = Player.Get(other.gameObject);
        if (ply is null || ContainsPlayer(ply))
            return;

        if (ply.TryGetComponent<HudComponent>(out var comp))
            comp.Team = getTeamString(Team);

        Players.Add(ply);
    }

    private void OnTriggerExit(Collider other)
    {
        Player ply = Player.Get(other.gameObject);
        if (ply is null || !ContainsPlayer(ply))
            return;

        if (ply.TryGetComponent<HudComponent>(out var comp))
            comp.Team = ChooseRole.Singleton.Config.Random;

        Players.Remove(ply);
    }

    public bool ContainsPlayer(Player player) => Players.Contains(player);

    private static string getTeamString(Team team)
    {
        if (team == Team.ClassD)
            return ChooseRole.Singleton.Config.ClassD;

        if (team == Team.SCPs)
            return ChooseRole.Singleton.Config.Scp;

        if (team == Team.FoundationForces)
            return ChooseRole.Singleton.Config.Mtf;

        if (team == Team.Scientists)
            return ChooseRole.Singleton.Config.Scientists;

        return ChooseRole.Singleton.Config.Random;
    }
}