namespace ScuutCore.Modules.CustomEscape
{
    using System.Collections.Generic;
    using ScuutCore.API.Features;
    using InventorySystem.Disarming;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Core.Items;
    using PluginAPI.Enums;
    using UnityEngine;
    public sealed class EventHandlers : InstanceBasedEventHandler<CustomEscape>
    {
        public Vector3 EscapeZone = Vector3.zero;

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(BetterDisarm()));
        }

        [PluginEvent(ServerEventType.PlayerEscape)]
        public bool OnEscaping(Player player, RoleTypeId newRole) => !Module.Config.CuffedRoleConversions.ContainsKey(player.Role);

        private IEnumerator<float> BetterDisarm()
        {
            for (;;)
            {
                yield return Timing.WaitForSeconds(1.5f);

                foreach (Player player in Player.GetPlayers())
                {
                    if (EscapeZone == Vector3.zero)
                        EscapeZone = Escape.WorldPos;

                    if (!player.ReferenceHub.inventory.IsDisarmed() || (player.Role.GetTeam() != Team.ChaosInsurgency && player.Role.GetTeam() != Team.FoundationForces) || (EscapeZone - player.Position).sqrMagnitude > 400f)
                        continue;

                    if (!Module.Config.CuffedRoleConversions.TryGetValue(player.Role, out var role))
                        continue;
                    var itemList = new List<ItemType>();
                    foreach (var item in player.Items)
                        itemList.Add(item.ItemTypeId);

                    Plugin.Coroutines.Add(Timing.RunCoroutine(DropItems(player, itemList)));
                    player.SetRole(role);
                }
            }
        }

        private static IEnumerator<float> DropItems(Player player, List<ItemType> items)
        {
            yield return Timing.WaitForSeconds(1f);
            foreach (var item in items)
                ItemPickup.Create(item, player.Position, default);
        }
    }
}