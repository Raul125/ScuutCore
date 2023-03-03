namespace ScuutCore.Modules.Replacer
{
    using System.Collections.Generic;
    using System.Linq;
    using ScuutCore.API.Features;
    using MEC;
    using PlayerRoles;
    using PlayerRoles.PlayableScps.Scp079;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;
    public sealed class EventHandlers : InstanceBasedEventHandler<Replacer>
    {
        [PluginEvent(ServerEventType.PlayerLeft)]
        public void OnDestroying(Player player)
        {
            if (global::RoundSummary.singleton._roundEnded || !Round.IsRoundStarted 
                || Round.Duration.TotalSeconds > Module.Config.DontReplaceTime || Module.Config.DisallowedRolesToReplace.Contains(player.Role))
                return;

            if (!player.IsAlive)
                return;

            int level = 0;
            if (player.ReferenceHub.roleManager._curRole is Scp079Role scp079)
                level = scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability) ? ability.AccessTierLevel : 0;

            var oldPos = player.Position;
            var oldRole = player.Role;
            float oldHealth = player.Health;
            var oldItems = new List<ItemType>();
            foreach (var item in player.Items)
                oldItems.Add(item.ItemTypeId);

            player.ClearInventory();

            var randomSpec = Player.GetPlayers().FirstOrDefault(x => x.Role is RoleTypeId.Spectator && !x.IsOverwatchEnabled);
            if (randomSpec == null)
                return;

            Module.Config.BroadCast.Show(randomSpec);
            randomSpec.SetRole(oldRole);
            Plugin.Coroutines.Add(Timing.CallDelayed(1f, () =>
            {
                if (randomSpec.ReferenceHub.roleManager._curRole is Scp079Role role)
                {
                    if (!role.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability))
                        return;

                    ability.TotalExp = level <= 1 ? 0 : ability.AbsoluteThresholds[Mathf.Clamp(level - 2, 0, ability.AbsoluteThresholds.Length - 1)];
                    return;
                }

                randomSpec.Position = oldPos;
                randomSpec.Health = oldHealth;
                foreach (var item in oldItems)
                    randomSpec.AddItem(item);
            }));
        }
    }
}