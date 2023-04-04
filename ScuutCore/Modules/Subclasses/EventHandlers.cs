namespace ScuutCore.Modules.Subclasses
{
    using System.Collections.Generic;
    using System.Linq;
    using MEC;
    using ScuutCore.API.Features;
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using RoundSummary = global::RoundSummary;

    public sealed class EventHandlers : IEventHandler
    {
        private Dictionary<Subclass, int> _subclassesSpawned = new Dictionary<Subclass, int>();

        [PluginEvent(ServerEventType.RoundEnd)]
        public void RoundEnd(RoundSummary.LeadingTeam leadingTeam)
        {
            _subclassesSpawned.Clear();
        }

        // scroll down
        public void OnSpawn(ScuutPlayer player, RoleTypeId roleTypeId)
        {
            foreach (var subclass in Subclass.List)
            {
                if (!subclass.ToReplace.Contains(roleTypeId))
                    continue;

                if (subclass.MaxAlive != -1 && subclass.MaxAlive <= subclass.GetPlayers().Count)
                    continue;

                if (subclass.MaxPerRound != -1)
                {
                    if (!_subclassesSpawned.ContainsKey(subclass))
                        _subclassesSpawned.Add(subclass, 0);

                    if (subclass.MaxPerRound <= _subclassesSpawned[subclass])
                        continue;

                    _subclassesSpawned[subclass]++;
                }

                if (!(UnityEngine.Random.Range(0f, 100f) <= subclass.SpawnChance))
                    continue;
                player.SubClass = subclass;
                return;
            }
            if (Subclasses.Singleton.Config.GiveItemsToNonSubclasses && Subclasses.Singleton.Config.ChanceForItems >= UnityEngine.Random.Range(1, 100))
            {
                Timing.CallDelayed(1f, () =>
                {
                    foreach (var item in Subclasses.Singleton.Config.Items)
                    {
                        player.AddItem(item);
                    }
                });
            }
        }

        [PluginEvent(ServerEventType.PlayerChangeRole)]
        public void OnDiedOnChangingRole(ScuutPlayer player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
        {
            if (player == null)
                return;
            if (Subclasses.Singleton.Config.RoleBlacklist.Contains(newRole) || Subclasses.Singleton.Config.SubclassSpawnReasonBlacklist.Contains(changeReason))
            {
                if (player.SubClass is not null)
                    player.SubClass = null;
                return;
            }

            OnSpawn(player, newRole);
        }
    }
}