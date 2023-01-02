using PluginAPI.Core;
using MEC;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Respawning;

namespace ScuutCore.Modules.Chaos
{
    public class EventHandlers
    {
        private Chaos chaos;
        public EventHandlers(Chaos ch)
        {
            chaos = ch;
        }

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnRespawningTeam(SpawnableTeamType team)
        {
            if (team == SpawnableTeamType.ChaosInsurgency)
                Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () => Cassie.Message(chaos.Config.ChaosCassie, false, false, false)));
        }
    }
}