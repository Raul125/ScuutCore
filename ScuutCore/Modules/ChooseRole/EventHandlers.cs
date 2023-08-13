namespace ScuutCore.Modules.ChooseRole
{
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
    using ScuutCore.API.Extensions;
    using PluginAPI.Events;

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

        private readonly HashSet<TeamTrigger> _triggers = new();

        private Vector3 lobbyPos = new(0.82f, 995.38f, -7.92f);

        public List<PrimitiveObjectToy> Primitives = new();
        private static PrimitiveObjectToy primitiveBaseObject;
        private static PrimitiveObjectToy PrimitiveBaseObject
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

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerJoin(Player player)
        {
            if (Round.IsRoundStarted)
                return;

            player.GameObject.AddComponent<HudComponent>().Init(player.ReferenceHub);
            player.Role = RoleTypeId.Tutorial;
            player.Position = lobbyPos;
            player.ClearInventory();
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
            var bulkList = Player.GetPlayers();
            bulkList.RemoveAll(x => x.IsOverwatchEnabled);
            var spCount = Module.Config.SpawnQueue.Count();
            for (int x = 0; x < bulkList.Count; x++)
            {
                if (x >= spCount)
                {
                    ClassDsToSpawn += 1;
                    continue;
                }
                switch (Module.Config.SpawnQueue[x])
                {
                    case '4':
                        ClassDsToSpawn += 1;
                        break;
                    case '0':
                        SCPsToSpawn += 1;
                        break;
                    case '1':
                        GuardsToSpawn += 1;
                        break;
                    case '3':
                        ScientistsToSpawn += 1;
                        break;
                }
            }

            bulkList.ShuffleList();

            List<Player> PlayersToSpawnAsSCP = new();
            List<Player> PlayersToSpawnAsScientist = new();
            List<Player> PlayersToSpawnAsGuard = new();
            List<Player> PlayersToSpawnAsClassD = new();

            ClassDPlayers = _triggers.First(x => x.Team is Team.ClassD).Players.Where(x => x != null && x.GameObject != null).ToList();
            GuardPlayers = _triggers.First(x => x.Team is Team.FoundationForces).Players.Where(x => x != null && x.GameObject != null).ToList();
            ScientistPlayers = _triggers.First(x => x.Team is Team.Scientists).Players.Where(x => x != null && x.GameObject != null).ToList();
            SCPPlayers = _triggers.First(x => x.Team is Team.SCPs).Players.Where(x => x != null && x.GameObject != null).ToList();

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
                        Player Ply = ClassDPlayers.RandomItem();
                        PlayersToSpawnAsClassD.Add(Ply);
                        ClassDPlayers.Remove(Ply); // Removing winner from the list
                        bulkList.Remove(Ply); // Removing the winners from the bulk list
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
                        Player Ply = ScientistPlayers.RandomItem();
                        PlayersToSpawnAsScientist.Add(Ply);
                        ScientistPlayers.Remove(Ply); // Removing winner from the list
                        bulkList.Remove(Ply); // Removing the winners from the bulk list
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
                        Player Ply = GuardPlayers.RandomItem();
                        PlayersToSpawnAsGuard.Add(Ply);
                        GuardPlayers.Remove(Ply); // Removing winner from the list
                        bulkList.Remove(Ply); // Removing the winners from the bulk list
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
                        Player Ply = SCPPlayers.RandomItem();
                        SCPPlayers.Remove(Ply);
                        PlayersToSpawnAsSCP.Add(Ply); // Removing winner from the list
                        bulkList.Remove(Ply); // Removing the winners from the bulk list
                    }

                    SCPsToSpawn = 0;
                }
            }
            // ---------------------------------------------------------------------------------------\\
            // ---------------------------------------------------------------------------------------\\
            // ---------------------------------------------------------------------------------------\\
            // ---------------------------------------------------------------------------------------\\

            // At this point we need to check for any blanks and fill them in via the bulk list guys
            if (ClassDsToSpawn != 0)
            {
                for (int x = 0; x < ClassDsToSpawn; x++)
                {
                    Player Ply = bulkList.RandomItem();
                    PlayersToSpawnAsClassD.Add(Ply);
                    bulkList.Remove(Ply); // Removing the winners from the bulk list
                }
            }

            if (SCPsToSpawn != 0)
            {
                for (int x = 0; x < SCPsToSpawn; x++)
                {
                    Player Ply = bulkList.RandomItem();
                    PlayersToSpawnAsSCP.Add(Ply);
                    bulkList.Remove(Ply); // Removing the winners from the bulk list
                }
            }

            if (ScientistsToSpawn != 0)
            {
                for (int x = 0; x < ScientistsToSpawn; x++)
                {
                    Player Ply = bulkList.RandomItem();
                    PlayersToSpawnAsScientist.Add(Ply);
                    bulkList.Remove(Ply); // Removing the winners from the bulk list
                }
            }

            if (GuardsToSpawn != 0)
            {
                for (int x = 0; x < GuardsToSpawn; x++)
                {
                    Player Ply = bulkList.RandomItem();
                    PlayersToSpawnAsGuard.Add(Ply);
                    bulkList.Remove(Ply); // Removing the winners from the bulk list
                }
            }
            // ---------------------------------------------------------------------------------------\\

            // Okay we have the list! Time to spawn everyone in, we'll leave SCP for last as it has a bit of logic.
            foreach (Player ply in PlayersToSpawnAsClassD)
                ply.SetRole(RoleTypeId.ClassD, RoleChangeReason.RoundStart);

            foreach (Player ply in PlayersToSpawnAsScientist)
                ply.SetRole(RoleTypeId.Scientist, RoleChangeReason.RoundStart);

            bool isChaos = Random.Range(0, 100) < Module.Config.ChaosProb;
            Queue<RoleTypeId> chaosQueue = new();
            SpawnableTeamHandlerBase chaosSpawnHandler = RespawnManager.SpawnableTeams[SpawnableTeamType.ChaosInsurgency];
            chaosSpawnHandler.GenerateQueue(chaosQueue, chaosSpawnHandler.MaxWaveSize);

            foreach (Player ply in PlayersToSpawnAsGuard)
            {
                if (isChaos)
                    ply.SetRole(chaosQueue.Dequeue(), RoleChangeReason.RoundStart);
                else
                    ply.SetRole(RoleTypeId.FacilityGuard, RoleChangeReason.RoundStart);
            }

            // ---------------------------------------------------------------------------------------\\

            // SCP Logic, preventing SCP-079 from spawning if there isn't at least 2 other SCPs
            List<RoleTypeId> Roles = new() { RoleTypeId.Scp049, RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939 };

            if (PlayersToSpawnAsSCP.Count > Module.Config.MinScpAmountFor079)
                Roles.Add(RoleTypeId.Scp079);

            foreach (Player ply in PlayersToSpawnAsSCP)
            {
                RoleTypeId role = Roles.RandomItem();
                Roles.Remove(role);
                ply.SetRole(role, RoleChangeReason.RoundStart);
            }

            global::RoundSummary.singleton.classlistStart.class_ds = PlayersToSpawnAsClassD.Count;
            global::RoundSummary.singleton.classlistStart.scps_except_zombies = PlayersToSpawnAsSCP.Count;

            if (isChaos)
                global::RoundSummary.singleton.classlistStart.chaos_insurgents = PlayersToSpawnAsGuard.Count;
            else
                global::RoundSummary.singleton.classlistStart.mtf_and_guards = PlayersToSpawnAsGuard.Count;

            global::RoundSummary.singleton.classlistStart.scientists = PlayersToSpawnAsScientist.Count;

            RoleAssigner._spawned = true;
            RoleAssigner.LateJoinTimer.Restart();
            foreach (var ply in bulkList)
                RoleAssigner.AlreadySpawnedPlayers.Add(ply.ReferenceHub.characterClassManager.UserId);

            foreach (var prim in _triggers)
            {
                NetworkServer.UnSpawn(prim.gameObject);
                Object.Destroy(prim);
            }

            _triggers.Clear();
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
            _triggers.Clear();

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
            _triggers.Add(SpawnTrigger(Team.ClassD, new Vector3(-4.1f, 994.6f, -4.9f)));
            _triggers.Add(SpawnTrigger(Team.FoundationForces, new Vector3(-4.1f, 994.6f, -10.85f)));
            _triggers.Add(SpawnTrigger(Team.SCPs, new Vector3(4.7f, 994.6f, -4.9f)));
            _triggers.Add(SpawnTrigger(Team.Scientists, new Vector3(4.7f, 994.6f, -10.85f)));
        }

        private TeamTrigger SpawnTrigger(Team team, Vector3 pos)
        {
            PrimitiveObjectToy primitiveObjectToy = Object.Instantiate(PrimitiveBaseObject);

            primitiveObjectToy.transform.position = pos;
            primitiveObjectToy.transform.eulerAngles = Vector3.zero;
            primitiveObjectToy.transform.localScale = new Vector3(3, 0.1f, 3);
            primitiveObjectToy.NetworkScale = primitiveObjectToy.transform.localScale;
            primitiveObjectToy.NetworkPrimitiveType = PrimitiveType.Cylinder;
            primitiveObjectToy.NetworkMaterialColor = getColor(team);

            NetworkServer.Spawn(primitiveObjectToy.gameObject);

            TeamTrigger tt = primitiveObjectToy.gameObject.AddComponent<TeamTrigger>();
            tt.Team = team;
            return tt;
        }

        private static Color getColor(Team team)
        {
            switch (team)
            {
                case Team.FoundationForces:
                    return RoleTypeId.FacilityGuard.GetColor();
                case Team.SCPs:
                    return RoleTypeId.Scp049.GetColor();
                case Team.Scientists:
                    return RoleTypeId.Scientist.GetColor();
                default: return RoleTypeId.ClassD.GetColor();
            }
        }
    }
}