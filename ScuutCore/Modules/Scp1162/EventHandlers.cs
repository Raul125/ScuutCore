namespace ScuutCore.Modules.Scp1162
{
    using PluginAPI.Core;
    using PlayerRoles;
    using UnityEngine;
    using InventorySystem.Items;
    using InventorySystem;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PlayerRoles.FirstPersonControl.Spawnpoints;

    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool OnDroppingItem(Player player, ItemBase item)
        {
            try
            {
                RoleSpawnpointManager.TryGetSpawnpointForRole(RoleTypeId.Scp173, out ISpawnpointHandler spawnpoint);
                spawnpoint.TryGetSpawnpoint(out Vector3 position, out float horizontalRotation);

                if (Vector3.Distance(player.Position, position) <= 8.2f)
                {
                    if (Scp1162.Instance.Config.UseHints)
                        player.ReceiveHint(Scp1162.Instance.Config.ItemDropMessage, Scp1162.Instance.Config.ItemDropMessageDuration);
                    else
                        player.SendBroadcast(Scp1162.Instance.Config.ItemDropMessage, Scp1162.Instance.Config.ItemDropMessageDuration, Broadcast.BroadcastFlags.Normal, true);

                    player.ReferenceHub.inventory.ServerRemoveItem(item.ItemSerial, item.PickupDropModel);
                    ItemType newItemType = Scp1162.Instance.Config.Chances.RandomItem();
                    PluginAPI.Core.Items.ItemPickup.Create(newItemType, player.Position, Quaternion.identity);
                    return false;
                }
            }
            catch
            {
            }

            return true;
        }
    }
}