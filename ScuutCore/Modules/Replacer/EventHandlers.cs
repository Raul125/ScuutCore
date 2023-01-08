using PluginAPI.Core;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PlayerRoles.PlayableScps.Scp079;
using UnityEngine;
using RemoteAdmin.Communication;

namespace ScuutCore.Modules.Replacer
{
    public class EventHandlers
    {
        private Replacer replacer;
        public EventHandlers(Replacer btc)
        {
            replacer = btc;
        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        public void OnDestroying(Player player)
        {
            if (global::RoundSummary.singleton._roundEnded || !Round.IsRoundStarted || Round.Duration.TotalSeconds > replacer.Config.DontReplaceTime 
                || replacer.Config.DisallowedRolesToReplace.Contains(player.Role))
                return;

            if (player.IsAlive)
            {
                int level = 0;
                if (player.ReferenceHub.roleManager._curRole is Scp079Role scp079)
                    level = scp079.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability) ? ability.AccessTierLevel : 0;

                Vector3 oldPos = player.Position;
                RoleTypeId oldRole = player.Role;
                float oldHealth = player.Health;
                List<ItemType> oldItems = new List<ItemType>();
                foreach (var item in player.Items)
                    oldItems.Add(item.ItemTypeId);
                player.ClearInventory();

                Player randomSpec = Player.GetPlayers().FirstOrDefault(x => x.Role is RoleTypeId.Spectator && !x.IsOverwatchEnabled);
                if (randomSpec != null)
                {
                    replacer.Config.BroadCast.Show(randomSpec);
                    randomSpec.SetRole(oldRole);
                    Timing.CallDelayed(1f, () =>
                    {
                        if (randomSpec.ReferenceHub.roleManager._curRole is Scp079Role scp079role)
                        {
                            if (!scp079role.SubroutineModule.TryGetSubroutine(out Scp079TierManager ability))
                                return;

                            ability.TotalExp = level <= 1 ? 0 : ability.AbsoluteThresholds[Mathf.Clamp(level - 2, 0, ability.AbsoluteThresholds.Length - 1)];
                            return;
                        }

                        randomSpec.Position = oldPos;
                        randomSpec.Health = oldHealth;
                        foreach (var item in oldItems)
                            randomSpec.AddItem(item);
                    });
                }
            }
        }
    }
}