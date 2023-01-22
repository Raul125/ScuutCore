using InventorySystem.Items.Pickups;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.BetterCandy
{
    using InventorySystem.Items.Usables.Scp330;

    public class EventHandlers
    {
        private BetterCandy betterCandy;
        public EventHandlers(BetterCandy btc)
        {
            betterCandy = btc;
        }

        //[PluginEvent(ServerEventType.PlayerPickupScp330)] using patch
        public void OnInteractingWithScp330(Player player, ItemPickupBase item)
        {
            if (Plugin.Random.Next(1, betterCandy.Config.MaxRandomizer) == betterCandy.Config.ChoosenNumber)
            {
                if (item is Scp330Pickup candypickup && Scp330Bag.TryGetBag(player.ReferenceHub, out Scp330Bag bag))
                {
                    var removed = bag.TryRemove(bag.Candies.FindIndex(x => x == candypickup.ExposedCandy));
                    if (removed == CandyKindID.None)
                        return;
                    if (bag.TryAddSpecific(CandyKindID.Pink))
                        bag.ServerRefreshBag();
                }
            }
        }
    }
}