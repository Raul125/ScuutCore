namespace ScuutCore.Modules.RespawnFix
{
    using System.Collections.Generic;
    using System.Linq;
    using ScuutCore.API.Features;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;
    using Respawning;
    using Respawning.NamingRules;

    public sealed class EventHandlers : InstanceBasedEventHandler<RespawnFix>
    {
        [PluginEvent(ServerEventType.TeamRespawn)]
        public bool OnRespawningTeam(TeamRespawnEvent e)
        {
            var team = e.Team;
            if (!Warhead.IsDetonated)
                return true;
            List<Player> _players = Player.GetPlayers().Where(x => x.Role is RoleTypeId.Spectator).ToList();
            RespawnManager.SpawnableTeams.TryGetValue(team, out var spawnableTeamHandlerBase);
            Queue<RoleTypeId> queue = new();
            spawnableTeamHandlerBase.GenerateQueue(queue, _players.Count);

            if (_players.Count > spawnableTeamHandlerBase.MaxWaveSize)
            {
                _players.ShuffleList();
                _players.RemoveRange(spawnableTeamHandlerBase.MaxWaveSize, _players.Count - spawnableTeamHandlerBase.MaxWaveSize);
            }

            if (_players.Count > 0 && UnitNamingRule.TryGetNamingRule(team, out var rule))
                UnitNameMessageHandler.SendNew(team, rule);

            foreach (Player ply in _players)
            {
                RoleTypeId newRole = queue.Dequeue();
                ply.SetRole(newRole, RoleChangeReason.Respawn);
            }

            return false;

        }
    }
}