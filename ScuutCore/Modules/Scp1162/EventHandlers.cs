namespace ScuutCore.Modules.Scp1162
{
    using API.Features;
    using InventorySystem;
    using InventorySystem.Items.Pickups;
    using InventorySystem.Items.Usables.Scp330;
    using Mirror;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<Scp1162>
    {
        private GameObject Scp1162gameObject;
        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
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

            if (player.CurrentItem == null)
                return false;
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

            var newItem = ItemType.None;
            getItem:
            foreach (var pair in Module.Config.Chances)
            {
                if (Plugin.Random.Next(0, 100) > pair.Value)
                    continue;
                newItem = pair.Key;
                break;
            }

            if (newItem == ItemType.None)
                goto getItem;

            player.AddItem(newItem);
            if (Module.Config.UseHints)
                player.ReceiveHint(Module.Config.ItemDropMessage, Module.Config.ItemDropMessageDuration);
            else
                player.SendBroadcast(Module.Config.ItemDropMessage, Module.Config.ItemDropMessageDuration);

            return false;
        }
    }
}