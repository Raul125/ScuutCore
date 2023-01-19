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
    using Mirror;
    using PluginAPI.Events;
    using InventorySystem.Items.Pickups;
    using InventorySystem.Items.Usables.Scp330;

    public class EventHandlers
    {
        public EventHandlers()
        {
        }

        private GameObject Scp1162gameObject = null;
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            Scp1162gameObject = null;
            var item = PluginAPI.Core.Items.ItemPickup.Create(ItemType.SCP500, Vector3.zero, default);
            Scp1162gameObject = item.GameObject;
            NetworkServer.UnSpawn(item.GameObject);

            item.Rigidbody.transform.parent = GameObject.Find("LCZ_173").transform;
            item.Rigidbody.useGravity = false;
            item.Rigidbody.drag = 0f;
            item.Rigidbody.freezeRotation = true;
            item.Rigidbody.isKinematic = true;
            item.GameObject.transform.localPosition = new Vector3(17f, 13.1f, 3f);
            item.GameObject.transform.localRotation = Quaternion.Euler(90, 1, 0);
            item.GameObject.transform.localScale = new Vector3(10, 10, 10);
            NetworkServer.Spawn(item.GameObject);
        }

        [PluginEvent(ServerEventType.PlayerSearchPickup)]
        public bool OnPlayerPickup(Player player, ItemPickupBase item)
        {
            if (item.gameObject != Scp1162gameObject)
                return true;

            if (player.CurrentItem != null)
            {
                if (player.CurrentItem.ItemTypeId == ItemType.SCP330)
                {
                    var bag = player.CurrentItem as Scp330Bag;
                    if (bag == null || bag.Candies.Count < 1)
                    {
                        return false;
                    }

                    bag.SelectCandy(0);
                    var removed = bag.TryRemove(0);
                    if (removed == CandyKindID.None)
                        return false;
                }
                else
                    player.ReferenceHub.inventory.ServerRemoveItem(player.CurrentItem.ItemSerial, player.CurrentItem.PickupDropModel);

                ItemType newItem = ItemType.None;

                getItem:
                foreach (var itemd in Scp1162.Instance.Config.Chances)
                {
                    if (Plugin.Random.Next(0, 100) <= itemd.Value)
                    {
                        newItem = itemd.Key;
                        break;
                    }
                }

                if (newItem == ItemType.None)
                    goto getItem;

                player.AddItem(newItem);

                if (Scp1162.Instance.Config.UseHints)
                    player.ReceiveHint(Scp1162.Instance.Config.ItemDropMessage, Scp1162.Instance.Config.ItemDropMessageDuration);
                else
                    player.SendBroadcast(Scp1162.Instance.Config.ItemDropMessage, Scp1162.Instance.Config.ItemDropMessageDuration);
            }

            return false;
        }
    }
}