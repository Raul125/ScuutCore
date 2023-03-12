namespace ScuutCore.Patches
{
    using System.Diagnostics;
    using HarmonyLib;
    using InventorySystem.Items;
    using Debug = UnityEngine.Debug;
    [HarmonyPatch(typeof(ItemBase), nameof(ItemBase.ServerDropItem))]
    public static class DropItemPatch
    {
        public static bool Prefix(ItemBase __instance)
        {
            Debug.Log($"drop item of type {__instance.ItemTypeId}");
            string trace = new StackTrace().ToString();
            if (trace.Contains("ScuutCore"))
                Debug.Log(trace);
            return true;
        }
    }
}