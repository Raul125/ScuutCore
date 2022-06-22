using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

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

            foreach (var item in Map.Pickups)
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
            foreach (var item in Map.Pickups)
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
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(cleanupUtility.Config.CleanDelay);

                if (cleanupUtility.Config.DestroyRagdolls)
                {
                    foreach (var ragdoll in Map.Ragdolls)
                    {
                        if (ragdoll != null)
                            ragdoll.Delete();
                    }
                }

                if (cleanupUtility.Config.ClearItems)
                {
                    foreach (var item in Map.Pickups)
                    {
                        if (item != null)
                            item.Destroy();
                    }
                }
            }
        }
    }
}