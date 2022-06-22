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

        private IEnumerator<float> CleanupCoroutine()
        {
            while (Round.IsStarted)
            {
                yield return Timing.WaitForSeconds(cleanupUtility.Config.CleanDelay);

                foreach (var ragdoll in Map.Ragdolls)
                {
                    if (ragdoll != null)
                        ragdoll.Delete();
                }
                    

                foreach (var item in Map.Pickups)
                {
                    if (item != null)
                        item.Destroy();
                }
            }
        }
    }
}