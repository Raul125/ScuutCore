using PluginAPI.Core;
using MEC;
using System.Collections.Generic;
using System.Linq;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Core.Items;
using InventorySystem.Items.Pickups;
using PlayerRoles.PlayableScps.Scp079;

namespace ScuutCore.Modules.CleanupUtility
{
    using System;
    using MapGeneration;
    using Object = UnityEngine.Object;

    public class EventHandlers
    {
        private CleanupUtility cleanupUtility;
        public EventHandlers(CleanupUtility cln)
        {
            cleanupUtility = cln;
        }

        /*public void OnRoundStart()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(CleanupCoroutine()));
        }*/

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
                    {
                        continue;
                    }
                }
                else
                {
                    if(RoomIdUtils.RoomAtPosition(item._transform.position)?.Zone != FacilityZone.LightContainment)
                    {
                        continue;
                    }
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
            {
                Log.Warning($"{errorcount} errors occured while trying to delete items.");
            }
        }

        /*public void OnDetonated()
        {
            foreach (var item in Pickup.List)
            {
                if (item == null)
                    continue;

                var room = Map.FindParentRoom(item.GameObject);
                if (room != null && room.Zone != Exiled.API.Enums.ZoneType.Surface)
                    item.Destroy();
            }
        }

        private IEnumerator<float> CleanupCoroutine()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(cleanupUtility.Config.CleanDelay);

                if (cleanupUtility.Config.DestroyRagdolls)
                {
                    foreach (var ragdoll in Map.Ragdolls.Where(x => x != null))
                        ragdoll.Delete();
                }

                if (cleanupUtility.Config.ClearItems)
                {
                    foreach (var item in Pickup.List.Where(x => x != null))
                        item.Destroy();
                }
            }
        }*/
    }
}