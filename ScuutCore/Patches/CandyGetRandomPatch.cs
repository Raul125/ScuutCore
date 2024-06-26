﻿namespace ScuutCore.Patches;

using HarmonyLib;
using InventorySystem.Items.Usables.Scp330;
using ScuutCore.Modules.BetterCandy;

[HarmonyPatch(typeof(Scp330Candies), nameof(Scp330Candies.GetRandom))]
public static class CandyGetRandomPatch
{
    // we can modify CandyPink.Weight instead of this goofy prefix.
    // this will require config changes though.
    public static bool Prefix(ref CandyKindID __result)
    {
        if (BetterCandy.Singleton is null || BetterCandy.Singleton.Config.IsEnabled is false)
            return true;

        if (Plugin.Random.Next(1, BetterCandy.Singleton.Config.MaxRandomizer) == BetterCandy.Singleton.Config.ChoosenNumber)
        {
            __result = CandyKindID.Pink;
            return false;
        }

        return true;
    }
}