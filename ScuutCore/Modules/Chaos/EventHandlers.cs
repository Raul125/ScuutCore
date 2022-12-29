using Exiled.Events.EventArgs;
using Exiled.API.Features;
using MEC;
using Exiled.Events.EventArgs.Server;

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
            {
                if (chaos.Config.CustomSubtitle == "")
                    Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () => Cassie.Message(chaos.Config.ChaosCassie, false, false, false)));
                else
                    Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () => Cassie.MessageTranslated(chaos.Config.ChaosCassie, chaos.Config.CustomSubtitle, false, false, true)));
            }
        }
    }
}