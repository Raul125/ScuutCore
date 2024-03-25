namespace ScuutCore.Modules.ChooseRole;

using ScuutCore.API.Features;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Respawning;
using PluginAPI.Core;
using System.Collections.Generic;
using PlayerRoles.RoleAssign;
using PlayerRoles;
using System.Linq;
using UnityEngine;
using PlayerStatsSystem;
using AdminToys;
using Mirror;
using PlayerRoles.Ragdolls;
using ScuutCore.API.Extensions;
using PluginAPI.Events;
using ScuutCore.Modules.ChooseRole.Components;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using GameCore;

public sealed class EventHandlers : InstanceBasedEventHandler<ChooseRole>
{
    private int SCPsToSpawn = 0;
    private int ClassDsToSpawn = 0;
    private int ScientistsToSpawn = 0;
    private int GuardsToSpawn = 0;

    public List<Player> SCPPlayers = new();
    public List<Player> ScientistPlayers = new();
    public List<Player> GuardPlayers = new();
    public List<Player> ClassDPlayers = new();

    public readonly HashSet<TeamTrigger> Triggers = new();
    public readonly HashSet<PrimitiveObjectToy> Primitives = new();

    private Vector3 lobbyPos = new(0.82f, 995.38f, -7.92f);

    [PluginEvent(ServerEventType.PlayerJoined)]
    public void OnPlayerJoin(Player player)
    {
        bool isStarted = ReferenceHub.LocalHub?.characterClassManager.RoundStarted ?? false;
        if ((!(global::RoundSummary.singleton._roundEnded || isStarted)) && (RoundStart.singleton.NetworkTimer > 4 || RoundStart.singleton.NetworkTimer == -2))
        {
            player.GameObject.AddComponent<HudComponent>().Init(player.ReferenceHub);
            player.Role = RoleTypeId.Tutorial;
            player.Position = lobbyPos;
            player.ClearInventory();
        }
    }

    [PluginEvent(ServerEventType.PlayerLeft)]
    public void OnPlayerLeave(Player player)
    {
        foreach (var trigger in Triggers)
        {
            if (trigger.Players.Contains(player))
                trigger.Players.Remove(player);
        }
    }

    [PluginEvent(ServerEventType.RagdollSpawn)]
    public bool OnRagdollSpawn(Player player, IRagdollRole ragdoll, DamageHandlerBase damage)
    {
        if (Round.IsRoundStarted)
            return true;

        return false;
    }

