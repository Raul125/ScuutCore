namespace ScuutCore.Patches
{
    using HarmonyLib;
    using InventorySystem.Items.Usables.Scp330;
    using ScuutCore.Modules.BetterCandy;

    [HarmonyPatch(typeof(Scp330Candies), nameof(Scp330Candies.GetRandom))]
    public static class CandyGetRandomPatch
    {
        public static bool Prefix(ref CandyKindID __result)
        {
            if (BetterCandy.Singleton == null || !BetterCandy.Singleton.Config.IsEnabled)
                return true;
            if (Plugin.Random.Next(1, BetterCandy.Singleton.Config.MaxRandomizer) == BetterCandy.Singleton.Config.ChoosenNumber)
            {
                __result = CandyKindID.Pink;
                return false;
            }

            return true;
        }
    }
}