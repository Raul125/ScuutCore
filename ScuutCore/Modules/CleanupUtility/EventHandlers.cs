namespace ScuutCore.Modules.CleanupUtility
{
    using API.Features;
    using InventorySystem.Items.Pickups;
    using MapGeneration;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using UnityEngine;

    public sealed class EventHandlers : InstanceBasedEventHandler<CleanupUtility>
    {
        [PluginEvent(ServerEventType.LczDecontaminationStart)]
        public void OnDecontaminating()
        {
            int errorcount = 0;
            foreach (var item in Object.FindObjectsOfType<ItemPickupBase>())
            {
                if (item == null)
                    continue;

                if (Module.Config.UseFastZoneCheck)
                {
                    if (item._transform.position.y is < -200 or > 200)
                        continue;
                }
                else
                {
                    if (RoomIdUtils.RoomAtPosition(item._transform.position)?.Zone != FacilityZone.LightContainment)
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
    }
}