    [PluginEvent(ServerEventType.RoundStart)]
    public void OnRoundStart()
    {
        RoleAssigner._spawned = true;
        List<Player> bulkList = Player.GetPlayers().Where(x => !x.IsOverwatchEnabled && !x.IsServer).ToList();
        for (int x = 0; x < bulkList.Count; x++)
        {
            if (x >= Module.Config.SpawnQueue.Count())
            {
                ClassDsToSpawn++;
                continue;
            }

            switch (Module.Config.SpawnQueue[x])
            {
                case '4':
                    ClassDsToSpawn++;
                    break;
                case '0':
                    SCPsToSpawn++;
                    break;
                case '1':
                    GuardsToSpawn++;
                    break;
                case '3':
                    ScientistsToSpawn++;
                    break;
            }
        }

        List<Player> PlayersToSpawnAsSCP = new();
        List<Player> PlayersToSpawnAsScientist = new();
        List<Player> PlayersToSpawnAsGuard = new();
        List<Player> PlayersToSpawnAsClassD = new();

        ClassDPlayers = Triggers.First(x => x.Team == Team.ClassD).Players.Where(x => !x.IsOffline).ToList();
        GuardPlayers = Triggers.First(x => x.Team == Team.FoundationForces).Players.Where(x => !x.IsOffline).ToList();
        ScientistPlayers = Triggers.First(x => x.Team == Team.Scientists).Players.Where(x => !x.IsOffline).ToList();
        SCPPlayers = Triggers.First(x => x.Team == Team.SCPs).Players.Where(x => !x.IsOffline).ToList();

        ClassDPlayers.ShuffleList();
        GuardPlayers.ShuffleList();
        ScientistPlayers.ShuffleList();
        SCPPlayers.ShuffleList();

        // ---------------------------------------------------------------------------------------\\
        // ClassD
        if (ClassDsToSpawn != 0)
        {
            if (ClassDPlayers.Count <= ClassDsToSpawn) // Less people (or equal) voted than what is required in the game.
            {
                foreach (Player ply in ClassDPlayers)
                {
                    PlayersToSpawnAsClassD.Add(ply);
                    ClassDsToSpawn -= 1;
                    bulkList.Remove(ply);
                }
            }
            else // More people voted than what is required, time to play the game of chance.
            {
                for (int x = 0; x < ClassDsToSpawn; x++)
                {
                    Player ply = ClassDPlayers[x];
                    PlayersToSpawnAsClassD.Add(ply);
                    ClassDPlayers.Remove(ply); // Removing winner from the list
                    bulkList.Remove(ply); // Removing the winners from the bulk list
                }

                ClassDsToSpawn = 0;
            }
        }

        // ---------------------------------------------------------------------------------------\\
        // Scientists
        if (ScientistsToSpawn != 0)
        {
            if (ScientistPlayers.Count <= ScientistsToSpawn) // Less people (or equal) voted than what is required in the game.
            {
                foreach (Player ply in ScientistPlayers)
                {
                    PlayersToSpawnAsScientist.Add(ply);
                    ScientistsToSpawn -= 1;
                    bulkList.Remove(ply);
                }
            }
            else // More people voted than what is required, time to play the game of chance.
            {
                for (int x = 0; x < ScientistsToSpawn; x++)
                {
                    Player ply = ScientistPlayers[x];
                    PlayersToSpawnAsScientist.Add(ply);
                    ScientistPlayers.Remove(ply); // Removing winner from the list
                    bulkList.Remove(ply); // Removing the winners from the bulk list
                }

                ScientistsToSpawn = 0;
            }
        }

        // ---------------------------------------------------------------------------------------\\
        // Guards
        if (GuardsToSpawn != 0)
        {
            if (GuardPlayers.Count <= GuardsToSpawn) // Less people (or equal) voted than what is required in the game.
            {
                foreach (Player ply in GuardPlayers)
                {
                    PlayersToSpawnAsGuard.Add(ply);
                    GuardsToSpawn -= 1;
                    bulkList.Remove(ply);
                }
            }
            else // More people voted than what is required, time to play the game of chance.
            {
                for (int x = 0; x < GuardsToSpawn; x++)
                {
                    Player ply = GuardPlayers[x];
                    PlayersToSpawnAsGuard.Add(ply);
                    GuardPlayers.Remove(ply); // Removing winner from the list
                    bulkList.Remove(ply); // Removing the winners from the bulk list
                }

                GuardsToSpawn = 0;
            }
        }

        // ---------------------------------------------------------------------------------------\\
        // SCPs
        if (SCPsToSpawn != 0)
        {
            if (SCPPlayers.Count <= SCPsToSpawn) // Less people (or equal) voted than what is required in the game.
            {
                foreach (Player ply in SCPPlayers)
                {
                    PlayersToSpawnAsSCP.Add(ply);
                    SCPsToSpawn -= 1;
                    bulkList.Remove(ply);
                }
            }
            else // More people voted than what is required, time to play the game of chance.
            {
                for (int x = 0; x < SCPsToSpawn; x++)
                {
                    Player ply = SCPPlayers[x];
                    PlayersToSpawnAsSCP.Add(ply);
                    SCPPlayers.Remove(ply); // Removing winner from the list
                    bulkList.Remove(ply); // Removing the winners from the bulk list
                }

                SCPsToSpawn = 0;
            }
        }

        // ---------------------------------------------------------------------------------------\\
        // ---------------------------------------------------------------------------------------\\
        // ---------------------------------------------------------------------------------------\\
        // ---------------------------------------------------------------------------------------\\

        // At this point we need to check for any blanks and fill them in via the bulk list guys
        for (int x = 0; x < ClassDsToSpawn; x++)
        {
            if (bulkList.Count == 0)
                break;

            Player ply = bulkList.RandomItem();
            PlayersToSpawnAsClassD.Add(ply);
            bulkList.Remove(ply); // Removing the winners from the bulk list
        }

        for (int x = 0; x < SCPsToSpawn; x++)
        {
            if (bulkList.Count == 0)
                break;

            Player ply = bulkList.RandomItem();
            PlayersToSpawnAsSCP.Add(ply);
            bulkList.Remove(ply); // Removing the winners from the bulk list
        }

        for (int x = 0; x < ScientistsToSpawn; x++)
        {
            if (bulkList.Count == 0)
                break;

            Player ply = bulkList.RandomItem();
            PlayersToSpawnAsScientist.Add(ply);
            bulkList.Remove(ply); // Removing the winners from the bulk list
        }

        for (int x = 0; x < GuardsToSpawn; x++)
        {
            if (bulkList.Count == 0)
                break;

            Player ply = bulkList.RandomItem();
            PlayersToSpawnAsGuard.Add(ply);
            bulkList.Remove(ply); // Removing the winners from the bulk list
        }
        // ---------------------------------------------------------------------------------------\\

        // Okay we have the list! Time to spawn everyone in, we'll leave SCP for last as it has a bit of logic.
        foreach (Player ply in PlayersToSpawnAsClassD)
        {
            RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.authManager.UserId);
            ply.SetRole(RoleTypeId.ClassD, RoleChangeReason.RoundStart);
        }

