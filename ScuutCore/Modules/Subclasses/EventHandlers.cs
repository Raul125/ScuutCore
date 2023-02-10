namespace ScuutCore.Modules.Subclasses
{
    using System.Collections.Generic;
    using System.Linq;
    using API.Features;
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

        [PluginEvent(ServerEventType.PlayerSpawn)]
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
        }

        [PluginEvent(ServerEventType.PlayerDeath)]
        public void OnDied(ScuutPlayer player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (player == null)
                return;

            if (player.SubClass is not null)
                player.SubClass = null;
        }
    }
}