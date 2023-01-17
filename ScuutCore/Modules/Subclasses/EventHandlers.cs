namespace ScuutCore.Modules.Subclasses
{
    using PlayerRoles;
    using PlayerStatsSystem;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.API.Features;
    using ScuutCore.API.Helpers;

    public class EventHandlers
    {
        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerJoin(Player player)
        {
            player.GameObject.AddComponent<SubclassComponent>();
        }

        [PluginEvent(ServerEventType.PlayerSpawn)]
        public void OnSpawn(Player player, RoleTypeId roleTypeId)
        {
            foreach (var subclass in Subclass.List)
            {
                if(!subclass.ToReplace.Contains(roleTypeId))
                    continue;
                if(subclass.MaxAlive <= subclass.GetPlayers().Count)
                    continue;
                if(UnityEngine.Random.Range(0f, 100f) <= subclass.SpawnChance)
                    player.SetSubclass(subclass);
            }
        }

        public void OnDied(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if(player == null)
                return;
            if (player.TryGetSubclass(out _))
            {
                player.RemoveSubclass();
            }
        }
    }
}