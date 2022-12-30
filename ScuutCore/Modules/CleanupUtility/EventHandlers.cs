using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Map;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace ScuutCore.Modules.CleanupUtility
{
    public class EventHandlers
    {
        private CleanupUtility cleanupUtility;
        public EventHandlers(CleanupUtility cln)
        {
            cleanupUtility = cln;
        }

        public void OnRoundStart()
        {
            Plugin.Coroutines.Add(Timing.RunCoroutine(CleanupCoroutine()));
        }

        public void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            if (!ev.IsAllowed)
                return;

            foreach (var item in Pickup.List)
            {
                if (item == null)
                    continue;

                var room = Map.FindParentRoom(item.GameObject);
                if (room != null && room.Zone == Exiled.API.Enums.ZoneType.LightContainment)
                    item.Destroy();
            }
        }

        public void OnDetonated()
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
        }
    }
}