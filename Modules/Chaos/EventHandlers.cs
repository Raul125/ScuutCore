using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;

namespace ScuutCore.Modules.Chaos
{
    public class EventHandlers
    {
        private Chaos chaos;
        public EventHandlers(Chaos ch)
        {
            chaos = ch;
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == Respawning.SpawnableTeamType.ChaosInsurgency)
                Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () => Cassie.Message(chaos.Config.ChaosCassie, false, false, false)));
        }
    }
}