namespace ScuutCore.Patches
{
    using HarmonyLib;
    using InventorySystem.Items;
    using PluginAPI.Core;

    [HarmonyPatch(typeof(ItemBase), nameof(ItemBase.ServerDropItem))]
    public static class DropItemPatch
    {
        public static bool Prefix(ItemBase __instance)
        {
            if (__instance.PickupDropModel == null)
            {
                var stackTrace = new System.Diagnostics.StackTrace();
                Log.Error("PickupDropModel is null! Stack trace: " + stackTrace);
                return false;
            }

            return true;
        }
    }
}