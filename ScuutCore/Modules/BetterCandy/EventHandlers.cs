using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Scp330;

namespace ScuutCore.Modules.BetterCandy
{
    public class EventHandlers
    {
        private BetterCandy betterCandy;
        public EventHandlers(BetterCandy btc)
        {
            betterCandy = btc;
        }

        public void OnInteractingWithScp330(InteractingScp330EventArgs ev)
        {
            ev.ShouldSever = false;

            if (ev.UsageCount == betterCandy.Config.PickCandyTimes)
                ev.ShouldSever = true;

            if (Plugin.Random.Next(1, betterCandy.Config.MaxRandomizer) == betterCandy.Config.ChoosenNumber)
            {
                ev.IsAllowed = false;
                ev.Player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
            }
        }
    }
}