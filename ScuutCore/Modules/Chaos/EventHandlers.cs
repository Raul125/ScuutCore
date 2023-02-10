namespace ScuutCore.Modules.Chaos
{
    using API.Features;
    using MEC;
    using NorthwoodLib.Pools;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Respawning;

    public sealed class EventHandlers : IInstanceBasedEventHandler<Chaos>
    {
        private Chaos chaos;

        public void AssignModule(Chaos module)
        {
            chaos = module;
        }

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnRespawningTeam(SpawnableTeamType team)
        {
            if (team is not SpawnableTeamType.ChaosInsurgency)
                return;
            Plugin.Coroutines.Add(Timing.CallDelayed(chaos.Config.CassieDelay, () =>
            {
                var announcement = StringBuilderPool.Shared.Rent();
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