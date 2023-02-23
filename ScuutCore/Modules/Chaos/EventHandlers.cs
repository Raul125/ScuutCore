namespace ScuutCore.Modules.Chaos
{
    using ScuutCore.API.Features;
    using MEC;
    using NorthwoodLib.Pools;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using Respawning;

    public sealed class EventHandlers : InstanceBasedEventHandler<Chaos>
    {

        [PluginEvent(ServerEventType.TeamRespawn)]
        public void OnRespawningTeam(SpawnableTeamType team)
        {
            if (team is not SpawnableTeamType.ChaosInsurgency)
                return;
            Plugin.Coroutines.Add(Timing.CallDelayed(Module.Config.CassieDelay, PlayAnnouncement));
        }

        private void PlayAnnouncement()
        {
            var announcement = StringBuilderPool.Shared.Rent();
            string[] cassie = Module.Config.ChaosCassie.Split('\n');
            string[] translations = Module.Config.CustomSubtitle.Split('\n');
            for (int i = 0; i < cassie.Length; i++)
                announcement.Append($"{translations[i].Replace(' ', ' ')}<size=0> {cassie[i]} </size><split>");

            RespawnEffectsController.PlayCassieAnnouncement(announcement.ToString(), false, false, true);
            StringBuilderPool.Shared.Return(announcement);
        }
    }
}