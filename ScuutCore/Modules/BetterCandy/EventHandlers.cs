using InventorySystem.Items.Pickups;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace ScuutCore.Modules.BetterCandy
{
    public class EventHandlers
    {
        private BetterCandy betterCandy;
        public EventHandlers(BetterCandy btc)
        {
            betterCandy = btc;
        }

        /*[PluginEvent(ServerEventType.PlayerPickupScp330)]
        public void OnInteractingWithScp330(Player player, ItemPickupBase item)
        {
            ev.ShouldSever = false;

            if (ev.UsageCount == betterCandy.Config.PickCandyTimes)
                ev.ShouldSever = true;

            if (Plugin.Random.Next(1, betterCandy.Config.MaxRandomizer) == betterCandy.Config.ChoosenNumber)
            {
                ev.IsAllowed = false;
                ev.Player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
            }
        }*/
    }
}