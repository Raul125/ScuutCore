
namespace ScuutCore.Modules.CleanupUtility
{
    using MapGeneration;
    using Object = UnityEngine.Object;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using InventorySystem.Items.Pickups;

    public class EventHandlers
    {
        private CleanupUtility cleanupUtility;
        public EventHandlers(CleanupUtility cln)
        {
            cleanupUtility = cln;
        }

        [PluginEvent(ServerEventType.LczDecontaminationStart)]
        public void OnDecontaminating()
        {
            int errorcount = 0;
            foreach (var item in Object.FindObjectsOfType<ItemPickupBase>())
            {
                if (item == null)
                    continue;

                if (cleanupUtility.Config.UseFastZoneCheck)
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