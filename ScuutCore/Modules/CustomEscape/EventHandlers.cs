using PluginAPI.Core;
using PluginAPI.Core.Items;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using InventorySystem.Disarming;
using Mirror;

namespace ScuutCore.Modules.CustomEscape
{
    public class EventHandlers
    {
        private CustomEscape customEscape;
        public EventHandlers(CustomEscape c)
        {
            customEscape = c;
        }

        public Vector3 EscapeZone = Vector3.zero;

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(BetterDisarm()));
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public bool OnEscaping(Player player, RoleTypeId newRole)
        {
            if (customEscape.Config.CuffedRoleConversions.ContainsKey(player.Role))
                return false;

            return true;
        }

        private IEnumerator<float> BetterDisarm()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1.5f);

                foreach (Player player in Player.GetPlayers())
                {
                    if (EscapeZone == Vector3.zero)
                        EscapeZone = Escape.WorldPos;

                    if (!player.ReferenceHub.inventory.IsDisarmed() || (player.Role.GetTeam() != Team.ChaosInsurgency && player.Role.GetTeam() != Team.FoundationForces) || (EscapeZone - player.Position).sqrMagnitude > 400f)
                        continue;

                    if (customEscape.Config.CuffedRoleConversions.TryGetValue(player.Role, out var role))
                    {
                        var itemList = new List<ItemType>();
                        foreach (var item in player.Items)
                            itemList.Add(item.ItemTypeId);

                        Plugin.Coroutines.Add(Timing.RunCoroutine(DropItems(player, itemList)));
                        player.SetRole(role);
                    }
                }
            }
        }

        private IEnumerator<float> DropItems(Player player, List<ItemType> items)
        {
            yield return Timing.WaitForSeconds(1f);

            foreach (ItemType item in items)
                ItemPickup.Create(item, player.Position, default);
        }
    }
}