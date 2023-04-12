namespace ScuutCore.Modules.CleanupUtility
{
    using InventorySystem.Items;
    using ScuutCore.API.Features;
    using InventorySystem.Items.Pickups;
    using MapGeneration;
    using Mirror;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using ScuutCore.Modules.CleanupUtility.Components;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<CleanupUtility>
    {
        [PluginEvent(ServerEventType.LczDecontaminationStart)]
        public void OnDecontaminating()
        {
            if (Module.Config.ClearItems)
                ClearItems(-200, 200, new[] { FacilityZone.LightContainment });
            if (Module.Config.ClearRagdolls)
                ClearRagdolls(-200, 200, new[] { FacilityZone.LightContainment });
        }

        [PluginEvent(ServerEventType.WarheadDetonation)]
        public void OnWarheadDetonated()
        {
            if (Module.Config.ClearItems)
                ClearItems(-1500, 500, new[] { FacilityZone.LightContainment, FacilityZone.HeavyContainment, FacilityZone.Entrance });
            if (Module.Config.ClearRagdolls)
                ClearRagdolls(-1500, 500, new[] { FacilityZone.LightContainment, FacilityZone.HeavyContainment, FacilityZone.Entrance });
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public void OnPlayerDroppedItem(Player player, ItemBase item)
        {
            if (Module.Config.ClearItems)
            {
                item.gameObject.AddComponent<AutoClearComponent>();
                var time = Module.Config.CleanDelay.TryGetValue(item.ItemTypeId, out var value) ? value : Module.Config.DefaultDelay;
                var comp = item.gameObject.GetComponent<AutoClearComponent>();
                comp.TimeLeft = time;
                comp.Enabled = true;
            }
        }

        private void ClearItems(int ymin, int ymax, FacilityZone[] zones)
        {
            int errorcount = 0;
            foreach (var item in Object.FindObjectsOfType<ItemPickupBase>())
            {
                if (item == null)
                    continue;

                if (Module.Config.UseFastZoneCheck)
                {
                    if (item._transform.position.y < ymin || item._transform.position.y > ymax)
                        continue;
                }
                else
                {
                    if (!zones.Contains(RoomIdUtils.RoomAtPosition(item._transform.position)?.Zone ?? FacilityZone.None))
                        continue;
                }

                try
                {
                    item.DestroySelf();
                }
                catch
                {
                    errorcount++;
                }
            }

            if (errorcount > 0)
                Log.Warning($"{errorcount} errors occured while trying to delete items.");
        }

        private void ClearRagdolls(int ymin, int ymax, FacilityZone[] zones)
        {
            int errorcount = 0;
            foreach (var item in Object.FindObjectsOfType<BasicRagdoll>())
            {
                if (item == null || item._cleanedUp)
                    continue;

                if (Module.Config.UseFastZoneCheck)
                {
                    if (item._originPoint.position.y < ymin || item._originPoint.position.y > ymax)
                        continue;
                }
                else
                {
                    if (!zones.Contains(RoomIdUtils.RoomAtPosition(item._originPoint.position)?.Zone ?? FacilityZone.None))
                        continue;
                }

                try
                {
                    NetworkServer.Destroy(item.gameObject);
                }
                catch
                {
                    errorcount++;
                }
            }

            if (errorcount > 0)
                Log.Warning($"{errorcount} errors occured while trying to clear ragdolls.");
        }
    }
}