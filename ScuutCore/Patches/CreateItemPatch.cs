namespace ScuutCore.Patches
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using HarmonyLib;
    using InventorySystem;
    using InventorySystem.Items;

    [HarmonyPatch(typeof(Inventory), nameof(Inventory.CreateItemInstance))]
    public static class CreateItemPatch
    {
        public static void Postfix(ItemBase __result)
        {
            if(__result == null)
                return;
            ItemCreationCache.Cache.Add(__result, $"stacktrace: {new StackTrace()}, itemtype: {__result.ItemTypeId}, serial: {__result.ItemSerial}, owner: {__result.Owner}");
        }
    }

    public static class ItemCreationCache
    {
        public static Dictionary<ItemBase, string> Cache = new Dictionary<ItemBase, string>();
    }
}