        foreach (Player ply in PlayersToSpawnAsScientist)
        {
            RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.authManager.UserId);
            ply.SetRole(RoleTypeId.Scientist, RoleChangeReason.RoundStart);
        }

        bool isChaos = Random.Range(1, 101) < Module.Config.ChaosProb;
        Queue<RoleTypeId> chaosQueue = new();
        SpawnableTeamHandlerBase chaosSpawnHandler = RespawnManager.SpawnableTeams[SpawnableTeamType.ChaosInsurgency];
        chaosSpawnHandler.GenerateQueue(chaosQueue, chaosSpawnHandler.MaxWaveSize);
        foreach (Player ply in PlayersToSpawnAsGuard)
        {
            if (isChaos)
                ply.SetRole(chaosQueue.Dequeue(), RoleChangeReason.RoundStart);
            else
                ply.SetRole(RoleTypeId.FacilityGuard, RoleChangeReason.RoundStart);

            RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.authManager.UserId);
        }

        // ---------------------------------------------------------------------------------------\\

        // SCP Logic, preventing SCP-079 from spawning if there isn't at least 2 other SCPs
        List<RoleTypeId> Roles = new() { RoleTypeId.Scp049, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939 };
        if (PlayersToSpawnAsSCP.Count > Module.Config.MinScpAmountFor079And3114)
        {
            if (Module.Config.Add3114)
                Roles.Add(RoleTypeId.Scp3114);

            Roles.Add(RoleTypeId.Scp079);
        }

        foreach (Player ply in PlayersToSpawnAsSCP)
        {
            RoleTypeId role = Roles.RandomItem();
            Roles.Remove(role);
            ply.SetRole(role, RoleChangeReason.RoundStart);
            RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.authManager.UserId);
        }

        global::RoundSummary.singleton.classlistStart.class_ds = PlayersToSpawnAsClassD.Count;
        global::RoundSummary.singleton.classlistStart.scps_except_zombies = PlayersToSpawnAsSCP.Count;
        if (isChaos)
            global::RoundSummary.singleton.classlistStart.chaos_insurgents = PlayersToSpawnAsGuard.Count;
        else
            global::RoundSummary.singleton.classlistStart.mtf_and_guards = PlayersToSpawnAsGuard.Count;

        global::RoundSummary.singleton.classlistStart.scientists = PlayersToSpawnAsScientist.Count;
        foreach (var ply in bulkList)
        {
            RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.authManager.UserId);
            ply.SetRole(RoleTypeId.ClassD, RoleChangeReason.RoundStart);
        }

        foreach (var prim in Primitives)
            NetworkServer.Destroy(prim.gameObject);
    }

    [PluginEvent(ServerEventType.WaitingForPlayers)]
    public void OnWaitingForPlayers()
    {
        SCPsToSpawn = 0;
        ClassDsToSpawn = 0;
        ScientistsToSpawn = 0;
        GuardsToSpawn = 0;

        SCPPlayers.Clear();
        ScientistPlayers.Clear();
        GuardPlayers.Clear();
        ClassDPlayers.Clear();

        Triggers.Clear();
        Primitives.Clear();

        GameObject.Find("StartRound").transform.localScale = Vector3.zero;
        SpawnMap();
    }

    [PluginEvent(ServerEventType.RoundEndConditionsCheck)]
    public RoundEndConditionsCheckCancellationData OnRoundEndConditionsCheck(bool baseGameConditionsSatisfied)
    {
        if (Round.Duration.Seconds < 15)
            return RoundEndConditionsCheckCancellationData.Override(false);

        return RoundEndConditionsCheckCancellationData.LeaveUnchanged();
    }

    private void SpawnMap()
    {
        Triggers.Add(SpawnTrigger(Team.ClassD, new Vector3(-4.1f, 994.6f, -4.9f)));
        Triggers.Add(SpawnTrigger(Team.FoundationForces, new Vector3(-4.1f, 994.6f, -10.85f)));
        Triggers.Add(SpawnTrigger(Team.SCPs, new Vector3(4.7f, 994.6f, -4.9f)));
        Triggers.Add(SpawnTrigger(Team.Scientists, new Vector3(4.7f, 994.6f, -10.85f)));
    }

    private static PrimitiveObjectToy primitiveBaseObject;
    public static PrimitiveObjectToy PrimitiveBaseObject
    {
        get
        {
            if (primitiveBaseObject is null)
            {
                foreach (GameObject gameObject in NetworkClient.prefabs.Values)
                {
                    if (gameObject.TryGetComponent(out PrimitiveObjectToy component))
                    {
                        primitiveBaseObject = component;
                        break;
                    }
                }
            }

            return primitiveBaseObject;
        }
    }

    private TeamTrigger SpawnTrigger(Team team, Vector3 pos)
    {
        PrimitiveObjectToy primitiveObjectToy = Object.Instantiate(PrimitiveBaseObject);

        primitiveObjectToy.transform.position = pos;
        primitiveObjectToy.transform.eulerAngles = Vector3.zero;
        primitiveObjectToy.transform.localScale = new Vector3(3, 0.1f, 3);

        primitiveObjectToy.NetworkScale = primitiveObjectToy.transform.localScale;
        primitiveObjectToy.NetworkPrimitiveType = PrimitiveType.Cylinder;
        primitiveObjectToy.NetworkMaterialColor = GetColor(team);

        NetworkServer.Spawn(primitiveObjectToy.gameObject);

        Primitives.Add(primitiveObjectToy);

        TeamTrigger tt = primitiveObjectToy.gameObject.AddComponent<TeamTrigger>();
        tt.Team = team;
        return tt;
    }

    private static Color GetColor(Team team)
    {
        return team switch
        {
            Team.FoundationForces => RoleTypeId.FacilityGuard.GetColor(),
            Team.SCPs => RoleTypeId.Scp049.GetColor(),
            Team.Scientists => RoleTypeId.Scientist.GetColor(),
            _ => RoleTypeId.ClassD.GetColor()
        };
    }
}