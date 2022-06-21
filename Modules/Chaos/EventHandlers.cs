using Exiled.Events.EventArgs;
using Exiled.API.Features;

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
                Cassie.Message(chaos.Config.ChaosCassie, false, false, false);
        }
    }
}