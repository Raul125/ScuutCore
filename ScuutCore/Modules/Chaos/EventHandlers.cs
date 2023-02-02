namespace ScuutCore.Modules.Chaos
{
    using MEC;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Respawning;
    using NorthwoodLib.Pools;
    using System.Text;

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
            if (team is SpawnableTeamType.ChaosInsurgency)
            {
                Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () =>
                {
                    StringBuilder announcement = StringBuilderPool.Shared.Rent();
                    string[] cassies = chaos.Config.ChaosCassie.Split('\n');
                    string[] translations = chaos.Config.CustomSubtitle.Split('\n');
                    for (int i = 0; i < cassies.Length; i++)
                        announcement.Append($"{translations[i].Replace(' ', ' ')}<size=0> {cassies[i]} </size><split>");

                    RespawnEffectsController.PlayCassieAnnouncement(announcement.ToString(), false, false, true);
                    StringBuilderPool.Shared.Return(announcement);
                }));
            }
        }
    }
}